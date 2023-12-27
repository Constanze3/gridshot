using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Invoke("OnAudioOver", audioSource.clip.length);
    }

    private void OnAudioOver()
    {
        Destroy(gameObject);
    }
}
