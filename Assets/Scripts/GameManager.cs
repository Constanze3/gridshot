using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Audio")]
    public AudioManager audioManager;

    [Header("Gameplay")]
    public Game game;
    public GameObject playerPrefab;
    public Transform spawn;

    [Header("UI")]
    public GameObject mainMenu;
    public PlayerUI playerUI;

    [Header("Post Processing")]
    public Volume volume;

    private State gameState;
    private Blur blur;

    public enum State
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

    private void SetState(State state) {
        this.gameState = state;
        audioManager.UpdateAudio(state);
    }

    private void Start()
    {
        volume.profile.TryGet<Blur>(out blur);
        blur.active = true;

        SetState(State.MainMenu);
    }

    public void TryStartGame(Game.Settings settings)
    {
        if (gameState != State.MainMenu) return;

        blur.active = false;
        mainMenu.SetActive(false);
        playerUI.gameObject.SetActive(true);

        Instantiate(playerPrefab, spawn);
        game.StartNew(settings);

        SetState(State.Playing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
