using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ElephantController : CharacterControls
{
    public float rotationSpeed = 20f;
    private float currentRotation;
    private float movement;
    public override void OnMove(InputAction.CallbackContext context)
    {

        Vector2 movementInput = context.ReadValue<Vector2>();
        currentRotation = movementInput.x * rotationSpeed;
        
        if (movementInput.y < 0f)
        {
            movementInput.y /= 5f;
        }
        movement = MovementSpeed * movementInput.y;

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, currentRotation/ 10, rb.angularVelocity.z);
        InputActionsManager.Instance.MovementInput = movement * transform.forward;
    }

}
