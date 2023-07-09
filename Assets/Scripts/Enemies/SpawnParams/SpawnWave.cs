using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnWave", menuName = "Enemies/SpawnWave")]
public class SpawnWave : ScriptableObject
{
    [Header("Times (in seconds)")]
    public float firstTime = 0.0f;
    public float timeBetweenWaves = 10.0f;//not including the waveduration

    public float waveDuration = 10.0f;

    [Header("spawnSet (time from 0 to 1 -> 1 is end of wave)")]
    public SpawnSet spawnSet;

    [Space]
    [Header("scaling")]
    public float startTimeScaleShift = 0.0f;

    public float GetNextWaveTime(float currentTime) {
        currentTime -= firstTime;
        int waveCount = Mathf.FloorToInt(currentTime / (timeBetweenWaves + waveDuration));
        return firstTime + (timeBetweenWaves + waveDuration) * (waveCount + 1);
    }
}

public class WaveTracker : SpawnSetTracker {
    public SpawnWave wave { get; private set; }
    public float waveStartTime { get; private set; }

    public WaveTracker(SpawnWave wave, float waveStartTime) : base() {
        this.wave = wave;
        spawnSet = wave.spawnSet;
        this.waveStartTime = waveStartTime;

        CreateTrackers();
    }

    public override void Update(float dt, Func<RateSpawnTracker, Vector3> getPos, Transform parent = null, float countShift = 0, float countScale = 1) {
        float waveDT = dt / wave.waveDuration;//NOTE: scaling down the time by waveDuration requires us the scale up the values by waveDuration to keep a unit of enemies per second
        base.Update(waveDT, getPos, parent, waveStartTime * wave.startTimeScaleShift + countShift, wave.waveDuration * countScale);
    }

    public bool IsWaveOver() {
        return currentTime > 1.0f;//NOTE: current time is in wave-progress -> goes from 0 to 1
    }
}