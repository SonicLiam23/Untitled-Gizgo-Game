using UnityEngine;
using UnityEngine.InputSystem;
public class MousePoint : MonoBehaviour
{

    [SerializeField] private Transform pointTransform;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 0.01f;

    private bool isFollowingMouse;

    private void Start()
    {
        if (pointTransform == null)
            pointTransform = transform;

        if (playerCamera == null)
            playerCamera = Camera.main;

    }

    private void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null || playerCamera == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
            isFollowingMouse = true;

        if (mouse.leftButton.wasReleasedThisFrame)
            isFollowingMouse = false;

        if (!isFollowingMouse || playerCamera == null)
            return;

        Vector2 mouseDelta = mouse.delta.ReadValue();
        Vector3 movement = playerCamera.transform.right * mouseDelta.x;
        movement += playerCamera.transform.up * mouseDelta.y;

        pointTransform.position += movement * mouseSensitivity;
    }
}
