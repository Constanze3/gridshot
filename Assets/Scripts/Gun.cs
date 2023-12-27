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
    public float range;
    public float shootDelay;

    [Header("Shooting Animation")]
    public float ShootingAnimationEase = 0.3f;
    public const float ShootingAnimationGunAngle = 20f;

    [Header("Sway")]
    public float swayStep = 0.1f;
    public float maxSwayStepDistance = 0.06f;
    public float swaySmooth = 10f;

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

        // animation
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
        Vector2 mouseInput = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );

        Vector2 swayPos = mouseInput * -swayStep;
        swayPos.x = Mathf.Clamp(swayPos.x, -maxSwayStepDistance, maxSwayStepDistance);
        swayPos.y = Mathf.Clamp(swayPos.y, -maxSwayStepDistance, maxSwayStepDistance);

        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos, swaySmooth * Time.deltaTime);
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