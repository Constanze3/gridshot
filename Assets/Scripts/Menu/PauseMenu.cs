using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public AudioSource audioSource;

    public void OnButtonClicked_Continue()
    { 
        GameManager.Instance.ClosePauseMenu();
    }

    public void OnButtonClicked_Options()
    {
        GameManager.Instance.OpenOptionsMenu();
    }

    public void OnButtonClicked_MainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void OnButtonClicked_Quit()
    {
        GameManager.Instance.Quit();
    }
}
