using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager {
    public float spawnRad = 5;
    public int maxSpawnTries = 5;

    [SerializeField]
    private SpawnSetTracker baseSpawnSet;

    public SpawnWave[] waveList;
    SortedList<float, SpawnWave> waves = new SortedList<float, SpawnWave>();//NOTE: this should work, because the order doesnt change, when the time changes (only once a wave is over, the time changes)
    WaveTracker activeWave;

    private Vector3 playerPos => PollingStation.Get<PlayerController>().transform.position;

    public void OnLoad() { }


    void Start()
    {
        baseSpawnSet.CreateTrackers();
        PollingStation.Get<GameplayManager>().onGameStartEvent += GameInit;
        PollingStation.Get<GameplayManager>().onGameOverEvent += Clear;
    }

    void GameInit() {
        float currentTime = BackendManager.Get.gameTime;
        foreach (var wave in waveList) waves.Add(wave.GetNextWaveTime(currentTime), wave);//reset the order of the waves
    }

    void Update()
    {
        if (!BackendManager.Get.runtimeActive) return;
        float currentTime = BackendManager.Get.gameTime;

        float nextWaveTime = waves.Keys[0];
        if (nextWaveTime-currentTime <= 0) {
            ActivateWave(currentTime, nextWaveTime);
        }

        if(activeWave != null) {
            UpdateSpawner(activeWave);
            if (activeWave.IsWaveOver()) {
                Debug.Log($"WAVE OVER: {activeWave.wave.name}");
                activeWave = null;
            }
        }
        else {
            UpdateSpawner(baseSpawnSet);
        }
    }

    void ActivateWave(float currentTime, float waveTime) {
        SpawnWave wave = waves.Values[0];
        waves.RemoveAt(0);
        waves.Add(wave.GetNextWaveTime(currentTime + 1E-5f), wave);//update the sorted list

        //Start the wave
        Debug.Log($"WAVE START: {wave.name}");
        activeWave = new WaveTracker(wave, waveTime);
    }

    void UpdateSpawner(SpawnSetTracker tracker) {
        tracker.Update(
            Time.deltaTime,
            tracker => {
                EnemySpawner spawner = (EnemySpawner)tracker.spawner;
                return getSpawnPos(spawner.sizeRad, spawner.spawnHeight);
            },
            GetParent()
        );
    }

    void Clear()
    {
        if (GetParent() != null)
            Destroy(GetParent().gameObject);

        baseSpawnSet.Reset();
        activeWave = null;
        waves.Clear();
    }

    public Vector3 getSpawnPos(float radOffset = 0, float height = 0, int recursionCount = 0)
    {
        if (recursionCount > maxSpawnTries) return Vector3.positiveInfinity;

        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        pos *= spawnRad + radOffset;
        pos += playerPos;

        if (!Physics.Raycast(pos + Vector3.up * 10, Vector3.down, out RaycastHit hit))//try to figure out the floor height
            return getSpawnPos(radOffset, height, recursionCount + 1);

        pos.y = hit.point.y + height;//set the y position to the y-pos, above floor height

        return pos;
    }

    private static Transform parent;
    public Transform GetParent()
    {
        if (parent == null)
            parent = new GameObject("Enemy Parent").transform;
        return parent;
    }


    public void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(playerPos, Vector3.up, spawnRad);
#endif
    }

}
