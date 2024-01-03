using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class managing all shattered spheres.
/// </summary>
public class ShatteredSphereManager : MonoBehaviour
{
    [SerializeField] private int maxShatteredSpheres;
    private readonly List<ShatteredSphere> shatteredSpheres = new();

    private readonly HashSet<Collider> ignoreCollisionWith = new();

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
            for (int i = shatteredSpheres.Count - maxShatteredSpheres - 1; 0 <= i; i--)
            {
                if (shatteredSphere == null)
                {
                    shatteredSpheres.RemoveAt(i);
                    continue;
                }

                shatteredSpheres[i].shardDelay = 0;
            }
        }

        foreach (Collider collider in new HashSet<Collider>(ignoreCollisionWith))
        {
            if (collider == null)
            {
                ignoreCollisionWith.Remove(collider);
                continue;
            }
            else
            {
                foreach (Transform shard in shatteredSphere.transform)
                {
                    Physics.IgnoreCollision(shard.gameObject.GetComponent<Collider>(), collider);
                }
            }
        }
    }

    /// <summary>
    /// Disables the collision of all shards of shattered spheres with the provided collider.
    /// </summary>
    /// <param name="collider"></param>
    public void DisableCollision(Collider collider)
    {
        ignoreCollisionWith.Add(collider);
    }
}
