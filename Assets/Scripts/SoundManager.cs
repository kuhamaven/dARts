using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource effectsSource;
    public AudioSource musicSource;

    public static SoundManager Instance = null;

    public AudioClip dartHit;
    public AudioClip dartReady;
    public AudioClip dartsMusic;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = dartsMusic;
        musicSource.Play();
    }

    public void PlayDartHit()
    {
        effectsSource.clip = dartHit;
        effectsSource.Play();
    }

    public void PlayDartReady()
    {
        effectsSource.clip = dartReady;
        effectsSource.Play();
    }
}
