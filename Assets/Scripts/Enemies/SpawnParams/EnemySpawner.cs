using UnityEngine;

[CreateAssetMenu(fileName = "new EnemySpawner", menuName = "Enemies/EnemySpawner")]
public class EnemySpawner : RateSpawner
{
    [Space]

    public float spawnHeight;
    public float sizeRad;
}

public class EnemySpawnTracker : RateSpawnTracker {
    public new EnemySpawner spawner { 
        get => (EnemySpawner)base.spawner; 
        set => base.spawner = value; 
    }

    public EnemySpawnTracker(EnemySpawner enemySpawner) : base(enemySpawner) { } 

    public new Enemy Spawn(Vector3 pos, Transform parent = null) {
        return base.Spawn(pos, parent).GetComponent<Enemy>();
    }
}