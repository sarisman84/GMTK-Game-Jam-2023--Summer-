using UnityEngine;
using UnityEditor;


// A tiny custom editor for ExampleScript component
[CustomEditor(typeof(BaseWeaponObject))]
public class ExampleEditor : Editor {
    // Custom in-scene UI for when ExampleScript
    // component is selected.

    PlayerController player;
    private void OnEnable()
    {
        player = PollingStation.Get<PlayerController>();
    }

    public void OnSceneGUI()
    {


        var t = target as BaseWeaponObject;
        var tr = BaseWeaponObject.GetClosestDamagable(player, t.attackRange);
        var pos = tr.transform.position;
        var size = tr.transform.localScale;
        // display an orange disc where the object is
        var color = new Color(1, 0.8f, 0.4f, 1);
        Handles.color = color;
        Handles.DrawWireCube(pos, size);
        // display object "value" in scene
        GUI.color = color;
        Handles.Label(pos, tr.health.ToString());
    }
}