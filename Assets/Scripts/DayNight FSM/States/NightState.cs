using UnityEngine;

public class NightState : ITimeState
{
    public float TempRate => 2f;
    public float StateDuration => 60f;
    public void OnEnter()
    {


        Debug.LogError("Entered Night State");

        //throw new System.NotImplementedException();


    }

    public void OnExit()
    {
        Debug.LogError("Exited Night State");

        //throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
       // throw new System.NotImplementedException();
    }
}
