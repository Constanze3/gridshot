using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Camera cam;

    [Header("Camera")]
    [SerializeField] private float sensitivity = 2f;

    [Header("Movement")]
    [SerializeField] private float speed = 3;
    [SerializeField] private float gravity = 9.81f;

    private CharacterController controller;
    private Vector2 rotation = Vector2.zero;
    private bool movementEnabled = true;

    public bool Enabled
    {
        get 
        { return enabled; }
        set
        {
            enabled = value;
            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public bool MovementEnabled
    {
        get { return movementEnabled; }
        set { movementEnabled = value; }
    }

    public float Sensitivity
    { 
        get { return sensitivity; }
        set { sensitivity = value; }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Enabled = true;
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
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

        // rotate camera
        cam.transform.localRotation = Quaternion.Euler(-rotation.y, rotation.x, cam.transform.localRotation.z);

        // rotate body
        Vector3 bodyRotation = body.localRotation.eulerAngles;
        bodyRotation.y = rotation.x;
        body.localRotation = Quaternion.Euler(bodyRotation);
    }

    private void Move()
    {
        // linear falling for now
        controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));

        if (!movementEnabled) return;

        Vector2 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 frontal = movementInput.x * body.right;
        Vector3 sideways = movementInput.y * body.forward;

        Vector3 movement = (frontal + sideways).normalized;

        controller.Move(speed * Time.deltaTime *  movement);
    }
}
