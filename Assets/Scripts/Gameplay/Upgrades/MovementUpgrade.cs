using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Movement Upgrade", menuName ="Upgrades/Movement",order = 0)]
public class MovementUpgrade : BaseUpgradeObject {

    public float additionalMovementSpeed;
    public override void ExecuteUpgrade(PlayerController aController)
    {
        aController.movementSpeed += additionalMovementSpeed;
    }
}
