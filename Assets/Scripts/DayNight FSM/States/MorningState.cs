using UnityEngine;

public class MorningState : ITimeState
{

    public float TempRate => 1f;
    public float StateDuration => 90f;
    public void OnEnter()
    {
        Debug.Log("Entered Morning State");

        //throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        Debug.Log("Exited Morning State");

        //throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        //throw new System.NotImplementedException();
    }
}
