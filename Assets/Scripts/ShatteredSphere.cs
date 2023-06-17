using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShatteredSphere : MonoBehaviour
{
    public float explosionForce = 300f;
    public float explosionRadius = 20f;
    public float shardDelay = 1.5f;
    public float shardShrinkFactor = 1f;

    public AudioClip sound;
    public float maxSoundPitchShift = 0.2f;

    private AudioSource audioSource;
    private int runningCoroutines = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(1f - maxSoundPitchShift, 1f + maxSoundPitchShift);
        audioSource.PlayOneShot(sound);
        foreach(Transform shard in transform)
        {
            Rigidbody shardRb = shard.gameObject.GetComponent<Rigidbody>();
            shardRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            StartCoroutine(ShrinkAndDestroy(shard, shardDelay));
        }
    }

    IEnumerator ShrinkAndDestroy(Transform shard, float delay)
    {
        runningCoroutines++;
        yield return new WaitForSeconds(delay);
        Vector3 newScale = shard.localScale;

        while (newScale.x >= 0.1)
        {
            newScale -= Vector3.one * shardShrinkFactor;
            shard.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(shard.gameObject);
        runningCoroutines--;
        if(runningCoroutines <= 0)
        {
            Destroy(gameObject);
        }
    }
}