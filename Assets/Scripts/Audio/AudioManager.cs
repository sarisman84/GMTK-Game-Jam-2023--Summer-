using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    GameObject audiosourceobject;

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

    public void PlayRandomSound(string sound1, string sound2, string sound3, string sound4)
    {
        Sound s1 = Array.Find(sounds, item => item.name == sound1);
        Sound s2 = Array.Find(sounds, item => item.name == sound2);
        Sound s3 = Array.Find(sounds, item => item.name == sound3);
        Sound s4 = Array.Find(sounds, item => item.name == sound4);

        int randomIndex = UnityEngine.Random.Range(0, 4);

        switch (randomIndex)
        {
            case 1:
                s1.source.Play();
                s1.source.pitch = UnityEngine.Random.Range(-0.75f, 1.25f);
                break;

            case 2:
                s2.source.Play();
                s2.source.pitch = UnityEngine.Random.Range(-0.75f, 1.25f);
                break;
            case 3:
                s3.source.Play();
                s3.source.pitch = UnityEngine.Random.Range(-0.5f, 1.5f);
                break;

            case 4:
                s4.source.Play();
                s4.source.pitch = UnityEngine.Random.Range(-0.5f, 1.5f);
                break;
        }
    }



}

    
    

    