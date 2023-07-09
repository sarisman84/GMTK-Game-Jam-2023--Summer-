using UnityEngine;

[System.Serializable]
public class ExtrapolateCurve
{
    public enum ContinuationBehaviour { CurveInternal, CurveLinear, ScaledInternal, ScaledLinear }

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private ContinuationBehaviour continuationBehaviour;
    public float scalePerSecond = 1;

    public float GetValue(float time) {
        ref Keyframe key = ref curve.keys[curve.length - 1];
        if (time > key.time)
            switch (continuationBehaviour) {
                case ContinuationBehaviour.CurveInternal: return curve.Evaluate(time);                                     //use the internal extrapolation
                case ContinuationBehaviour.CurveLinear: return key.value + key.outTangent * (time - key.time);//linearly extrapolate the probability with the last tangent slope
                case ContinuationBehaviour.ScaledInternal: return curve.Evaluate(time) * scalePerSecond * (time - key.time);//use the internal extrapolation, but scale it linearly with time
                case ContinuationBehaviour.ScaledLinear: return key.value + scalePerSecond * (time - key.time);//linearly extrapolate the probability with scale factor
            }

        return curve.Evaluate(time);
    }
}
