using System;
using UnityEngine;


public class RateSpawner : ScriptableObject
{
    public enum ContinuationBehaviour { CurveInternal, CurveLinear, ScaledInternal, ScaledLinear }
    public enum DiviationType { None, Constant, Normal }


    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private AnimationCurve spawnRate;//its probability density, because we integrate over it to get the actual probability

    [Space]
    [SerializeField]
    private ContinuationBehaviour continuationBehaviour;
    public float scalePerSecond = 1;

    [Space]
    public DiviationType diviationType;
    public float diviation;


    #region SpawnCount Calculation
    private float GetDensityVal(float time) {
        ref Keyframe key = ref spawnRate.keys[spawnRate.length - 1];
        if (time > key.time)
            switch (continuationBehaviour) {
                case ContinuationBehaviour.CurveInternal:  return spawnRate.Evaluate(time);                                     //use the internal extrapolation
                case ContinuationBehaviour.CurveLinear:    return key.value +                key.outTangent * (time - key.time);//linearly extrapolate the probability with the last tangent slope
                case ContinuationBehaviour.ScaledInternal: return spawnRate.Evaluate(time) * scalePerSecond * (time - key.time);//use the internal extrapolation, but scale it linearly with time
                case ContinuationBehaviour.ScaledLinear:   return key.value +                scalePerSecond * (time - key.time);//linearly extrapolate the probability with scale factor
            }
            
        return spawnRate.Evaluate(time);
    }


    public int GetSpawnCount(float timeA, float timeB) {
        float spawn = GetSpawnFloat(timeA, timeB);
        return Mathf.RoundToInt(spawn);
    }
    public float GetSpawnFloat(float timeA, float timeB) {
        float exact = GetSpawnCountExact(timeA, timeB);
        switch (diviationType) {
            case DiviationType.Constant: return UnityEngine.Random.Range(exact - 0.5f*diviation, exact + 0.5f*diviation);
            case DiviationType.Normal: return SampleNormalDistr(exact, diviation);
            default: return exact;
        }
    }
    private float GetSpawnCountExact(float timeA, float timeB) => Integrate(timeA, timeB, GetDensityVal);

    public static float Integrate(float timeA, float timeB, Func<float, float> timeToVal, float baseDt = 1 / 200.0f/*corresponds to 200 samples every second*/) {
    float timeDiff = timeB - timeA;
        int integrationSteps = Mathf.Clamp(Mathf.FloorToInt(timeDiff / baseDt), 1, 10);//we will use at most 10 steps, and minimal 1
        float dt = timeDiff / integrationSteps;//by flooring to an exact amout, we stretch the timesteps to fit exactly the timeframe
        float sum = 0;
        for (float t = timeA; t < timeB - 1E-5f; t += dt) {
            sum += timeToVal(t) * dt;
        }

        return sum;
    }

    public static float SampleNormalDistr(float mean, float scale) {//https://en.wikipedia.org/wiki/Normal_distribution
        //using the cumulative distribution function to take a random sample (but invert it)
        throw new NotImplementedException();

        //float samplePoint = UnityEngine.Random.value;
        //return InvErrorFunction(2*samplePoint - 1) * scale * 1.41421356237f + mean;//scale / sqrt(2)
        //see https://en.wikipedia.org/wiki/Inverse_transform_sampling
    }
    #endregion

    public GameObject Spawn(Vector3 pos, Transform parent = null) {
        return Instantiate(prefab, pos, Quaternion.identity, parent);
    }
}

public class RateSpawnTracker {
    public int spawnedCount { get; private set; } = 0;
    private float currentIntegralVal = 0;
    public float lastTime { get; private set; }

    public RateSpawner spawner;

    public RateSpawnTracker(RateSpawner spawner) {
        this.spawner = spawner;
    }

    public void Update(float currentTime) {
        currentIntegralVal += spawner.GetSpawnFloat(lastTime, currentTime);
        lastTime = currentTime;
    }

    public int GetSpawnCount() {//how many objects still need to be spawned to get to the currentIntegralValue
        return Mathf.RoundToInt(currentIntegralVal) - spawnedCount;
    }

    public GameObject Spawn(Vector3 pos, Transform parent = null) {
        spawnedCount++;
        return spawner.Spawn(pos, parent);
    }

    public void Reset() {
        spawnedCount = 0;
        currentIntegralVal = 0;
        lastTime = 0;
    }
}