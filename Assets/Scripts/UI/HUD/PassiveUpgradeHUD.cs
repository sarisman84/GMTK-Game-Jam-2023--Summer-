using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeHUD : MonoBehaviour, IManager {

    public GameObject passivePrefab;
    public Dictionary<BaseUpgradeObject, Image> upgradeToImage = new Dictionary<BaseUpgradeObject, Image>();
    
    public void OnLoad() {}

    public void AddPassiveUpgrade(BaseUpgradeObject upgrade, int upgradeCount) {
        Image img;
        if (upgradeToImage.ContainsKey(upgrade)) {
            img = upgradeToImage[upgrade];
        }
        else {
            img = Instantiate(passivePrefab, transform).GetComponent<Image>();
            img.sprite = upgrade.icon;
            upgradeToImage.Add(upgrade, img);
        }

        TextMeshProUGUI tmpText = img.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (upgradeCount > 1) {
            tmpText.text = $"x{upgradeCount}";
        }
        else {
            tmpText.text = "";
        }
    }

    public void Clear() {
        upgradeToImage.Clear();
        for (int i = transform.childCount; i >= 0; i--) {//destroy all children
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
