using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFreeLookController : MonoBehaviour
{
    public float sensitivity = 2f;

    private InputAction rotationAction;

    private void Start()
    {
        rotationAction = new InputAction(binding: "<Mouse>/delta");
        rotationAction.Enable();
    }

    private void Update()
    {
        Vector2 rotationInput = rotationAction.ReadValue<Vector2>() * sensitivity;

        float rotationX = transform.localRotation.eulerAngles.x - rotationInput.y;
        float rotationY = transform.localRotation.eulerAngles.y + rotationInput.x;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
