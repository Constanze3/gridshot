using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public AudioSource musicAudioSource;
    public AudioSource ambianceAudioSource;
    public Volume volume;

    [Header("Background Audio")]
    public AudioClip mainMenuAudio;
    public AudioClip gameAudio;

    [Header("Gameplay")]
    public GameObject playerPrefab;
    public Transform spawn;

    [Header("UI")]
    public GameObject mainMenu;
    public GameObject playerUI;

    private GameState gameState;
    public event Action OnStartGame;

    private GameSettings game = null;

    private Blur blur;

    private enum GameState {
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

        gameState = GameState.MainMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public enum GameMode { 
        Zen,
        Timer,
    }

    public class GameSettings
    {
        private GameMode gameMode;
        public GameMode GameMode {
            get { 
                return gameMode; 
            }
            set {
                switch (value)
                {
                    case GameMode.Zen:
                        gameMode = GameMode.Zen;
                        break;
                    case GameMode.Timer:
                        gameMode = GameMode.Timer;
                        TimeLimit = 60;
                        break;
                }
            }
        }
        public float TimeLimit { get; set; }
    }

    /// <summary>
    /// Sets up the game and spawns the player.
    /// </summary>
    /// <param name="gameSettings"> settings for the game to be started </param>
    public void InitializeGame(GameSettings gameSettings) {
        if (gameState != GameState.MainMenu) return;

        blur.active = false;

        game = gameSettings;

        musicAudioSource.Stop();

        mainMenu.SetActive(false); 
        playerUI.SetActive(true);

        // TODO Maybe display gamemode on player ui as well
        if (game.GameMode == GameMode.Timer) { 
            // TODO Set up timer
        }

        Instantiate(playerPrefab, spawn);

        OnStartGame?.Invoke();
        gameState = GameState.Playing;
    }

    /// <summary>
    /// Starts the game that is already initialized.
    /// </summary>
    public void StartGame() {
        if (gameState != GameState.Ready) return;

        if (game.GameMode == GameMode.Timer) { 
            // TODO Start timer
        }
    }

}
