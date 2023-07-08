using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackendManager {

    static BackendManager instance;

    public static BackendManager Get
    {
        get
        {
            if (instance == null)
                instance = new BackendManager();

            return instance;
        }


    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void GlobalStart()
    {
        PollingStation.Get<PlayerController>().StartCoroutine(Get.UpdateEvent());
    }


    public static T CreateOrFetchManager<T>() where T : MonoBehaviour
    {
        GameObject manager = null;
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < rootObjects.Length; i++)
        {
            if (rootObjects[i].CompareTag("Backend"))
            {
                manager = rootObjects[i];
                break;
            }


        }

        manager = new GameObject(typeof(T).Name);

        T result = manager.GetComponent<T>();
        if (result == null)
        {
            result = manager.AddComponent<T>();
        }
        return result;


    }





    public bool runtimeActive = true;

    private float _gameTime = 0;


    public float gameTime
    {
        get
        {
            return _gameTime;
        }
    }


    IEnumerator UpdateEvent()
    {
        while (true)
        {
            if (runtimeActive)
            {
                _gameTime += Time.deltaTime;
            }
            yield return null;
        }
    }
}
