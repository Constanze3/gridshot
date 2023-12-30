using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform shotPoint;
    public GameObject shotPrefab;
    public AudioSource audioSource;

    [Header("Properties")]
    public float range = 10;
    public float shootDelay = 0.1f;

    [Header("Shooting Animation")]
    public float shootingAnimationEase = 0.3f;
    public float shootingAnimationGunAngle = 20;

    [Header("Reload Animation")]
    public float reloadAnimationDuration = 0.5f;

    [Header("Sway Position")]
    public float swayStep = 0.1f;
    public float maxSway = 0.06f;
    public float swayPositionSmooth = 5;

    [Header("Sway Rotation")]
    public float swayRotationStep = 8;
    public float maxRotationSway = 20;
    public float swayRotationSmooth = 5;

    private bool isShooting = false;

    private bool isReloading = false; // if true we are reloading currently
    private bool isReloading2 = false; // if false we are not allowed to reload

    // booleans for disabling shooting and reloading
    private bool canShoot = true;
    private bool canReload = true;

    private Coroutine shootingEaseCoroutine;

    private void Update()
    {
        if (GameManager.Instance.gameState != GameManager.State.Playing) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        Sway();
    }

    private void Shoot()
    {
        if (!canShoot || isShooting || isReloading) return;

        isShooting = true;
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

    private void ResetShoot()
    {
        isShooting = false;
    }

    private IEnumerator ShootingAnimation()
    {
        isShooting = true;
        float time = 0;
        while (time <= shootingAnimationEase)
        {
            transform.localRotation = Quaternion.AngleAxis(
                Mathf.LerpAngle(shootingAnimationGunAngle, 0, time / shootingAnimationEase),
                Vector3.left
            );
            time += Time.deltaTime;
            yield return null;
        }
        isShooting = false;
    }

    private void Reload()
    {
        if (!canReload || isReloading2 || isShooting) return;

        audioSource.Play();
        StartCoroutine(ReloadAnimation());
    }

    private IEnumerator ReloadAnimation()
    {
        isReloading = true;
        isReloading2 = true;

        float time = 0;
        while (time <= reloadAnimationDuration)
        {
            float angle = Mathf.Lerp(0, 360, time / reloadAnimationDuration);
            transform.localRotation = Quaternion.AngleAxis(angle, Vector3.left);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = Quaternion.AngleAxis(0, Vector3.left);
        isReloading = false;

        // little extra delay at the end
        // also for the sound to finish
        // before allowing reloading again
        yield return new WaitForSeconds(0.2f);
        isReloading2 = false;

    }

    /// <summary>
    /// The gun will lean (both with position and rotation) in the opposite direction the mouse is moving.
    /// </summary>
    private void Sway()
    {
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


}