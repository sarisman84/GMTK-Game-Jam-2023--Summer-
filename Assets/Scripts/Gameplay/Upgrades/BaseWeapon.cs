using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Weapon", menuName = "Upgrades/Weapons/Basic", order = 0)]
public class BaseWeapon : BaseWeaponObject {

    public float detectionAngle = 20.0f;

    public override void OnUpdate(PlayerController aController)
    {
        currentAttackRate += Time.deltaTime;
        if (currentAttackRate >= attackSpeed)
        {
            currentAttackRate = 0;

            DetectionDesc desc = new DetectionDesc();

            desc.viewDirection = GetDirectionToClosestDamagable(aController, attackRange);
            desc.detectionRadius = attackRange;
            desc.viewAngleInDegrees = detectionAngle;
            desc.originPoint = aController;

            var foundDamagables = GetAllDamagablesInAView(desc);

            foreach (var item in foundDamagables)
            {
                item.Hit(attackDamage, aController);
            }

            Debug.Log($"Total Enemy Hit Count: {foundDamagables.Count}");
      
        }
    }
}
