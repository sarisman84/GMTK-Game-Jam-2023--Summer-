using UnityEngine;
using UnityEngine.UI;

public class VialHUD : MonoBehaviour, IManager
{
    public Image blood;

    public void OnLoad() {
        SetVialFill(0.0f);//start empty
        PollingStation.Get<GameplayManager>().onGameStartEvent += () => SetVialFill(0.0f);//empty the vial on game over
    }

    public void SetVialFill(float fill0To1) {
        blood.material.SetFloat("_Fill", fill0To1);
    }
}
