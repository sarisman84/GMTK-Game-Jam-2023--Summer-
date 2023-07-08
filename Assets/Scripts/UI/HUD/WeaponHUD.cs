using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour, IManager
{
    public Transform[] weaponGroups;
    public GameObject weaponImagePrefab;
    private Dictionary<BaseWeaponObject, Image> weaponToId = new Dictionary<BaseWeaponObject, Image>();

    public void OnLoad() {

    }

    public Image AddWeaponImage(BaseWeaponObject weapon) {
        int shortestRow = weaponGroups.Length - 1;
        int minCount = int.MaxValue;
        for (int i = weaponGroups.Length-1; i >= 0; i--) {
            if (weaponGroups[i].childCount > minCount)
                continue;

            shortestRow = i;
            minCount = weaponGroups[i].transform.childCount;
        }

        Image weaponImage = Instantiate(weaponImagePrefab, weaponGroups[shortestRow]).GetComponent<Image>();
        weaponImage.sprite = weapon.icon;
        weaponToId.Add(weapon, weaponImage);
        return weaponImage;
    }

    /*
    public Image GetWeaponImage(int i) {
        int iSum = 0;
        for (int j = 0; j < weaponGroups.Length; j++) {
            iSum += weaponGroups[j].childCount;
            if(iSum > i) {
                return weaponGroups[j].GetChild(i - (iSum - weaponGroups[j].childCount)).GetComponent<Image>();
            }
        }
        return null;
    }
    */

    public void StartCooldown(BaseWeaponObject weapon, float cooldownSec) {
        weaponToId[weapon]
            .transform
            .GetChild(0)
            .GetComponent<ImageFillAnimator>()
            .StartCooldown(cooldownSec);
    }
}
