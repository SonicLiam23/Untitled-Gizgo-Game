using UnityEngine;
using TMPro;

public class TimerScirpt : MonoBehaviour
{
  public TextMeshProUGUI timerText;
    public float timeRemaining = 120f;
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
        // Pause game
        Time.timeScale = 0f;
        
    }
}
