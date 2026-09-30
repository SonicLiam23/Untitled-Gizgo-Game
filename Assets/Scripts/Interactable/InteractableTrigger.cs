using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableTrigger : MonoBehaviour
{
    private IInteractable interactable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = GetComponent<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterCore>(out CharacterCore player))

        interactable?.OnEnterRadius();
    }

    private void OnTriggerExit(Collider other)
    {
        interactable?.OnExitRadius();
    }
}
