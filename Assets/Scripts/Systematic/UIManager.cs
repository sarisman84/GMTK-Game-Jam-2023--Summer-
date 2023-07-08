using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType {
    MainMenu,
    PauseMenu,
    Credits,
    Settings,
    GameOver,
    IngameHUD
}

public enum FadeMode {
    In, Out
}

public class UIManager : MonoBehaviour, IManager {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnGlobalStart()
    {
        GameObject manager = GameObject.Find("Systems") ?? new GameObject("Systems");
        UIManager isValid = manager.GetComponent<UIManager>() ?? manager.AddComponent<UIManager>();

        Debug.Log($"{isValid.name} loaded!");

        DontDestroyOnLoad(manager.gameObject);
    }

    public void OnLoad()
    {

    }



    public CanvasGroup
        mainMenu,
        pauseMenu,
        credits,
        settings,
        gameOver,
        ingameHUD;

    [Space]
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;





    public void LoadMenu(int aMenuType)
    {
        ResetAllCanvases();
        switch ((MenuType)aMenuType)
        {
            case MenuType.MainMenu:
                SetCanvasActive(mainMenu, true);
                break;
            case MenuType.PauseMenu:
                SetCanvasActive(pauseMenu, true);
                break;
            case MenuType.Credits:
                SetCanvasActive(credits, true);
                break;
            case MenuType.Settings:
                SetCanvasActive(settings, true);
                break;
            case MenuType.GameOver:
                SetCanvasActive(gameOver, true);
                break;
            case MenuType.IngameHUD:
                SetCanvasActive(ingameHUD, true);
                break;
            default:
                break;
        }
    }



    private void ResetAllCanvases()
    {
        SetCanvasActive(mainMenu, false);
        SetCanvasActive(pauseMenu, false);
        SetCanvasActive(credits, false);
        SetCanvasActive(settings, false);
        SetCanvasActive(gameOver, false);
        SetCanvasActive(ingameHUD, false);
    }


    private void SetCanvasActive(CanvasGroup aCanvas, bool aNewState)
    {
        if (!aCanvas) return;

        StartCoroutine(FadeCoroutine(aCanvas, aNewState ? FadeMode.In : FadeMode.Out, fadeOutDuration, fadeInDuration));

        for (int i = 0; i < aCanvas.transform.childCount; i++)
        {
            GameObject obj = aCanvas.transform.GetChild(i).gameObject;

            obj.SetActive(aNewState);
        }
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
