using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public State gameState { get; private set; }

    [Header("Audio")]
    public AudioManager audioManager;

    [Header("Gameplay")]
    public Game game;
    public Player playerPrefab;
    public Transform spawn;

    [Header("UI")]
    public GameObject mainMenu;
    public PlayerUI playerUI;
    public PauseMenu pauseMenu;

    [Header("Post Processing")]
    public Volume volume;

    private Player player;
    private Blur blur;

    public enum State
    {
        MainMenu,
        Ready,
        Playing,
        Paused,
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

    private void SetState(State state)
    {
        if (state == State.Paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        audioManager.UpdateAudio(this.gameState, state);
        this.gameState = state;
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        playerUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);

        volume.profile.TryGet<Blur>(out blur);
        blur.active = true;

        SetState(State.MainMenu);
    }

    private void UpdatePlaying()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetPlayerMovementLock(!player.MovementEnabled);
        }
    }

    private void SetPlayerMovementLock(bool value)
    {
        player.MovementEnabled = value;
        playerUI.SetMovementLock(!player.MovementEnabled);
    }

    private void UpdatePaused()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePauseMenu();
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case State.Playing:
                UpdatePlaying();
                break;
            case State.Paused:
                UpdatePaused();
                break;
        }
    }

    public void TryStartGame(Game.Settings settings)
    {
        if (gameState != State.MainMenu) return;

        Cursor.lockState = CursorLockMode.Locked;
        blur.active = false;

        mainMenu.SetActive(false);
        playerUI.gameObject.SetActive(true);

        player = Instantiate(playerPrefab, spawn);
        SetPlayerMovementLock(false);

        game.StartNew(settings);
        SetState(State.Playing);
    }

    public void OpenPauseMenu()
    {
        player.Enabled = false;
        Cursor.lockState = CursorLockMode.None;
        blur.active = true;

        playerUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);

        SetState(State.Paused);

    }

    public void ClosePauseMenu()
    {
        player.Enabled = true;
        blur.active = false;

        pauseMenu.gameObject.SetActive(false);
        playerUI.gameObject.SetActive(true);

        SetState(State.Playing);
    }

    public void LoadMainMenu()
    {
        game.Stop();
        Destroy(player.gameObject);

        playerUI.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);

        blur.active = true;
        mainMenu.SetActive(true);

        SetState(State.MainMenu);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}