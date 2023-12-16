
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Sphere : Target
{
    public Material tintedGlassMaterial;
    public ShatteredSphere shatteredSphere;

    private Material material;

    private void Start()
    {
        material = new Material(tintedGlassMaterial)
        {
            color = UnityEngine.Random.ColorHSV()
        };

        GetComponent<Renderer>().material = material;
    }

    /// <summary>
    /// Executes when the sphere is shot.
    /// </summary>
    public override void OnShot()
    {
        gameObject.SetActive(false);
        SignalOnDestroy();

        ShatteredSphere shattered = Instantiate(shatteredSphere, transform.position, Quaternion.identity);
        shattered.SetMaterial(material);

        Destroy(gameObject);
    }
}
