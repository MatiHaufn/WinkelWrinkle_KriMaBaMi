using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    
    
    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            DontDestroyOnLoad(gameObject);
            return;
        }


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.sampleClip;
            s.source.pitch = s.pitch;
            if (s.pitch == 0)
            { s.pitch = 1; }

            s.source.volume = s.volume;
            if (s.volume == 0)
            { s.volume = 1; }

            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        //Faster iteration that foreach
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not recognized!");
            return;
        }
        
        s.source.Play();
    } 

}
