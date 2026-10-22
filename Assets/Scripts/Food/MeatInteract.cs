using UnityEngine;

public class MeatInteract : MonoBehaviour, IInteractable
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
        if (GameManager.Instance.ActiveCharacter.type == CHARACTER.HUMAN)
        {
            GameManager.Instance.characterCore[CHARACTER.HUMAN].RestoreHunger(FoodRestored);
            ScoreManager.Instance.AddMeat();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You must be a human to eat this");
        }
    }
}
