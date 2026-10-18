using UnityEngine;
using TMPro;

public class TimerScirpt : MonoBehaviour
{
  public TextMeshProUGUI timerText;
    public float timeRemaining;
    ITimeState timeState;
    void Start()
    {
        timeState = DayCycleStateMachine.Instance.CurrentState;
        timeRemaining = timeState.StateDuration;
    }
    void Update()
    {

        timeRemaining -= Time.deltaTime;
        timerText.text = "Time Remaining: " + Mathf.Round(timeRemaining).ToString();


        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerText.text = "Time's Up!";

            timerEnded();
        }
    }

    //IM NOT SURE IF THERE IS A BETTER WAY TO DO THIS RIGHT NOW, IF THERE IS LET ME KNOW
    void timerEnded()
    {
        //Change State
        if(DayCycleStateMachine.Instance.CurrentState is MorningState)
        {
            DayCycleStateMachine.Instance.ChangeState(typeof(AfternoonState));
        }
        else if(DayCycleStateMachine.Instance.CurrentState is AfternoonState)
        {
            DayCycleStateMachine.Instance.ChangeState(typeof(NightState));
        }
        else
        {
            DayCycleStateMachine.Instance.ChangeState(typeof(MorningState));
        }

        timeState = DayCycleStateMachine.Instance.CurrentState;
        timeRemaining = timeState.StateDuration;

        // Time.timeScale = 0f;
    }
}
