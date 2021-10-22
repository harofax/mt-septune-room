using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class JukeBox : MonoBehaviour
{
    [FormerlySerializedAs("bg_mainmenu")] public AudioSource jukeboxAudioSource;

    [Range(0, 1)] public float targetVolume;

    [Range(1, 20)] public float fadeDuration;

    private int songIndex = 0;

    public AudioClip[] songs;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(StartFade(jukeboxAudioSource, fadeDuration, targetVolume));
        jukeboxAudioSource.Play();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        jukeboxAudioSource.Stop();
    }

    public void NextSong()
    {
        songIndex++;
        if (songIndex > songs.Length - 1)
        {
            songIndex = 0;
        }
        jukeboxAudioSource.clip = songs[songIndex];
        PlaySong();
    }

    public void StopSong()
    {
        jukeboxAudioSource.Stop();
    }

    public void PlaySong()
    {
        jukeboxAudioSource.Play();
    }

    public void PauseSong()
    {
        jukeboxAudioSource.Pause();
    }

    public void SetVolume(float vol)
    {
        if (vol < 0.0 || vol > 1.0) return;
        jukeboxAudioSource.volume = vol;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}


