using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GizmoDrawer : MonoBehaviour, IManager {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnStart()
    {
        GameObject gizmoDrawer = new GameObject("Gizmo Drawer [DEBUG]");
        gizmoDrawer.AddComponent<GizmoDrawer>();
    }

    List<System.Action> _gizmoDraw = new List<System.Action>();

    public System.Action gizmoDraw
    {
        set
        {
            _gizmoDraw.Add(value);
        }
        private get { return _gizmoDraw[_gizmoDraw.Count - 1]; }
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < _gizmoDraw.Count; i++)
        {
            _gizmoDraw[i]();
        }
    }


    public void OnLoad()
    {

    }

}
