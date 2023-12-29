using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Gameplay")]
    public Game game;
    public GameObject playerPrefab;
    public Transform spawn;

    [Header("Audio Manager")]
    public AudioSource musicAudioSource;
    public AudioSource ambianceAudioSource;
    public Volume volume;

    [Header("Background Audio")]
    public AudioClip mainMenuAudio;
    public AudioClip gameAudio;

    [Header("UI")]
    public GameObject mainMenu;
    public PlayerUI playerUI;

    private State gameState;
    private Blur blur;

    private enum State
    {
        MainMenu,
        Ready,
        Playing,
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        volume.profile.TryGet<Blur>(out blur);
        blur.active = true;

        musicAudioSource.clip = mainMenuAudio;
        musicAudioSource.loop = true;
        musicAudioSource.Play();

        ambianceAudioSource.clip = gameAudio;
        ambianceAudioSource.loop = true;
        ambianceAudioSource.Play();

        gameState = State.MainMenu;
    }

    public void TryStartGame(Game.Settings settings)
    {
        if (gameState != State.MainMenu) return;

        musicAudioSource.Stop();

        blur.active = false;
        mainMenu.SetActive(false);
        playerUI.gameObject.SetActive(true);

        Instantiate(playerPrefab, spawn);
        game.StartNew(settings);

        gameState = State.Playing;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
