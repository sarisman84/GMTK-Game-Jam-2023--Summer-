using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GizmoDrawer : MonoBehaviour, IManager {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnStart()
    {
        GameObject gizmoDrawer = new GameObject("Gizmo Drawer [DEBUG]");
        gizmoDrawer.AddComponent<GizmoDrawer>();

        DontDestroyOnLoad(gizmoDrawer);
    }

    public event System.Action gizmoDraw;


    private void OnDrawGizmos()
    {
        if (gizmoDraw != null)
            gizmoDraw();
    }


    public void OnLoad()
    {

    }

}
