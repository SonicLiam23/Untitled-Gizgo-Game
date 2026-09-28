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

    public void SetTarget(GameObject newTarget, TARGET_TYPE type)
    {
        Target = newTarget;
        if (type == TARGET_TYPE.HUMAN)
        {
            Camera.main.fieldOfView = 60f;
        }
        else if (type == TARGET_TYPE.ELEPHANT)
        {
            Camera.main.fieldOfView = 90f;
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
