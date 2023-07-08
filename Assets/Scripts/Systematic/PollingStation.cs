using System.Collections.Generic;
using UnityEngine;

public class PollingStation
{
    private static PollingStation instance;
    public static bool quitting = false;

    public static PollingStation Get() {
        if(instance != null) 
            return instance;//return the instance, if found already

        /*
        instance = FindObjectOfType<PollingStation>();
        if (instance != null) {
            if (instance.managers == null)
                instance.LoadManagers();
            return instance;
        }*/

        instance = new PollingStation();
        if (instance.managers == null)
            instance.LoadManagers();
        return instance;
    }


    private Dictionary<System.Type, MonoBehaviour> managers;
    private void LoadManagers() {
        //MonoBehaviour.DontDestroyOnLoad(gameObject);//only dont destroy on load this gameObject -> all managers to persist between scenes should be a child (NOTE: all others wont be loaded tho)
        managers = new Dictionary<System.Type, MonoBehaviour>();

        MonoBehaviour[] ojs = Object.FindObjectsOfType<MonoBehaviour>();
        foreach(MonoBehaviour manager in ojs) {
            if (!typeof(IManager).IsAssignableFrom(manager.GetType())) continue;//skip all non-Managers

            if (!managers.ContainsKey(manager.GetType())) {
                managers.Add(manager.GetType(), manager);
                (manager as IManager).OnLoad();
            }
            else {
                Debug.LogError(manager.gameObject.name + " of type " + manager.GetType() + " was a dublicate");
            }
        }
    }

#if UNITY_EDITOR
    public static T Get<T>() where T : MonoBehaviour, IManager {
        if (UnityEditor.EditorApplication.isPlaying) {
            return (T)Get()?.managers[typeof(T)];
        }
        else {
            return Object.FindObjectOfType<T>();//this is only for edit mode (slow)
        }
    }
#else
    public static T Get<T>() where T : MonoBehaviour, IManager {
        return (T)Get()?.managers[typeof(T)];
    }
#endif

    /*
    private void OnApplicationQuit() {
        quitting = true;
    }*/
}
