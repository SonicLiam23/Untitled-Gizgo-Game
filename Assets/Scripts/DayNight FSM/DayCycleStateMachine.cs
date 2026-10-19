using System;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleStateMachine : StateMachineBase<ITimeState, DayCycleStateMachine>
{
    // unsure if these would help readability or not yet.
    //Type morning => typeof(MorningState);
    //Type afternoon => typeof(AfternoonState);
    //Type night => typeof(NightState);


    public static DayCycleStateMachine Instance {  get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            this.enabled = false;
            Debug.LogWarning("You have multiple instances of the time cycle state machine. Disabling this one");
        }

        Instance = this;
        // initialize the state dict!
        states = new Dictionary<Type, ITimeState>
        {
            {typeof(NightState), new NightState() },
            {typeof(AfternoonState), new AfternoonState() },
            {typeof(MorningState), new MorningState() },
        };
    }

    private void Start()
    {
        ChangeState(typeof(MorningState));
    }

    protected override void Update()
    {
        base.Update();
    }
}
