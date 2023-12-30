using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Game : MonoBehaviour
{
    public TheGrid grid;
    public PlayerUI playerUI;

    public bool IsPlaying { get; private set; }

    public event Action OnStart;
    public event Action OnEnd;

    public enum GameMode
    {
        Zen,
        Timer,
    }

    public class Settings
    {
        private GameMode gameMode;
        public GameMode GameMode
        {
            get
            {
                return gameMode;
            }
            set
            {
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
        public int TimeLimit { get; set; }
    }

    private Settings settings;
    private float time = 0;
    private int score = 0;

    public void StartNew(Settings settings)
    {
        this.settings = settings;
        time = 0;
        score = 0;

        IsPlaying = true;

        // playerUI.SetGameMode(settings.GameMode);

        OnStart?.Invoke();
    }

    private void Update()
    {
        if (!IsPlaying) return;

        time += Time.deltaTime;

        HandleTime();
        HandleScore();
    }

    private void HandleTime()
    {
        switch (settings.GameMode)
        {
            case GameMode.Timer:
                int timeLeft = settings.TimeLimit - (int)Math.Floor(time);
                playerUI.SetTime(timeLeft);

                if (timeLeft <= 0)
                {
                    EndGame();
                }

                break;
            case GameMode.Zen:
            default:
                playerUI.SetTime((int)Math.Floor(time));
                break;
        }
    }

    private void HandleScore()
    {
        score = grid.Score;
        playerUI.SetScore(score);
    }

    private void EndGame()
    {
        IsPlaying = false;
        OnEnd?.Invoke();
    }

    public void Stop()
    {
        IsPlaying = false;
    }
}
