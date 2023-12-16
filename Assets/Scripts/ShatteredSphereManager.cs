using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for limiting the number of shattered spheres.
/// </summary>
public class ShatteredSphereManager : MonoBehaviour
{
    public int maxShatteredSpheres;
    private readonly List<ShatteredSphere> shatteredSpheres = new();

    /// <summary>
    /// Adds a shattered sphere to the manager.
    /// </summary>
    /// <param name="shatteredSphere">
    /// shattered sphere
    /// </param>
    public void AddShatteredSphere(ShatteredSphere shatteredSphere)
    {
        shatteredSphere.transform.SetParent(transform);
        shatteredSpheres.Add(shatteredSphere);
        if (maxShatteredSpheres < shatteredSpheres.Count)
        {
            for (int i =  shatteredSpheres.Count - maxShatteredSpheres - 1; 0 <= i; i--)
            {
                if (shatteredSphere == null)
                {
                    shatteredSpheres.RemoveAt(i);
                    continue;
                }

                shatteredSpheres[i].shardDelay = 0;
            }
        }
    }
}
