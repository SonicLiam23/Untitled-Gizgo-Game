using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    public Vector3 PositionOffset;
    public Vector3 Rotation;
    private GameObject Target;

    public void SetTartet(GameObject newTarget)
    {
        Target = newTarget;
    }

    private void Update()
    {
        transform.position = Target.transform.position + PositionOffset;
        transform.rotation = Quaternion.Euler(Rotation);
    }

}
