using UnityEngine;

public class AfternoonState : ITimeState
{
    public float TempRate => 0f;
    public float StateDuration => 60f;
    public void OnEnter()
    {
        Debug.LogError("Entered Afternoon State");

        throw new System.NotImplementedException();
    }

    public void OnExit()
    {

        Debug.LogError("Exited Afternoon State");

        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
