using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HealthBar : MonoBehaviour, IManager
{
    [SerializeField]
    private RectTransform healthBlock;
    [SerializeField]
    private TextMeshProUGUI healthText;

    private RectTransform rect;

    public void OnLoad() {
        rect = GetComponent<RectTransform>();
        SetHealthPercent(1.0f);
    }

    public void GetHealth(Damagable oj) {
        SetHealthPercent(oj.health / oj.maxHealth);
    }

    public void SetHealthPercent(float healthZeroToOne) {
        healthBlock.sizeDelta = new Vector2(rect.sizeDelta.x * healthZeroToOne, healthBlock.sizeDelta.y);
        healthText.text = Mathf.RoundToInt(healthZeroToOne * 100).ToString() + "% HP";
    }
}
