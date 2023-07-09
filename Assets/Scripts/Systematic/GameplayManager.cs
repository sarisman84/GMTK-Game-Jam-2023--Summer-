using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayManager : MonoBehaviour, IManager {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnGlobalStart()
    {
        GameObject go = BackendManager.CreateOrFetchManager<GameplayManager>().gameObject;

        DontDestroyOnLoad(go);
    }

    PlayerController player;


    public event Action onGameOverEvent;

    public void OnLoad()
    {
        
    }


    public void Start()
    {
        player = PollingStation.Get<PlayerController>();
        player.SetActive(false);
    }

    public void SpawnPlayer()
    {
        player.SetActive(true);
        player.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
    }

    public void GameOver()
    {
        FindObjectOfType<AudioManager>().StopPlaying("ingamemusic");
        FindObjectOfType<AudioManager>().Play("death");
        player.SetActive(false);



        if (onGameOverEvent != null)
            onGameOverEvent();
    }

    public void Quit()
    {
        Application.Quit();
    }


}
