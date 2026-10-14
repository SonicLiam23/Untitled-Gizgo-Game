using UnityEngine;

public class PlantInteract : MonoBehaviour, IInteractable
{
    public GameObject OnEnterRadius(Collider other)
    {
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {
        if (GameManager.Instance.ActiveCharacter.type == CHARACTER.ELEPHANT)
        {
            GameManager.Instance.RestoreHunger(25f);
            ScoreManager.Instance.AddPlant();
            Debug.Log("Elephant ate the plant!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You must be an elephant to eat this");
        }
    }
}
