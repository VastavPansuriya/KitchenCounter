using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    private const string PLAYERPREF_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }


    private float volume = .3f;

    private AudioSource audioSource;
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYERPREF_MUSIC_VOLUME, .3f);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }


    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYERPREF_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
