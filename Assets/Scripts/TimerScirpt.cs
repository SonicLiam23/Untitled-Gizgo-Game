using UnityEngine;
using TMPro;

public class TimerScirpt : MonoBehaviour
{
  public TextMeshProUGUI timerText;
    public float timeRemaining;
    void Start()
    {
        timeRemaining = DayCycleStateMachine.Instance.CurrentState.StateDuration;
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

        timeRemaining = DayCycleStateMachine.Instance.CurrentState.StateDuration;

        // Time.timeScale = 0f;
    }
}
