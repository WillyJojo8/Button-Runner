using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicPlayer;
    public AudioSource soundPlayer; // Para sonidos cortos tipo punto
    public AudioSource longSoundPlayer; // Para sonidos largos tipo power-up

    public AudioClip[] availableSoundClips;
    private Dictionary<string, AudioClip> loadedAudioClips;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        loadedAudioClips = new Dictionary<string, AudioClip>();
        foreach (AudioClip audio in availableSoundClips)
        {
            loadedAudioClips[audio.name] = audio;
        }

        musicPlayer?.Play();
    }

    public void PlaySound(string name)
    {
        if (loadedAudioClips.TryGetValue(name, out AudioClip clip))
        {
            soundPlayer.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"AudioClip '{name}' no encontrado.");
        }
    }

    public void PlayLongSound(string name)
    {
        if (loadedAudioClips.TryGetValue(name, out AudioClip clip))
        {
            longSoundPlayer.clip = clip;
            longSoundPlayer.loop = false;
            longSoundPlayer.Play();
        }
        else
        {
            Debug.LogWarning($"AudioClip largo '{name}' no encontrado.");
        }
    }

    public void StopMusic()
    {
        musicPlayer?.Stop();
    }
}