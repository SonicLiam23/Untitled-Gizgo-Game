using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private Transform clockHandTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        clockHandTransform = transform.Find("ClockHand");
    }
    private void Update()
    {
        clockHandTransform.eulerAngles = new Vector3(0, 0, -Time.time * 2f); // Rotate the clock hand based on time
    }
}
