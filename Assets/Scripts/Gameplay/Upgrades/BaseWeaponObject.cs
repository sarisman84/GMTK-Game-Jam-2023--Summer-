using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DetectionDesc {
    public Damagable originPoint;
    public Vector3 viewDirection;
    public float detectionRadius;
    public float viewAngleInDegrees;
}


public abstract class BaseWeaponObject : ScriptableObject {

    public float baseAttackDamage;
    public float baseAttackSpeed;
    public float baseAttackRange;
    [Space]
    public float additionalAttackDamage;
    public float additionalAttackSpeed;
    public float additionalAttackRange;
    [Space]
    public string description;
    public string upgradeDescription;
    public Sprite icon;


    [HideInInspector] public int upgradeCount;

    protected float currentAttackRate;


    public float attackDamage
    {
        protected set;
        get;
        //get { return baseAttackDamage + (upgradeCount * additionalAttackDamage); }
    }

    public float attackRange
    {
        protected set;
        get;
        //get { return baseAttackRange + (upgradeCount * additionalAttackRange); }
    }

    public float attackSpeed
    {
        protected set;
        get;
        //get { return baseAttackSpeed - (upgradeCount * additionalAttackSpeed); }
    }

    public static List<Damagable> GetAllDamagablesInAView(DetectionDesc aDescription)
    {
        Collider[] foundColliders = Physics.OverlapSphere(aDescription.originPoint.transform.position, aDescription.detectionRadius);

        if (foundColliders.Length == 0) return null;

        aDescription.viewDirection.Normalize();

        List<Damagable> result = new List<Damagable>();

        foreach (var item in foundColliders)
        {
            Damagable damagable = item.GetComponent<Damagable>();
            if (!damagable) continue;
            if (damagable == aDescription.originPoint) continue;

            Vector3 directionToDamagable = (damagable.transform.position - aDescription.originPoint.transform.position).normalized;


            float dotProduct = Vector3.Dot(directionToDamagable, aDescription.viewDirection);
            float viewAngleInRadians = (aDescription.viewAngleInDegrees / 2.0f) * Mathf.Deg2Rad;

            if (Mathf.Acos(dotProduct) < viewAngleInRadians)
            {
                result.Add(damagable);
            }
        }

        return result;
    }

    public static List<Damagable> GetAllDamagablesInRange(Damagable damagable, float aDetectionRadius)
    {
        Collider[] foundColliders = Physics.OverlapSphere(damagable.transform.position, aDetectionRadius);

        if (foundColliders.Length == 0) return null;

        List<Damagable> result = new List<Damagable>();

        foreach (var item in foundColliders)
        {
            if (item.gameObject == damagable.gameObject) continue;

            if (item.GetComponent<Damagable>() is Damagable damagable1)
                result.Add(damagable1);

        }
        return result;
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
    public static Vector3 GetDirectionToClosestDamagable(Damagable anOrigin, float aDetectionRadius)
    {
        Damagable foundEntity = GetClosestDamagable(anOrigin, aDetectionRadius);

        if (!foundEntity) return Vector3.zero;
        Vector3 result = (foundEntity.transform.position - anOrigin.transform.position).normalized;
        result.y = 0;
        return result;
    }

    public static Quaternion AngleFromDir(Vector3 aDirection, Vector3 anUpDir)
    {
        return Quaternion.LookRotation(aDirection, anUpDir);
    }

    public static Vector3 DirFromAngle(float anAngleInDegrees, float localEuelerAnglesRot)
    {
        if (localEuelerAnglesRot != 0)
            anAngleInDegrees += localEuelerAnglesRot;

        return new Vector3(Mathf.Sin(anAngleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(anAngleInDegrees * Mathf.Deg2Rad));
    }

    public void OnUpdate(UpgradeManager aManager)
    {
        currentAttackRate += Time.deltaTime;
        if(currentAttackRate >= attackSpeed)
        {
            currentAttackRate = 0;
            OnUpdateWeapon(aManager);
            PollingStation.Get<WeaponHUD>().StartCooldown(this, attackSpeed);
        }
       
    }
    public abstract void OnUpdateWeapon(UpgradeManager aManager);

    public virtual void OnUpgrade(UpgradeManager aManager)
    {
        attackDamage = baseAttackDamage + (upgradeCount * additionalAttackDamage);
        attackSpeed = baseAttackSpeed - (upgradeCount * additionalAttackSpeed);
        attackRange = baseAttackRange + (upgradeCount * additionalAttackRange);
    }
    public virtual void OnDrawGizmo(UpgradeManager aManager) { }
}
