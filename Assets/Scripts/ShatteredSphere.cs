using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredSphere : MonoBehaviour
{
    public float explosionForce;
    public float explosionRadius;
    public float shardShrinkFactor;
    public AudioClip sound;

    private AudioSource audioSource;
    private int runningCoroutines = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound);
        foreach(Transform shard in transform)
        {
            Rigidbody shardRb = shard.gameObject.GetComponent<Rigidbody>();
            shardRb.AddExplosionForce(explosionForce, shard.position, explosionRadius);
            StartCoroutine(ShrinkAndDestroy(shard, 5));
        }
    }

    IEnumerator ShrinkAndDestroy(Transform shard, float delay)
    {
        runningCoroutines++;
        yield return new WaitForSeconds(delay);
        Vector3 newScale = shard.localScale;

        while (newScale.x >= 0)
        {
            newScale -= Vector3.one * shardShrinkFactor;
            shard.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(shard);
        runningCoroutines--;
        if(runningCoroutines <= 0)
        {
            Destroy(gameObject);
        }
    }
}