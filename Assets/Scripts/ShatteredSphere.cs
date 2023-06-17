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
        ShatteredSphereManager manager = GameObject.Find("ShatteredSphereManager").GetComponent<ShatteredSphereManager>();
        transform.SetParent(manager.transform);
        manager.addShatteredSphere(this);

        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(1f - maxSoundPitchShift, 1f + maxSoundPitchShift);
        audioSource.PlayOneShot(sound);
        foreach (Transform shard in transform)
        {
            Rigidbody shardRb = shard.gameObject.GetComponent<Rigidbody>();
            shardRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            StartCoroutine(ShrinkAndDestroy(shard));
        }
    }

    IEnumerator ShrinkAndDestroy(Transform shard)
    {
        runningCoroutines++;
        float timePassed = 0;

        while (timePassed < shardDelay)
        {
            yield return new WaitForSeconds(0.05f);
            timePassed += 0.05f;
        }
        Vector3 newScale = shard.localScale;

        while (newScale.x >= 0.1)
        {
            newScale -= Vector3.one * shardShrinkFactor;
            shard.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(shard.gameObject);
        runningCoroutines--;
        if (runningCoroutines <= 0)
        {
            Destroy(gameObject);
        }
    }
}