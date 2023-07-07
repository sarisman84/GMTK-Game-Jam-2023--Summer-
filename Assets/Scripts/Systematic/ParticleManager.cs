using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleManager : MonoBehaviour, IManager {

    [System.Serializable]
    public struct VFXPrefab
    {
        public GameObject prefab;
        public string name;
    }


    public List<VFXPrefab> vfxPrefabToInstantiate;
    public void OnLoad()
    {
       
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
