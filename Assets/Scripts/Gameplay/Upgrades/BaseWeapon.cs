using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Weapon", menuName = "Upgrades/Weapons/Basic", order = 0)]
public class BaseWeapon : BaseWeaponObject {
    public override void OnUpdate(PlayerController aController)
    {
        currentAttackRate += Time.deltaTime;
        if (currentAttackRate >= attackSpeed)
        {
            currentAttackRate = 0;
            Damagable damagable = GetClosestDamagable(aController, attackRange);
            if(damagable != null)
            {
                damagable.Hit(attackDamage, aController);
                Debug.Log($"Attacking {damagable.gameObject.name}![Remaining health: {damagable.health}]");
            }
      
        }
    }
}
