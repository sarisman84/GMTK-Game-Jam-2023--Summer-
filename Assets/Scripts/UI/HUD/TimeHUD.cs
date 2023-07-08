using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimeHUD : MonoBehaviour
{
    private TextMeshProUGUI text;
    const float zeroEpsilon = 0.05f;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if(BackendManager.Get.gameTime <= zeroEpsilon) {
            text.text = "00:00.00";
            return;
        }
        text.text = TimeSpan.FromSeconds(BackendManager.Get.gameTime).ToString("mm\\:ss\\.ff");
    }
}
