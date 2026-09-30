using System.Collections.Generic;
using UnityEngine;

public class CharacterCore : MonoBehaviour
{
    private List<IInteractable> currentInteractables;


    private void Awake()
    {
        currentInteractables = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            currentInteractables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            currentInteractables.Remove(interactable);
        }
    }
}
