using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
public enum TARGET_TYPE
{
    HUMAN,
    ELEPHANT,
    NONE
}
public class CameraAdjust : MonoBehaviour
{


    public Vector3 PositionOffset;
    public Vector3 Rotation;
    private GameObject Target;
    [Range(30f, 120f)]
    public float humanFOV = 60f;
    [Range(30f, 120f)]
    public float elephantFOV = 90f;

    public void SetTarget(GameObject newTarget, TARGET_TYPE type)
    {
        Target = newTarget;
        if (type == TARGET_TYPE.HUMAN)
        {
            Camera.main.fieldOfView = humanFOV;
        }
        else if (type == TARGET_TYPE.ELEPHANT)
        {
            Camera.main.fieldOfView = elephantFOV;
        }
        else
        {
            //fallback to default value
            Camera.main.fieldOfView = 60f;
        }
    }
    private void Update()
    {
        transform.position = Target.transform.position + PositionOffset;
        transform.rotation = Quaternion.Euler(Rotation);
    }

}
