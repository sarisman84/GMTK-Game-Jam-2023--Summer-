using System.Collections;
using UnityEngine;


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
