using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgradeObject : ScriptableObject
{
    public string description;
    public abstract void ExecuteUpgrade(PlayerController aController);
}
