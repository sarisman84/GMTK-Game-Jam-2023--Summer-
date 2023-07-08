using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Divine Thrust", menuName = "Upgrades/Weapons/Divine Thrust", order = 0)]
public class DivineThrust : BaseWeaponObject {

    public float barrelDistance = 0.5f;

    public float baseProjectileSpeed;
    public float additionalProjectileSpeed;

    public GameObject projectilePrefab;

    public float projectileSpeed
    {
        private set;
        get;
    }

    private Vector3 closestEnemyDir;


    public override void OnUpdateWeapon(UpgradeManager aManager)
    {


        closestEnemyDir = GetDirectionToClosestDamagable(aManager.player, attackRange);

        if (closestEnemyDir == Vector3.zero) return;

        ProjectileDesc<PlayerController> desc = new ProjectileDesc<PlayerController>();
        desc.projectilePrefab = projectilePrefab;
        //Offset its spawning location so its not in the player
        desc.spawnPoint = aManager.player.transform.position + closestEnemyDir.normalized * barrelDistance;
        desc.spawnRotation = Quaternion.LookRotation(closestEnemyDir.normalized, Vector3.up);

        //Set its velocity to be the direction of the closest enemy
        desc.velocity = closestEnemyDir.normalized * projectileSpeed;
        desc.damage = attackDamage;
        desc.shooter = aManager.player;


        Projectile.Create(desc);
    }

    public override void OnUpgrade(UpgradeManager aManager)
    {
        base.OnUpgrade(aManager);
        projectileSpeed = baseProjectileSpeed + (upgradeCount * additionalProjectileSpeed);

    }

    public override void OnDrawGizmo(UpgradeManager aManager)
    {
        Vector3 centerPos = aManager.player.transform.position + (closestEnemyDir.normalized * attackRange / 2.0f);

        Handles.color = Color.yellow;
        Handles.matrix = Matrix4x4.TRS(centerPos, AngleFromDir(closestEnemyDir, Vector3.up), new Vector3(0.5f, 0.0f, attackRange));
        Handles.DrawWireCube(Vector3.zero, Vector3.one);
        Handles.matrix = Matrix4x4.identity;
    }
}
