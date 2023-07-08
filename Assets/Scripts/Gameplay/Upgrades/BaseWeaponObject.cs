﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DetectionDesc 
{
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
    public Sprite icon;


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

            if(Mathf.Acos(dotProduct) < viewAngleInRadians)
            {
                result.Add(damagable);
            }
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
        return (foundEntity.transform.position - anOrigin.transform.position).normalized;
    }
    public abstract void OnUpdate(PlayerController aController);
}
