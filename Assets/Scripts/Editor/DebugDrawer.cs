using UnityEngine;
using UnityEditor;

public class DebugDrawer {
   

    static DebugDrawer instance;

    public static DebugDrawer Get
    {
        get
        {
            if (instance == null)
                instance = new DebugDrawer();

            return instance;
        }
    }

    DebugDrawer()
    {
        SceneView.duringSceneGui += OnScene;
    }


    void OnScene(SceneView sceneView)
    {

    }



    public static void DrawCone(Vector3 originPoint, float anAngleInDegrees)
    {

    }



    public void OnSceneGUI()
    {


        //var t = target as BaseWeaponObject;
        //var tr = BaseWeaponObject.GetClosestDamagable(player, t.attackRange);
        //var pos = tr.transform.position;
        //var size = tr.transform.localScale;
        //// display an orange disc where the object is
        //var color = new Color(1, 0.8f, 0.4f, 1);
        //Handles.color = color;
        //Handles.DrawWireCube(pos, size);
        //// display object "value" in scene
        //GUI.color = color;
        //Handles.Label(pos, tr.health.ToString());
    }
}