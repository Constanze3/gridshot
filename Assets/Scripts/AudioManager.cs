using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource ambianceSource;

    [Serializable]
    public struct Clip
    {
        public string name;
        public AudioClip clip;
    }

    public Clip[] m_MusicClips;
    public Clip[] m_AmbianceClips;

    private Dictionary<string, AudioClip> musicClips;
    private Dictionary<string, AudioClip> ambianceClips;

    private float defaultMusicVolume;
    private float defaultAmbianceVolume;

    private void LoadEditorData()
    {
        musicClips = new Dictionary<string, AudioClip>();
        foreach (Clip c in m_MusicClips)
        {
            if (!musicClips.TryAdd(c.name, c.clip))
                throw new Exception("Music clip names should be unique");
        }

        ambianceClips = new Dictionary<string, AudioClip>();
        foreach (Clip c in m_AmbianceClips)
        {
            if (!ambianceClips.TryAdd(c.name, c.clip))
                throw new Exception("Ambiance clip names should be unique");
        }
    }

    private void Awake()
    {
        LoadEditorData();

        musicSource.loop = true;
        ambianceSource.loop = true;

        defaultMusicVolume = musicSource.volume;
        defaultAmbianceVolume = ambianceSource.volume;
    }

    public void UpdateAudio(GameManager.State oldState, GameManager.State newState)
    {
        switch (newState)
        {
            case GameManager.State.MainMenu:
                StopFade();
                musicSource.clip = musicClips["MainMenu"];
                ambianceSource.clip = ambianceClips["Sea"];
                musicSource.Play();
                ambianceSource.Play();
                break;
            case GameManager.State.Playing:
                if (oldState == GameManager.State.MainMenu)
                {
                    StartCoroutine(FadeOut(musicSource, 1));
                }
                ambianceSource.clip = ambianceClips["Sea"];
                break;
            default:
                break;
        }
    }

    private void StopFade()
    {
        StopAllCoroutines();
        musicSource.volume = defaultMusicVolume;
        ambianceSource.volume = defaultAmbianceVolume;
    }

    private IEnumerator FadeIn(AudioSource source, float time, float volume)
    {
        source.Play();
        float t = 0;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0, volume, t / time);
            yield return null;
        }
    }

    private IEnumerator FadeOut(AudioSource source, float time)
    {
        float initialVolume = source.volume;
        float t = 0;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(initialVolume, 0, t / time);
            yield return null;
        }

        source.Stop();
        source.volume = initialVolume;
    }
}