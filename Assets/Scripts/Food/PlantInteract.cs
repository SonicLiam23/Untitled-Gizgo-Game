using UnityEngine;

public class PlantInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private float FoodRestored = 10f;
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
            GameManager.Instance.characterCore[CHARACTER.ELEPHANT].RestoreHunger(FoodRestored);
            ScoreManager.Instance.AddPlant();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You must be an elephant to eat this");
        }
    }
}
