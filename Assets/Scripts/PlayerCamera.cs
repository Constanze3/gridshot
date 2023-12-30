using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity = 2f;

    private Vector2 rotation = Vector2.zero;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (GameManager.Instance.gameState != GameManager.State.Playing) return;

        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        if (rotation.x < 0)
        {
            rotation.x += 360;
        }
        else if (360 < rotation.x)
        {
            rotation.x -= 360;
        }

        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -90, 90);

        transform.localRotation = Quaternion.Euler(-rotation.y, rotation.x, transform.localRotation.z);
    }
}
