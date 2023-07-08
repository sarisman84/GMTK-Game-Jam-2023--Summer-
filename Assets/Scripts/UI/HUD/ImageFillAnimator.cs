using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageFillAnimator : MonoBehaviour
{
    private Image image;
    public float fullTime;
    public float currentTime;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Update() {
        if (!BackendManager.Get.runtimeActive) return;
        if (currentTime <= 0) return;
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0);
        image.fillAmount = currentTime / fullTime;
    }

    public void StartCooldown(float cooldownTime) {
        fullTime = cooldownTime;
        currentTime = fullTime;
    }
}
