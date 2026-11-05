using UnityEngine;

public class MeatInteract : MonoBehaviour, IInteractable
{
    [SerializeField] Animator UIAnimator;
    [SerializeField] AudioClip sound; //Sound when picked up
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
            AudioManager.instance.PlaySound(sound);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You must be a human to eat this");
            UIAnimator.SetTrigger("Reject"); //Animation if trying to interact as elephant
        }
    }
}
