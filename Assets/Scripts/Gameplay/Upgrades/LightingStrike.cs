using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lighting Strike", menuName = "Upgrades/Weapons/Lighting Strike", order = 0)]
public class LightingStrike : BaseWeaponObject {

    public int baseAmountOfLightingStrikes;
    public int additionalAmountOfLightingStrikes;

    public int amountOfLightingStrikes
    {
        private set;
        get;
    }

    public override void OnUpdateWeapon(UpgradeManager aController)
    {
        var foundEnemies = GetAllDamagablesInRange(aController.player, attackRange);
        if (foundEnemies.Count == 0) return;
        for (int i = 0; i < amountOfLightingStrikes; i++)
        {
            var enemy = foundEnemies[Random.Range(0, foundEnemies.Count)];

            enemy.Hit(attackDamage, aController);
        }

    }

    public override void OnUpgrade(UpgradeManager aManager)
    {
        base.OnUpgrade(aManager);

        amountOfLightingStrikes = baseAmountOfLightingStrikes + (upgradeCount * additionalAmountOfLightingStrikes);
    }


    public override void OnDrawGizmo(UpgradeManager aController)
    {
        Handles.color = Color.cyan;
        Handles.DrawWireArc(aController.transform.position, Vector3.up, Vector3.forward, 360.0f, attackRange);
    }
}
