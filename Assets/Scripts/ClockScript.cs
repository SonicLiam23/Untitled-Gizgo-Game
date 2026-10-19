using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private Transform clockHandTransform;
    private void Awake()
    {
        clockHandTransform = transform.Find("ClockHand");
       


       
    }

    private void Update()
    {
      clockHandTransform.eulerAngles = new Vector3(0, 0, -Time.realtimeSinceStartup * 3f);
    }
}