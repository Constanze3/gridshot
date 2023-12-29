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

    private void LoadEditorData()
    {
        musicClips = new Dictionary<string, AudioClip>();
        foreach (Clip c in m_MusicClips)
        {
            if(!musicClips.TryAdd(c.name, c.clip))
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
    }

    private void Start()
    {
        musicSource.loop = true;
        ambianceSource.loop = true;
    }

    public void UpdateAudio(GameManager.State state)
    {
        switch (state)
        {
            case GameManager.State.MainMenu:
                musicSource.clip = musicClips["MainMenu"];
                ambianceSource.clip = ambianceClips["Sea"];
                musicSource.Play();
                ambianceSource.Play();
                break;
            case GameManager.State.Playing:
                StartCoroutine(FadeOut(musicSource, 1));
                ambianceSource.clip = ambianceClips["Sea"];
                break;
            case GameManager.State.Ready:
            default:
                break;
        }
    }

    private IEnumerator FadeIn(AudioSource source, float time, float volume)
    {
        source.Play();
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
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
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(initialVolume, 0, t / time);
            yield return null;
        }

        source.Stop();
        source.volume = initialVolume;
    }
}
