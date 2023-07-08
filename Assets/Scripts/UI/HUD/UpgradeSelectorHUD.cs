using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeSelectorHUD : MonoBehaviour, IManager {

    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;

    public struct Selection {
        public Image background;
        public Button button;
        public TextMeshProUGUI descriptor;
    }

    public struct SelectionDesc {
        public Sprite icon;
        public string description;
        public Action selectionEvent;
    }


    private List<Selection> selectionButtons;
    private CanvasGroup alphaHandler;

    private void Awake()
    {
        FetchSelectionButtons();
        alphaHandler = GetComponentInChildren<CanvasGroup>();

        alphaHandler.alpha = 0;
    }

    private void OnValidate()
    {
        alphaHandler = alphaHandler ?? GetComponentInChildren<CanvasGroup>();
        alphaHandler.alpha = 0;
    }





    Selection GetSelectionComponents(Button aButton)
    {
        Selection selection = new Selection();
        selection.background = aButton.GetComponent<Image>();
        selection.button = aButton;
        selection.descriptor = aButton.GetComponentInChildren<TextMeshProUGUI>();

        return selection;
    }


    void FetchSelectionButtons()
    {
        selectionButtons = new List<Selection>();
        foreach (var item in GetComponentsInChildren<Button>())
        {
            selectionButtons.Add(GetSelectionComponents(item));
        }
    }


    public void SetSelections(SelectionDesc[] someDescriptions)
    {
        StartCoroutine(Transition(FadeMode.In));
        for (int i = 0; i < someDescriptions.Length; i++)
        {
            var selection = selectionButtons[i];


            Action selectionEvent = someDescriptions[i].selectionEvent;

            selection.button.onClick.RemoveAllListeners();
            selection.button.onClick.AddListener(() =>
            {
                selectionEvent();
                StartCoroutine(Transition(FadeMode.Out));
            });


            selection.descriptor.text = someDescriptions[i].description;
            selection.background.sprite = someDescriptions[i].icon;

        }
    }

    IEnumerator Transition(FadeMode aMode)
    {
        if (aMode == FadeMode.Out)
        {
            foreach (var item in selectionButtons)
            {
                item.button.onClick.RemoveAllListeners();
            }
        }
        yield return UIManager.FadeCoroutine(alphaHandler, aMode, fadeOutDuration, fadeInDuration);

        BackendManager.Get.runtimeActive = aMode == FadeMode.Out;

    }

    public void OnLoad()
    {
        Awake();
    }
}
