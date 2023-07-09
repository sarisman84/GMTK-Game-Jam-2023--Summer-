using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnSet", menuName = "Enemies/SpawnSet")]
public class SpawnSet : ScriptableObject
{
    public RateSpawner[] spawns;
}

[System.Serializable]
public class SpawnSetTracker {
    public SpawnSet spawnSet;
    public RateSpawnTracker[] spawners { get; set; }
    public float currentTime { get; private set; } = 0;

    public void CreateTrackers() {
        spawners = new RateSpawnTracker[spawnSet.spawns.Length];
        for (int i = 0; i < spawnSet.spawns.Length; i++) {
            spawners[i] = new RateSpawnTracker(spawnSet.spawns[i]);
        }
    }

    public void Reset() {
        foreach(RateSpawnTracker tracker in spawners)
            tracker?.Reset();

        currentTime = 0;
    }

    public virtual void Update(float dt, Func<RateSpawnTracker, Vector3> getPos, Transform parent = null, float countShift = 0, float countScale = 1) {
        currentTime += dt;

        foreach (RateSpawnTracker spawner in spawners) {
            spawner.Update(currentTime);

            int count = spawner.GetSpawnCount(countShift, countScale);
            for (int i = 0; i < count; i++) {
                Vector3 pos = getPos(spawner);
                if (pos.x == float.PositiveInfinity) {
                    Debug.LogWarning("could not find a spawning position");
                    continue;
                }

                spawner.Spawn(pos, parent);
            }
        }
    }
}