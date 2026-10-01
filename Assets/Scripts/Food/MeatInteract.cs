using UnityEngine;

public class MeatInteract : MonoBehaviour, IInteractable
{
    public GameObject OnEnterRadius(Collider other)
    {
        Debug.Log("Entered Meat Radius");
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        Debug.Log("exited Meat Radius");
        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {
        if (GameManager.Instance.ActiveCharacter.type == CHARACTER.HUMAN)
        {
            GameManager.Instance.RestoreHunger(5f);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You must be a human to eat this");
        }
    }
}
