using UnityEngine;
using UnityEngine.InputSystem;

public class ElephantController : CharacterControls
{
    [Header("Steering")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float steeringDamping = 10f;

    [Header("Base Speed & Limits")]
    [SerializeField] private float baseMaxVelocity = 2f;    // holding W speed
    [SerializeField] private float maxVelocityCap = 10f;    // absolute cap after sprint taps
    [SerializeField] private float reverseMaxVelocity = 2f; // max reverse speed

    [Header("Shift to sprint")]
    [SerializeField] private float sprintStep = 1.5f;       // boost added per shift tap
    [SerializeField] private float sprintDecayRate = 2f;    // velocity lost per second back to base speed

    [Header("Acceleration")]
    [SerializeField] private float acceleration = 4f;       // speed buildup rate
    [SerializeField] private float deceleration = 5f;       // braking/coasting rate

    private float currentMaxVelocity;
    private float currentVelocity;
    private float currentRotation;
    private float movement;
    private bool forwardHeld;
    private bool backwardHeld;

    public override void Awake()
    {
        base.Awake();
        currentMaxVelocity = baseMaxVelocity;
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) isMoving = true;


        Vector2 input = context.ReadValue<Vector2>();

        // Tank rotation: A/D or Left Stick X
        currentRotation = input.x * rotationSpeed;

        // Forward / Backward tracking: W/S or Left Stick Y
        forwardHeld = input.y > 0.1f;
        backwardHeld = input.y < -0.1f;

        if (context.canceled)
        {
            isMoving = false;
            forwardHeld = false;
            backwardHeld = false;
            currentRotation = 0f;
        }
    }

    // Bind this to your 'Sprint' action (Shift / Controller Button) in Unity's Input Action asset
    public override void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started && forwardHeld)
        {
            ApplySprintTap();
        }
    }

    private void Update()
    {
        // "decelerate" current max vel to base
        if (currentMaxVelocity > baseMaxVelocity)
        {
            currentMaxVelocity = Mathf.MoveTowards(currentMaxVelocity, baseMaxVelocity, sprintDecayRate * Time.deltaTime);
        }

        // "actual" linear velocity (with respect to the forward axis, not just any)
        float actualForwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        // pushing forward but physically stuck against an obstacle:
        if (forwardHeld && currentVelocity > 1f && actualForwardSpeed < 0.4f)
        {
            // Clamp velocity down to real speed so you don't build phantom momentum (the sprint trying to push you forward)
            currentVelocity = Mathf.Max(0f, actualForwardSpeed);
            currentMaxVelocity = baseMaxVelocity; // optionally wipe sprint boost on impact
        }
        // Same for backing into a wall
        else if (backwardHeld && currentVelocity < -1f && actualForwardSpeed > -0.4f)
        {
            currentVelocity = Mathf.Min(0f, actualForwardSpeed);
        }

        // calculate target velocity based on input
        float targetVelocity = 0f;
        if (forwardHeld)
        {
            targetVelocity = currentMaxVelocity;
        }
        else if (backwardHeld)
        {
            targetVelocity = -reverseMaxVelocity;
        }

        bool isCounterBraking = (backwardHeld && currentVelocity > 0.1f) || (forwardHeld && currentVelocity < -0.1f);

        float rate;
        if (isCounterBraking)
        {
            rate = deceleration * 4f; // 4x braking bite so S immediately halts forward movement
        }
        else if (forwardHeld || backwardHeld)
        {
            rate = acceleration;
        }
        else
        {
            rate = deceleration;
        }

        currentVelocity = Mathf.MoveTowards(currentVelocity, targetVelocity, rate * Time.deltaTime);
        movement = currentVelocity;

        isMoving = forwardHeld || backwardHeld || Mathf.Abs(currentVelocity) > 0.05f || Mathf.Abs(currentRotation) > 0.1f;
    }

    private void ApplySprintTap()
    {
        // increase max speed the max speed
        currentMaxVelocity = Mathf.Min(currentMaxVelocity + sprintStep, maxVelocityCap);

        // Give a small impulse so tapping Shift feels "punchy"
        currentVelocity = Mathf.Min(currentVelocity + (sprintStep * 0.75f), currentMaxVelocity);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Tank steering
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, currentRotation / steeringDamping, rb.angularVelocity.z);

        // Tank forward/backward
        InputActionsManager.Instance.MovementInput = movement * transform.forward;
    }

    private void OnDisable()
    {
        movement = 0f;
        currentRotation = 0f;
        currentVelocity = 0f;
        currentMaxVelocity = baseMaxVelocity;
        forwardHeld = false;
        backwardHeld = false;
    }
}