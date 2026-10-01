using UnityEngine;

public class CampfireInteract : MonoBehaviour, IInteractable
{
    public GameObject OnEnterRadius(Collider other)
    {
        Debug.Log("Entered Campfire Radius");
        GameManager.Instance.isCampfireActive = true;
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        Debug.Log("Exited Campfire Radius");
        GameManager.Instance.isCampfireActive = false;
        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {
        Debug.Log("Interacted with Campfire");
    }
}
