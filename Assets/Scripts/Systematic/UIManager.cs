using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FadeMode {
    In, Out
}

public class UIManager : MonoBehaviour, IManager {
    public void OnLoad()
    {

    }

    public UnityEvent onGameOver;
    public UnityEvent onGameStart;
    public UnityEvent onRuntimeInit;
    public UnityEvent onGamePaused;


    void Start()
    {
        PollingStation.Get<GameplayManager>().onGameOverEvent += () => { onGameOver.Invoke(); };
        PollingStation.Get<GameplayManager>().onGameStartEvent += () => { onGameStart.Invoke(); };
        onRuntimeInit.Invoke();
    }

    public void StartGame()
    {
        PollingStation.Get<GameplayManager>().GameStart();
        PollingStation.Get<AudioManager>().Play("ingamemusic");
    }


    public void PauseGame()
    {
        PollingStation.Get<GameplayManager>().PauseGame();
        onGamePaused?.Invoke();
    }

    public void Unpause()
    {
        PollingStation.Get<GameplayManager>().UnpauseGame();
    }

    public void Quit()
    {
        Application.Quit();
    }


    public static IEnumerator FadeCoroutine(CanvasGroup aCanvas, FadeMode aMode, float aFadeOutDuration, float aFadeInDuration)
    {
        float elapsed = 0.0f;

        while (aMode == FadeMode.Out ? (elapsed < aFadeOutDuration) : (elapsed < aFadeInDuration))
        {
            float alpha = Mathf.Lerp(
                aMode == FadeMode.Out ? 1.0f : 0.0f,
                aMode == FadeMode.Out ? 0.0f : 1.0f,
                aMode == FadeMode.Out ? (elapsed / aFadeOutDuration) : (elapsed / aFadeInDuration));

            aCanvas.alpha = alpha;

            elapsed += Time.deltaTime;

            yield return null;
        }

        aCanvas.alpha = aMode == FadeMode.Out ? 0.0f : 1.0f;
    }
}
