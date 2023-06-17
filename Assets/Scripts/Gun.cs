using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform cam;
    public float range;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(!Physics.Raycast(cam.position, cam.forward, out hit, range))
        {
            return;
        }

        if(hit.transform.gameObject.CompareTag("Target"))
        {
            Target target= hit.transform.GetComponent<Target>();
            target.onShot();
        }
    }
}