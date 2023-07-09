using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Upgrade", menuName = "Upgrades/Health", order = 0)]
public class HealthUpgrade : BaseUpgradeObject {

    public float additionalMaxHealth;
    public override void OnUpdate(PlayerController aController)
    {
        aController.maxHealth = aController.baseMaxHealth + (upgradeCount * additionalMaxHealth);
    }
}