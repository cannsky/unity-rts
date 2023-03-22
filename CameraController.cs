using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float rotationSmoothTime = 0.1f;
    public float zoomSpeed = 5.0f;
    public float zoomSmoothTime = 0.1f;

    private Vector3 movement;
    private Vector2 rotation;
    private Vector2 currentRotationVelocity;
    private Vector2 currentRotation;
    private float zoomInput;
    private float currentZoom;
    private float currentZoomVelocity;

    void Update()
    {
        // Get the movement input from WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            // Hide the cursor and lock it in the center of the screen
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Get the rotation input from the mouse
            float rotationHorizontal = Input.GetAxis("Mouse X");
            float rotationVertical = -Input.GetAxis("Mouse Y");

            // Calculate the target rotation vector
            rotation = new Vector2(rotationHorizontal, rotationVertical);
        }
        else
        {
            // Show the cursor and unlock it
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            rotation = Vector2.zero;
        }

        // Smoothly interpolate the current rotation to the target rotation
        currentRotation = Vector2.SmoothDamp(currentRotation, rotation, ref currentRotationVelocity, rotationSmoothTime);

        // Get the zoom input from the mouse wheel
        zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // Smoothly interpolate the current zoom to the target zoom
        currentZoom = Mathf.SmoothDamp(currentZoom, zoomInput, ref currentZoomVelocity, zoomSmoothTime);

        // Calculate the movement vector based on the camera's rotation
        movement = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        movement.y = 0.0f; // Keep movement on the XZ plane

        // Update the movement vector based on the current zoom
        movement += transform.forward * currentZoom * zoomSpeed;
    }

    void FixedUpdate()
    {
        // Apply movement
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime, Space.World);

        // Apply rotation
        Vector3 newForward = Quaternion.AngleAxis(currentRotation.x * rotationSpeed * Time.fixedDeltaTime, Vector3.up) * transform.forward;
        newForward = Quaternion.AngleAxis(currentRotation.y * rotationSpeed * Time.fixedDeltaTime, transform.right) * newForward;
        transform.forward = newForward;
    }
}