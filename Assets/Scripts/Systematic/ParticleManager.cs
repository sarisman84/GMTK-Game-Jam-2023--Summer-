using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDesc {

    public enum VariableType {
        Color, Float, Bool
    }

    public struct Variable {
        public string variableName;
        public object value;
        public VariableType type;

    }
    public VisualEffectAsset effectAsset;
    public Quaternion rotation;
    public Vector3 position;
    public Vector3 scale;
    public List<Variable> variables = new List<Variable>();
}

public class ParticleManager : MonoBehaviour, IManager {

    class VFXInstance {
        public VisualEffect effect;
        public bool isUsed;
    }

    public int allocatedVFXInstances = 100;

    private List<VFXInstance> currentInstances;

    public void OnLoad()
    {
        currentInstances = new List<VFXInstance>();
        for (int i = 0; i < allocatedVFXInstances; i++)
        {
            GameObject effectHolder = new GameObject($"Effect Instance {i}");
            effectHolder.transform.parent = transform;


            var instance = new VFXInstance();
            instance.effect = effectHolder.AddComponent<VisualEffect>();
            instance.effect.visualEffectAsset = null;
            instance.effect.Stop();
            instance.isUsed = false;
            currentInstances.Add(instance);
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentInstances.Count; i++)
        {
            var instance = currentInstances[i];

            if (instance.effect.culled || instance.effect.aliveParticleCount <= 0 || instance.effect.pause)
                instance.isUsed = false;
        }
    }

    public void PlayEffect(VFXDesc aDescription)
    {
        var availablePlayer = currentInstances.Find((x) => { return !x.isUsed; });

        if (availablePlayer == null)
        {
            allocatedVFXInstances += allocatedVFXInstances;
            OnLoad();

            PlayEffect(aDescription);
        }



        for (int i = 0; i < aDescription.variables.Count; i++)
        {
            var variable = aDescription.variables[i];


            switch (variable.type)
            {
                case VFXDesc.VariableType.Color:
                    availablePlayer.effect.SetVector4(variable.variableName, (Vector4)variable.value);
                    break;
                case VFXDesc.VariableType.Float:
                    availablePlayer.effect.SetFloat(variable.variableName, (float)variable.value);
                    break;
                case VFXDesc.VariableType.Bool:
                    availablePlayer.effect.SetBool(variable.variableName, (bool)variable.value);
                    break;
                default:
                    break;
            }
        }

        availablePlayer.effect.transform.position = aDescription.position;
        availablePlayer.effect.transform.rotation = aDescription.rotation;
        availablePlayer.effect.transform.localScale = aDescription.scale;

        availablePlayer.effect.visualEffectAsset = aDescription.effectAsset;
        availablePlayer.effect.Play();

        availablePlayer.isUsed = true;
    }
}
