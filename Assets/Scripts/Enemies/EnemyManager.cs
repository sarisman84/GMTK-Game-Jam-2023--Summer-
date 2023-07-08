using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    public float spawnRad = 5;
    public int maxSpawnTries = 5;

    [SerializeField]
    private List<EnemySpawner> spawns = new List<EnemySpawner>();
    private List<EnemySpawnTracker> spawners;

    private Vector3 playerPos => PollingStation.Get<PlayerController>().transform.position;

    public void OnLoad() {}

    
    void Start()
    {
        spawners = new List<EnemySpawnTracker>(spawns.Count);
        foreach(EnemySpawner s in spawns) {
            spawners.Add(new EnemySpawnTracker(s));
        }
    }

    void Update()
    {
        if (!BackendManager.Get.runtimeActive) return;

        float currentTime = BackendManager.Get.gameTime;
        float lastTime = currentTime - Time.deltaTime;
        foreach(EnemySpawnTracker spawner in spawners) {
            spawner.Update(currentTime);

            int count = spawner.GetSpawnCount();
            for (int i = 0; i < count; i++) {
                Vector3 pos = getSpawnPos(spawner.spawner.sizeRad, spawner.spawner.spawnHeight);
                if (pos.x == float.PositiveInfinity) {
                    Debug.LogWarning("could not find a spawning position");
                    continue;
                }
                Enemy e = spawner.Spawn(pos, GetParent());
            }
        }
    }

    public Vector3 getSpawnPos(float radOffset = 0, float height = 0, int recursionCount = 0) {
        if (recursionCount > maxSpawnTries) return Vector3.positiveInfinity;

        float angle = Random.Range(0, 2*Mathf.PI);
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        pos *= spawnRad + radOffset;
        pos += playerPos;

        if(!Physics.Raycast(pos + Vector3.up * 10, Vector3.down, out RaycastHit hit))//try to figure out the floor height
            return getSpawnPos(radOffset, height, recursionCount+1);

        pos.y = hit.point.y + height;//set the y position to the y-pos, above floor height

        return pos;
    }

    private static Transform parent;
    public Transform GetParent() {
        if(parent == null)
            parent = new GameObject("Enemy Parent").transform;
        return parent;
    }

    public void OnDrawGizmosSelected() {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(playerPos, Vector3.up, spawnRad);
    }
}
