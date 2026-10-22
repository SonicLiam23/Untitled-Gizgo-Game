
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControls : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public float DefaultMovementSpeed = 5f;
    public float ChangeInMovementSpeed = 0f;
    protected Rigidbody rb;
    protected bool isMoving;

    CharacterCore characterCore;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterCore = GetComponent<CharacterCore>();
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    public void OnToggleFollow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.ToggleFollow();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (characterCore?.currentInteractables.Count > 0)
            {
                foreach (IInteractable i in characterCore.currentInteractables)
                {
                    i.OnInteract(gameObject);
                }
            }
        }
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rb.linearVelocity = Vector3.zero; // Stop movement when switching 
            GameManager.Instance.SwitchCharacter();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public virtual void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMoving = true;
        }    
        else if (context.canceled)
        {
            isMoving = false;
        }


        Vector2 movementInput = context.ReadValue<Vector2>();
        float MovementSpeed = DefaultMovementSpeed + ChangeInMovementSpeed;
        InputActionsManager.Instance.MovementInput = new Vector3(movementInput.x, 0, movementInput.y) * MovementSpeed;
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.ExitMinigame();
        }
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.started) GameManager.Instance.EnterMinigame();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }



    protected virtual void FixedUpdate()
    {
        if (isMoving) rb.linearVelocity = new Vector3(InputActionsManager.Instance.MovementInput.x, rb.linearVelocity.y, InputActionsManager.Instance.MovementInput.z);
    }
}
