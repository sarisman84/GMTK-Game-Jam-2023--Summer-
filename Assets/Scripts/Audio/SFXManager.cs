using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour, IManager
{
    public List<AudioClip> audioTracks; //This is where the tracks come from

    public void OnLoad()
    {

    }

    void Start()
    {
        foreach (var item in audioTracks)
        {
            gameObject.AddComponent<AudioSource>().clip = item;
        }
    }


    void Update()
    {
        
    }

    public void playupgradesfx()
    {
        
    }
}
