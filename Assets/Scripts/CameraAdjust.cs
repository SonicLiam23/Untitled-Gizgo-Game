using UnityEngine;

public class CameraAdjust : MonoBehaviour
{


    public Vector3 PositionOffset;
    public Vector3 Rotation;
    private GameObject Target;
    [Range(30f, 120f)]
    public float humanFOV = 60f;
    [Range(30f, 120f)]
    public float elephantFOV = 90f;

    public void SetTarget(GameObject newTarget, CHARACTER type = CHARACTER.NONE)
    {
        Target = newTarget;
        if (type == CHARACTER.HUMAN)
        {
            Camera.main.fieldOfView = humanFOV;
        }
        else if (type == CHARACTER.ELEPHANT)
        {
            Camera.main.fieldOfView = elephantFOV;
        }
        else
        {
            //fallback to default value
            Camera.main.fieldOfView = 60f;
        }
    }


    public void SetTarget(CurrentCharacter character)
    {
        SetTarget(character.gameObject, character.type);
    }


    private void Update()
    {
        transform.position = Target.transform.position + PositionOffset;
        transform.rotation = Quaternion.Euler(Rotation);
    }

}
