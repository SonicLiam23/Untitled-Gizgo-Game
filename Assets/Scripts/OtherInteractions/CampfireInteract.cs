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


        if(ScoreManager.Instance.GetSticks() >= 3 && !GameManager.Instance.isCampfireLit)
        {
            ScoreManager.Instance.RemoveSticks(3);
            GameManager.Instance.isCampfireLit = true;
        }
        else
        {
            Debug.Log("Not enough sticks in inventory to light up the campfire.");
        }
    }
}
