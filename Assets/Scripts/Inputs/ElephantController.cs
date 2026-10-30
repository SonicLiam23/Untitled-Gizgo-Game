
using UnityEngine;
using UnityEngine.InputSystem;


public class ElephantController : CharacterControls
{
    public float rotationSpeed = 20f;
    private float currentRotation;
    private float movement;
    [Header("Velocity System")]
    [SerializeField] private float baseMaxVelocity = 2f;   // starting max velocity when holding forward
    [SerializeField] private float maxVelocityCap = 10f;   // absolute cap for max velocity after shift taps
    [SerializeField] private float sprintStep = 2f;        // amount added to max velocity per shift tap
    [SerializeField] private float acceleration = 3f;      // units per second when holding forward
    [SerializeField] private float deceleration = 4f;      // units per second when releasing forward
    [SerializeField] private float boostStep = 2f;    // amount added per press
    [SerializeField] private float maxBoost = 8f;    // max absolute boost
    [SerializeField] private float decayRate = 2f;    // units per second (1 => 5 units decays in 5s)
    private float boostSpeed = 0f;                    // current extra speed (+ forward, - reverse)

    private float currentMaxVelocity; // current target max (starts at baseMaxVelocity)
    private float currentVelocity;    // actual current forward velocity (signed)
    private bool forwardHeld = false;
    private bool backwardHeld = false;
    [SerializeField] private float reverseMaxVelocity = 2f; // max speed in reverse (positive value, used as negative)

    // ... existing fields like MovementSpeed, rotationSpeed, currentRotation, movement ...

    public override void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();

        // Add a boost only on input start (button/stick press)
        if (context.started)
        {
            isMoving = true;
            if (movementInput.y > 0f)
            {
                if (boostSpeed < 0f) boostSpeed = 0f; // reset if switching direction
                boostSpeed = Mathf.Min(boostSpeed + boostStep, maxBoost);
            }
            else if (movementInput.y < 0f)
            {
                if (boostSpeed > 0f) boostSpeed = 0f;
                boostSpeed = Mathf.Max(boostSpeed - boostStep, -maxBoost);
            }
        }
        else if (context.canceled)
        {
            isMoving = false;
        }

        // rotation same as before
        currentRotation = movementInput.x * rotationSpeed;

        // track whether forward/backward is being held (used in Update for velocity changes)
        forwardHeld = movementInput.y > 0f;
        backwardHeld = movementInput.y < 0f;

        // movement value will be driven by currentVelocity (set in Update/FixedUpdate)
        // keep 'movement' populated for other code that may reference it
        movement = currentVelocity;
    }

    private void Update()
    {
        // Initialize currentMaxVelocity on first Update
        if (currentMaxVelocity == 0f)
            currentMaxVelocity = baseMaxVelocity;

        // Check keyboard shift tap as a fallback (in case input system binding isn't used)
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null && (kb.leftShiftKey.wasPressedThisFrame || kb.rightShiftKey.wasPressedThisFrame))
        {
            IncreaseMaxVelocity();
        }

        // Accelerate toward the appropriate target while forward or backward is held, otherwise decelerate toward 0
        if (forwardHeld)
        {
            currentVelocity = Mathf.MoveTowards(currentVelocity, currentMaxVelocity, acceleration * Time.deltaTime);
        }
        else if (backwardHeld)
        {
            // target is negative reverse max
            currentVelocity = Mathf.MoveTowards(currentVelocity, -reverseMaxVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Mathf.MoveTowards(currentVelocity, 0f, deceleration * Time.deltaTime);
        }

        // keep movement in sync
        movement = currentVelocity;
        currentMaxVelocity = Mathf.Max(currentMaxVelocity - Time.deltaTime, baseMaxVelocity);

    }

    // Optional input callback: wire this to your Sprint (Shift) action in the Input System
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            IncreaseMaxVelocity();
    }

    private void IncreaseMaxVelocity()
    {
        currentMaxVelocity = Mathf.Min(currentMaxVelocity + sprintStep, maxVelocityCap);
        // ensure currentVelocity doesn't exceed new max immediately (optional)
        currentVelocity = Mathf.Min(currentVelocity, currentMaxVelocity);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, currentRotation/ 10, rb.angularVelocity.z);
        InputActionsManager.Instance.MovementInput = movement * transform.forward;
    }


    //Reset all variables so that velocity isn't retained when switching off and back on to the elephant
    private void OnDisable()
    {
        movement = 0f;
        currentRotation = 0f;
        currentMaxVelocity = baseMaxVelocity;
        currentVelocity = 0f;
        boostSpeed = 0f;
        forwardHeld = false;
        backwardHeld = false;
    }
}
