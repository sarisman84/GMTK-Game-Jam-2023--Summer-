using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnWave", menuName = "Enemies/SpawnWave")]
public class SpawnWave : ScriptableObject
{
    public float firstTime = 0.0f;
    public float timeBetweenWaves = 10.0f;//not including the waveduration

    public float waveDuration = 10.0f;

    public RateSpawner[] spawners;

    float GetNextWaveTime(float currentTime) {
        currentTime -= firstTime;
        int waveCount = Mathf.FloorToInt(currentTime / (timeBetweenWaves + waveDuration));
        return firstTime + (timeBetweenWaves + waveDuration) * (waveCount + 1);
    }
}
