using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private  TMP_Text scoreText;
    [SerializeField] private Image movementLockImage;

    [SerializeField] private Sprite movementLockedSprite;
    [SerializeField] private Sprite movementUnlockedSprite;

    private struct TimeForDisplay {
        public float minutes;
        public float seconds;

        public string getString() {
            string seconds_str = seconds.ToString();
            if (seconds < 10) {
                seconds_str = "0" + seconds_str;
            }

            return string.Format("{0}:{1}", minutes, seconds_str);
        }
    }
    private TimeForDisplay timeForDisplay;

    private void Start()
    {
        SetTime(0);
        SetScore(0);
    }

    /// <summary>
    /// Sets the displayed time.
    /// </summary>
    /// <param name="seconds"></param>
    public void SetTime(int seconds)
    {
        seconds = Mathf.Max(0, seconds);

        timeForDisplay = new()
        {
            minutes = seconds / 60,
            seconds = seconds % 60
        };

        timeText.text = timeForDisplay.getString();
    }

    /// <summary>
    /// Sets the displayed score.
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(int score) 
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Updates the movement lock icon.
    /// </summary>
    /// <param name="value"></param>
    public void SetMovementLock(bool value)
    {
        if (value)
        {
            movementLockImage.sprite = movementLockedSprite;
        }
        else
        {
            movementLockImage.sprite = movementUnlockedSprite;
        }
    }
}
