using UnityEngine;
using UnityEngine.UI;

public class VialHUD : MonoBehaviour, IManager
{
    public Image blood;

    public void OnLoad() {
        SetVialFill(0.0f);//start empty
    }

    public void SetVialFill(float fill0To1) {
        blood.material.SetFloat("_Fill", fill0To1);
    }
}
