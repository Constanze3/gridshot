using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredSphereManager : MonoBehaviour
{
    public int maxShatteredSpheres;

    List<ShatteredSphere> shatteredSpheres = new List<ShatteredSphere>();

    public void addShatteredSphere(ShatteredSphere shatteredSphere)
    {
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
