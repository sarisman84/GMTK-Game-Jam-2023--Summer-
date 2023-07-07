using System.Collections;
using UnityEngine;


public abstract class BaseWeaponObject : ScriptableObject {

    public float baseAttackDamage;
    public float baseAttackSpeed;
    public float baseAttackRange;
    [Space]
    public float additionalAttackDamage;
    public float additionalAttackSpeed;
    public float additionalAttackRange;

    [HideInInspector] public int upgradeCount;

    protected float currentAttackRate;


    public float attackDamage
    {
        get { return baseAttackDamage + (upgradeCount * additionalAttackDamage); }
    }

    public float attackRange
    {
        get { return baseAttackRange + (upgradeCount * additionalAttackRange); }
    }

    public float attackSpeed
    {
        get { return baseAttackSpeed - (upgradeCount * additionalAttackSpeed); }
    }


    public static Damagable GetClosestDamagable(Damagable anOrigin, float aDetectionRadius)
    {
        Collider[] foundColliders = Physics.OverlapSphere(anOrigin.transform.position, aDetectionRadius);

        if (foundColliders.Length == 0) return null;


        float length = float.MaxValue;
        Damagable result = null;

        foreach (var item in foundColliders)
        {
            if (item.gameObject == anOrigin.gameObject) continue;


            float tLength = (item.transform.position - anOrigin.transform.position).sqrMagnitude;

            if (length > tLength)
            {
                length = tLength;
                result = item.GetComponent<Damagable>();
            }
        }
        return result;
    }
    
    public abstract void OnUpdate(PlayerController aController);
}
