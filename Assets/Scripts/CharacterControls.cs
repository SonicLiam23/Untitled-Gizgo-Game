using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControls : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public float MovementSpeed = 5f;
    private Rigidbody rb;

    CharacterCore characterCore;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterCore = GetComponent<CharacterCore>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (characterCore?.currentInteractables.Count > 0)
                characterCore?.currentInteractables[0].OnInteract(gameObject);
        }
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        rb.linearVelocity = Vector3.zero; // Stop movement when switching characters
        GameManager.Instance.OnSwitch();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        InputActionsManager.Instance.MovementInput = new Vector3(movementInput.x, 0, movementInput.y) * MovementSpeed;
        
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }



    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(InputActionsManager.Instance.MovementInput.x, rb.linearVelocity.y, InputActionsManager.Instance.MovementInput.z);
    }
}
