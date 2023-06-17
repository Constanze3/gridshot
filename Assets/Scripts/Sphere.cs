using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : Target
{
    public GameObject shatteredSphere;

    public override void onShot()
    {
        Destroy(gameObject);
        Instantiate(shatteredSphere, transform.position, Quaternion.identity);
    }
}