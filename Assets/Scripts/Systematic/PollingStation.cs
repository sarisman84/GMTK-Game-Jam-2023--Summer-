using System.Collections.Generic;
using UnityEngine;

public class PollingStation : MonoBehaviour
{
    private static PollingStation instance;
    public static bool quitting = false;

    public static PollingStation Get() {
        if(instance != null) 
            return instance;//return the instance, if found already


        instance = FindObjectOfType<PollingStation>();
        if (instance != null) {
            if (instance.managers == null)
                instance.LoadManagers();
            return instance;
        }  

        return null;
    }


    private Dictionary<System.Type, Manager> managers;
    private void LoadManagers() {
        DontDestroyOnLoad(gameObject);//only dont destroy on load this gameObject -> all managers to persist between scenes should be a child (NOTE: all others wont be loaded tho)
        managers = new Dictionary<System.Type, Manager>();

        Manager[] loadedManagers = FindObjectsOfType<Manager>(true);
        foreach(Manager manager in loadedManagers) {
            if (!managers.ContainsKey(manager.GetType())) {
                managers.Add(manager.GetType(), manager);
                manager.OnLoad();
            }
            else {
                Debug.LogError(manager.gameObject.name + " of type " + manager.GetType() + " was a dublicate");
            }
        }
    }


    public static T Get<T>() where T : Manager {
        return (T)Get()?.managers[typeof(T)];
    }


    private void OnApplicationQuit() {
        quitting = true;
    }
}
