using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public Color defaultColor = Color.black;
    public Color selectedColor = Color.green;

    [Serializable]
    public struct Editor_TextButton
    {
        public string name;
        public TMP_Text text;
    }

    public Editor_TextButton[] gameModeButtons;

    private AudioSource audioSource;
    private Dictionary<string, TMP_Text> gameModeButtonTexts;
    private Game.Settings settings;

    private void LoadEditorData()
    {
        gameModeButtonTexts = new Dictionary<string, TMP_Text>();
        foreach (Editor_TextButton bt in gameModeButtons)
        {
            gameModeButtonTexts.Add(bt.name, bt.text);
        }
    }

    private void Awake()
    {
        LoadEditorData();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        settings = new Game.Settings();
        GameMode_UpdateDisplay("Zen");
    }

    private void GameMode_UpdateDisplay(string name) {
        gameModeButtonTexts[name].color = selectedColor;
        foreach (KeyValuePair<string, TMP_Text> entry in gameModeButtonTexts)
        {
            if (entry.Key == name) continue;
            entry.Value.color = defaultColor;
        }
    }

    public void GameMode_ButtonClicked(string name) {
        Game.GameMode gameMode = name switch
        {
            "Zen" => Game.GameMode.Zen,
            "Minute" => Game.GameMode.Timer,
            _ => throw new NullReferenceException()
        };

        if (settings.GameMode == gameMode) return;

        settings.GameMode = gameMode;
        if (name == "Minute") {
            settings.TimeLimit = 60;
        }

        GameMode_UpdateDisplay(name);
        audioSource.Play();
    }

    public void StartClicked() {
        if (settings == null) return;
        GameManager.Instance.TryStartGame(settings);
    }
}
