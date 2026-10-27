using UnityEngine;
using UnityEngine.InputSystem;
public class MousePoint : MonoBehaviour
{

    [SerializeField] private Transform pointTransform;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float MouseSensitivity = 0.01f;

    private bool isFollowingMouse;

    private void Start()
    {
        pointTransform ??= transform;
            //pointTransform = transform;

        PlayerCamera ??= Camera.main;
            //PlayerCamera = Camera.main;

    }

    private void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null || PlayerCamera == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
            isFollowingMouse = true;

        if (mouse.leftButton.wasReleasedThisFrame)
            isFollowingMouse = false;

        if (!isFollowingMouse || PlayerCamera == null)
            return;

        Vector2 mouseDelta = mouse.delta.ReadValue();
        Vector3 movement = PlayerCamera.transform.right * mouseDelta.x;
        movement += PlayerCamera.transform.up * mouseDelta.y;

        pointTransform.position += movement * MouseSensitivity;
    }
}
