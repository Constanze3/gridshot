using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 3;
    public float gravity = 9.81f;

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        movement.Normalize();

        characterController.Move(speed * Time.deltaTime * movement);
        characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
    }
}
