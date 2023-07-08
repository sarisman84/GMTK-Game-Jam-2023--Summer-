using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameplayManager : MonoBehaviour, IManager {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnGlobalStart()
    {
        GameObject manager = GameObject.Find("Systems") ?? new GameObject("Systems");
        GameplayManager isValid = manager.GetComponent<GameplayManager>() ?? manager.AddComponent<GameplayManager>();

        Debug.Log($"{isValid.name} loaded!");

        DontDestroyOnLoad(manager.gameObject);
    }

    PlayerController player;


    public event Action onGameOverEvent;

    public void OnLoad()
    {
        player = PollingStation.Get<PlayerController>();
    }


    public void Awake()
    {
        player.SetActive(false);
    }

    public void SpawnPlayer()
    {
        player.SetActive(true);
        player.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
    }

    public void GameOver()
    {
        player.SetActive(false);

        if (onGameOverEvent != null)
            onGameOverEvent();
    }

    public void Quit()
    {
        Application.Quit();
    }







}
