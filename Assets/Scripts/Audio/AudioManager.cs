using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.Stop();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        StartCoroutine(fadeout());

        IEnumerator fadeout()
        {
            while (Mathf.Abs(s.source.volume - 0) > 0.01f)
            {
                // Gradually alter the float value towards the target value
                s.source.volume = Mathf.MoveTowards(s.source.volume, 0, 0.25f * Time.deltaTime);


                yield return null;
            }

        }
    }
}

    
    

    