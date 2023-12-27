using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform shotPoint;
    public GameObject shotPrefab;

    [Header("Properties")]
    public float range = 10;
    public float shootDelay = 0.1f;

    [Header("Shooting Animation")]
    public float ShootingAnimationEase = 0.3f;
    public float ShootingAnimationGunAngle = 20;

    [Header("Sway Position")]
    public float swayStep = 0.1f;
    public float maxSway = 0.06f;
    public float swayPositionSmooth = 5;

    [Header("Sway Rotation")]
    public float swayRotationStep = 8;
    public float maxRotationSway = 20;
    public float swayRotationSmooth = 5;

    private bool canShoot = true;
    private Coroutine shootingEaseCoroutine;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        Sway();
    }

    private void Shoot()
    {
        if (!canShoot) return;

        canShoot = false;
        Invoke(nameof(ResetShoot), shootDelay);

        // shooting animation
        if (shootingEaseCoroutine != null) StopCoroutine(shootingEaseCoroutine);
        shootingEaseCoroutine = StartCoroutine(ShootingAnimation());

        // sound and muzzle flash
        Instantiate(shotPrefab, shotPoint);

        if (!Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range))
        {
            return;
        }

        if (hit.transform.gameObject.CompareTag("Target"))
        {
            Target target = hit.transform.GetComponent<Target>();
            target.OnShot();
        }
    }
    private void ResetShoot() {
        canShoot = true;
    }


    private void Sway() {
        Vector2 mouseInput = new(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );

        // move gun in opposite direction compared to the current mouse movement
        Vector2 swayPos = mouseInput * -swayStep;
        swayPos.x = Mathf.Clamp(swayPos.x, -maxSway, maxSway);
        swayPos.y = Mathf.Clamp(swayPos.y, -maxSway, maxSway);

        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos, swayPositionSmooth * Time.deltaTime);

        Vector2 swayRotationVector = mouseInput * -swayRotationStep;
        swayRotationVector.x = Mathf.Clamp(swayRotationVector.x, -maxRotationSway, maxRotationSway);
        swayRotationVector.y = Mathf.Clamp(swayRotationVector.y, -maxRotationSway, maxRotationSway);

        // rotate gun in opposite direction compared to the current mouse movement
        // also turns the gun on frontal axis on horizontal mouse movement
        Quaternion swayRotation = Quaternion.Euler(swayRotationVector.y, swayRotationVector.x, swayRotationVector.x);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, swayRotation, swayRotationSmooth * Time.deltaTime);
    }

    private IEnumerator ShootingAnimation()
    {
        float time = 0;
        while (time <= ShootingAnimationEase)
        {
            transform.localRotation = Quaternion.AngleAxis(
                Mathf.LerpAngle(ShootingAnimationGunAngle, 0, time/ ShootingAnimationEase),
                Vector3.left
            );
            time += Time.deltaTime;
            yield return null;
        }
    }
}