using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] music, sfx;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic("Theme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string soundName)
    {
        Sound targetSound = Array.Find(music, s => s.soundName == soundName);

        if (targetSound == null)
        {
            Debug.Log("SOUND DOES NOT EXIST.");
        }
        else
        {
            musicSource.clip = targetSound.soundClip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string soundName)
    {
        Sound targetSound = Array.Find(sfx, sound => sound.soundName == soundName);

        if (targetSound == null)
        {
            Debug.Log("SFX DOES NOT EXIST.");
        }
        else
        {
            sfxSource.clip = targetSound.soundClip;
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }
}
