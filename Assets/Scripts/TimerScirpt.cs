using UnityEngine;
using TMPro;
using System;

public class TimerScirpt : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    [Header("State Time Range")]

    [SerializeField] private float morningStart = 0.0f;
    [SerializeField] private float morningEnd = 0.5f;
    [SerializeField] private float afternoonStart = 0.5f;
    [SerializeField] private float afternoonEnd = 0.9f;
    [SerializeField] private float nightStart = 0.9f;
    [SerializeField] private float nightEnd = 1.0f;



    [Header("Sun")]
    [SerializeField] private Light sun;
    [SerializeField] private Gradient sunColour;
    [SerializeField] private AnimationCurve sunIntensity;

    [Header("Moon")]
    [SerializeField] private Light moon;
    [SerializeField] private Gradient moonColour;
    [SerializeField] private AnimationCurve moonIntensity;  

    [Header("Other Light Settings")]

    [SerializeField] private AnimationCurve lightIntensityMultiplier;
    [SerializeField] private AnimationCurve reflectionsIntensityMultiplier;

    [SerializeField] private Vector3 Afternoon;


    [Range(0.0f, 1.0f)]
    public float time;
    private float stateDuration;
    public float timeRemaining;

    void Start()
    {
        stateDuration = DayCycleStateMachine.Instance.CurrentState.StateDuration;
        timeRemaining = stateDuration;
    }
    void Update()
    {

        timeRemaining -= Time.deltaTime;
        timerText.text = "Time Remaining: " + Mathf.Round(timeRemaining).ToString();
        
        //Get the progress of time from the duration of the current state and how long its left from it and give the time slider its value
        float timeProgress = 1f -  (timeRemaining/stateDuration);
        time = Mathf.Lerp(GetStateStartTime(), GetStateEndTime(), timeProgress);

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerText.text = "Time's Up!";

            timerEnded();
        }

        //Visual linear rotation of the sun and moon based on the current time

        //sun.trasnform.eulerAngles = (time - 0.25f) * Afternoon * 4.0f;
        //moon.transform.eulerAngles = (time - 0.75f) * Afternoon * 4.0f;

        float sunRotationAngle = (time / 0.9f) * 180f;
        sun.transform.localRotation = Quaternion.Euler(sunRotationAngle, 0, 0);

        float moonRotationAngle = ((time - 0.9f) / 0.1f) * 180f;
        moon.transform.localRotation = Quaternion.Euler(moonRotationAngle, 0, 0);
        sun.color = sunColour.Evaluate(time);
        moon.color = moonColour.Evaluate(time);

        //Changes the intensity and reflections from the sun and moon on the skybox
        RenderSettings.ambientIntensity = lightIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);
    }


    float GetStateStartTime()
    {
        if (DayCycleStateMachine.Instance.CurrentState is MorningState) return morningStart;
        if (DayCycleStateMachine.Instance.CurrentState is AfternoonState) return afternoonStart;
        return nightStart;
    }

    float GetStateEndTime()
    {
        if (DayCycleStateMachine.Instance.CurrentState is MorningState) return morningEnd;
        if (DayCycleStateMachine.Instance.CurrentState is AfternoonState) return afternoonEnd;
        return nightEnd;
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

            if(sun.gameObject.activeInHierarchy && !moon.gameObject.activeInHierarchy)
            {
                sun.gameObject.SetActive(false);
                moon.gameObject.SetActive(true);
            }
        }
        else
        {
            DayCycleStateMachine.Instance.ChangeState(typeof(MorningState));

            if (!sun.gameObject.activeInHierarchy && moon.gameObject.activeInHierarchy)
            {
                sun.gameObject.SetActive(true);
                moon.gameObject.SetActive(false);
            }
        }

        timeRemaining = DayCycleStateMachine.Instance.CurrentState.StateDuration;

        // Time.timeScale = 0f;
    }

    public void SetTimePasses(bool shouldTimePass)
    {
        enabled = shouldTimePass;
    }
}
