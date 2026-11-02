using UnityEngine;

public class StickInteract : MonoBehaviour, IInteractable
{
    [SerializeField] Animator UIAnimator;

    public GameObject OnEnterRadius(Collider other)
    {
        Debug.Log("Entered Stick Radius");
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        Debug.Log("exited Stick Radius");
        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {
        if (GameManager.Instance.ActiveCharacter.type == CHARACTER.HUMAN)
        {
            ScoreManager.Instance.AddSticks();
            Debug.Log("Collected the stick!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You must be a Human to collect this");
            UIAnimator.SetTrigger("Reject"); //Animation if trying to interact as elephant
        }
    }
}
