using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip playerRunning;
    public AudioClip backgroundMusic;
    public AudioClip playerJumping;
    public AudioClip enemyPunch;
    

    private void Start()
    {
        musicSource.clip = backgroundMusic;   
        musicSource.Play();
        musicSource.loop = true;
    }

    public void PlaySFX(AudioClip clip, bool loop = false)
    {
       if (SFXSource.clip == clip && SFXSource.isPlaying) return; // prevent restart every frame
            SFXSource.clip = clip;
            SFXSource.loop = loop;
            SFXSource.Play();
    }

    public void PlayJumpSFX()
    {
        SFXSource.PlayOneShot(playerJumping);
    }

    public void StopSFX()
    {
    SFXSource.Stop();
    }

    public void PlayPunchSFX()
    {
        musicSource.PlayOneShot(enemyPunch) ;
    }

    public void FadeMusicOut(float volume)
    {
        musicSource.volume = volume; // ✅ called every frame to reduce volume
    }

public void StopMusic()
    {
        musicSource.Stop(); // ✅ fully stop the music
    }

}
