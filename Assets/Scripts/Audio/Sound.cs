using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 3f)]
    public float pitch;

    [Range(.1f, 3f)]
    public float volume;

    [Range(0, 1f)]
    public float spatialblend;

    [Range(1, 500f)]
    public float mindistance;

    [Range(0, 500f)]
    public float maxdistance;

    public bool loop;

    public AudioSource source;
}
