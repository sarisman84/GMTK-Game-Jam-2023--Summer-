using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Weapon", menuName = "Upgrades/Weapons/Basic", order = 0)]
public class BaseWeapon : BaseWeaponObject {

    public float detectionAngle = 20.0f;

    private Vector3 closestEnemyDir;

    public override void OnUpdate(PlayerController aController)
    {
        currentAttackRate += Time.deltaTime;
        if (currentAttackRate >= attackSpeed)
        {
            currentAttackRate = 0;

            DetectionDesc desc = new DetectionDesc();

            closestEnemyDir = GetDirectionToClosestDamagable(aController, attackRange);

            desc.viewDirection = closestEnemyDir;
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

    public override void OnDrawGizmo(PlayerController aController)
    {
        Handles.color = Color.red;
        Handles.DrawWireArc(aController.transform.position, Vector3.up, Vector3.forward, 360.0f, attackRange);

        var quatern = AngleFromDir(closestEnemyDir, Vector3.up);

        Vector3 viewAngleA = DirFromAngle(quatern.eulerAngles.y, -(detectionAngle / 2.0f));
        Vector3 viewAngleB = DirFromAngle(quatern.eulerAngles.y, (detectionAngle / 2.0f));


        Handles.DrawLine(aController.transform.position, aController.transform.position + (viewAngleA).normalized * attackRange);
        Handles.DrawLine(aController.transform.position, aController.transform.position + (viewAngleB).normalized * attackRange);

    }
}
