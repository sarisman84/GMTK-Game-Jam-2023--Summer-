using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InspectAttribute))]
public class ViewAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUIUtility.singleLineHeight;
        if (property.objectReferenceValue == null || !AreAnySubPropertiesVisible(property))
        {
            return totalHeight;
        }
        if (property.isExpanded)
        {
            totalHeight += (EditorGUIUtility.singleLineHeight * 2.0f) * 1.15f;
            var data = property.objectReferenceValue as ScriptableObject;
            if (data == null) return EditorGUIUtility.singleLineHeight;
            SerializedObject serializedObject = new SerializedObject(data);
            SerializedProperty prop = serializedObject.GetIterator();

            var headerProps = GetAttributeProperties(serializedObject, typeof(HeaderAttribute));
            var spaceProps = GetAttributeProperties(serializedObject, typeof(SpaceAttribute));

            if (prop.NextVisible(true))
            {
                do
                {
                    if (prop.name == "m_Script") continue;

                    var p = headerProps.Find(x => x.Contains(prop));

                    if (p.HasAttribute())
                    {
                        //Debug.Log("Found a header att");
                        totalHeight += EditorGUIUtility.singleLineHeight * 1.15f;
                    }


                    p = spaceProps.Find(x => x.Contains(prop));

                    if (p.HasAttribute())
                    {
                        var spaceAtt = p.GetAttribute<SpaceAttribute>();

                        totalHeight += (EditorGUIUtility.singleLineHeight + spaceAtt.height) * 1.15f;
                    }


                    var subProp = serializedObject.FindProperty(prop.name);
                    float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
                    totalHeight += height;
                }
                while (prop.NextVisible(false));
            }
            // Add a tiny bit of height if open for the background
            totalHeight += EditorGUIUtility.standardVerticalSpacing;
        }
        return totalHeight;
    }

    const int buttonWidth = 66;

    static readonly List<string> ignoreClassFullNames = new List<string> { "TMPro.TMP_FontAsset" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InspectAttribute at = attribute as InspectAttribute;
        EditorGUI.BeginProperty(position, label, property);
        var type = GetFieldType();

        if (type == null || ignoreClassFullNames.Contains(type.FullName))
        {
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
            return;
        }

        ScriptableObject propertySO = null;
        if (!property.hasMultipleDifferentValues && property.serializedObject.targetObject != null && property.serializedObject.targetObject is ScriptableObject)
        {
            propertySO = (ScriptableObject)property.serializedObject.targetObject;
        }

        var propertyRect = Rect.zero;
        var guiContent = new GUIContent(string.IsNullOrEmpty(at.displayName) ? property.displayName : at.displayName);
        var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        if (property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
        {
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true);
        }
        else
        {
            // So yeah having a foldout look like a label is a weird hack 
            // but both code paths seem to need to be a foldout or 
            // the object field control goes weird when the codepath changes.
            // I guess because foldout is an interactable control of its own and throws off the controlID?
            foldoutRect.x += 12;
            EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true, EditorStyles.label);
        }
        var indentedPosition = EditorGUI.IndentedRect(position);
        var indentOffset = indentedPosition.x - position.x;
        propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset), EditorGUIUtility.singleLineHeight);

        if (propertySO != null || property.objectReferenceValue == null)
        {
            propertyRect.width -= buttonWidth;
        }

        property.objectReferenceValue = EditorGUI.ObjectField(propertyRect, GUIContent.none, property.objectReferenceValue, type, false);
        if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

        var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

        if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
        {
            var data = (ScriptableObject)property.objectReferenceValue;

            if (property.isExpanded)
            {
                // Draw a background that shows us clearly which fields are part of the ScriptableObject
                if (at.displayBackground)
                    GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

                EditorGUI.indentLevel++;
                SerializedObject serializedObject = new SerializedObject(data);


                var headerProps = GetAttributeProperties(serializedObject, typeof(HeaderAttribute));
                var spaceProps = GetAttributeProperties(serializedObject, typeof(SpaceAttribute));

                float y = position.y + EditorGUIUtility.singleLineHeight * 1.15f;

                var h = headerProps.Find(x => x.Contains(property));
                var s = spaceProps.Find(x => x.Contains(property));

                // Iterate over all the values and draw them
                SerializedProperty prop = serializedObject.GetIterator();
                if (prop.NextVisible(true))
                {
                    do
                    {
                        // Don't bother drawing the class file
                        if (prop.name == "m_Script") continue;
                        float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                        EditorGUI.PropertyField(new Rect(position.x, y, position.width - buttonWidth, height), prop, true);
                        y += height + EditorGUIUtility.standardVerticalSpacing;
                    }
                    while (prop.NextVisible(false));
                }
                if (GUI.changed)
                    serializedObject.ApplyModifiedProperties();

                EditorGUI.indentLevel--;
            }
        }
        else
        {
            var to = property.serializedObject.targetObject;

            if (to.GetType().IsSubclassOf(typeof(ScriptableObject)))
            {
                if (GUI.Button(buttonRect, "Create"))
                {

                    string selectedAssetPath = "Assets";
                    if (property.serializedObject.targetObject is MonoBehaviour)
                    {



                        MonoScript ms = MonoScript.FromMonoBehaviour((MonoBehaviour)property.serializedObject.targetObject);
                        selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
                    }

                    property.objectReferenceValue = CreateAssetWithSavePrompt(type, selectedAssetPath);
                }
            }


        }
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }


    struct SerializedAttribute
    {
        public enum AttributeType
        {
            Header, Space
        }
        public SerializedAttribute(SerializedProperty aProperty, object anAttribute, AttributeType aType)
        {
            myProperty = aProperty;
            myAttribute = anAttribute;
            myAttributeType = aType;
        }
        public SerializedProperty myProperty;

        public AttributeType myAttributeType;


        public bool Contains(SerializedProperty aProperty)
        {
            return myProperty == aProperty;
        }

        public T GetAttribute<T>() where T : PropertyAttribute
        {
            return (T)myAttribute;
        }

        public bool HasAttribute()
        {
            return myAttribute != null;
        }


        object myAttribute;
    }


    private static List<SerializedAttribute> GetAttributeProperties(SerializedObject anObject, Type aTypeToSearch)
    {
        List<SerializedAttribute> headerProperties = new List<SerializedAttribute>();
        SerializedProperty prop = anObject.GetIterator();
        while (prop.NextVisible(true))
        {
            var field = anObject.targetObject.GetType().GetField(prop.name);
            if (field == null) continue;
            var r = field.GetCustomAttributes(aTypeToSearch, true);
            if (r == null || r.Length <= 0) continue;
            var headerAttribute = r[0];
            if (headerAttribute != null)
            {
                headerProperties.Add(new SerializedAttribute(prop, headerAttribute, aTypeToSearch == typeof(HeaderAttribute) ? SerializedAttribute.AttributeType.Header : SerializedAttribute.AttributeType.Space));
            }
        }
        return headerProperties;
    }

    // Creates a new ScriptableObject via the default Save File panel
    static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
    {
        path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", path);
        if (path == "") return null;
        ScriptableObject asset = ScriptableObject.CreateInstance(type);
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        EditorGUIUtility.PingObject(asset);
        return asset;
    }

    Type GetFieldType()
    {
        Type type = fieldInfo.FieldType;
        if (type.IsArray) type = type.GetElementType();
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
        return type;
    }

    static bool AreAnySubPropertiesVisible(SerializedProperty property)
    {
        var data = (ScriptableObject)property.objectReferenceValue;
        SerializedObject serializedObject = new SerializedObject(data);
        SerializedProperty prop = serializedObject.GetIterator();
        while (prop.NextVisible(true))
        {
            if (prop.name == "m_Script") continue;
            return true; //if theres any visible property other than m_script
        }
        return false;
    }
}
