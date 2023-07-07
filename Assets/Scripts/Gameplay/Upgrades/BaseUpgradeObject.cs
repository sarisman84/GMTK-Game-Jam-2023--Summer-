using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgradeObject : ScriptableObject
{
    public string description;
    [HideInInspector] public int upgradeCount;
    public abstract void OnUpdate(PlayerController aController);
}
