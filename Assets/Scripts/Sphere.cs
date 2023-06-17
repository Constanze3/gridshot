using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Sphere : Target
{
    public GameObject shatteredSphere;

    public override void onShot()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
        Instantiate(shatteredSphere, transform.position, Quaternion.identity);
    }
}
