using UnityEngine;

public class CampfireInteract : MonoBehaviour, IInteractable
{
    public GameObject OnEnterRadius(Collider other)
    {
        GameManager.Instance.IsCampfireActive = true;
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        GameManager.Instance.IsCampfireActive = false;
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
