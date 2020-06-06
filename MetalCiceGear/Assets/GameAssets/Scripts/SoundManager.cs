using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> gameSounds = new List<AudioClip>();
    AudioSource aSource; 

    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlayMainTheme()
    {
        StopMusic();
        aSource.loop = true;
        aSource.volume = 0.3f;
        aSource.clip = gameSounds[0];
        aSource.Play();
    }

    public void PlayDieOneShot()
    {
        StopMusic();
        aSource.volume = 0.5f;
        aSource.PlayOneShot(gameSounds[1]);
    }

    public void PlayWinOneShot()
    {
        StopMusic();
        aSource.volume = 0.5f;
        aSource.PlayOneShot(gameSounds[2]);
    }

    public void PlayGameOverOneShot()
    {
        StopMusic();
        aSource.volume = 0.5f;
        aSource.PlayOneShot(gameSounds[3]);
    }

    public void PlayCelebrationOneShot()
    {
        StopMusic();
        aSource.volume = 0.5f;
        aSource.PlayOneShot(gameSounds[4]);
    }

    public void StopMusic()
    {
        if (aSource.isPlaying) aSource.Stop();
    }
}
