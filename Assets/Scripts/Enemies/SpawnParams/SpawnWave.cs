using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnWave", menuName = "Enemies/SpawnWave")]
public class SpawnWave : ScriptableObject
{
    float firstTime = 0.0f;
    float timeBetweenWaves = 10.0f;//not including the waveduration

    float waveDuration = 10.0f;
}
