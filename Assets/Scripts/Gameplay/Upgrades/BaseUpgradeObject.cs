using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgradeObject : ScriptableObject
{
    public string description;
    public Sprite icon;
    [HideInInspector] public int upgradeCount;
    public abstract void OnUpdate(PlayerController aController);
}
