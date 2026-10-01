using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableTrigger : MonoBehaviour
{
    private IInteractable interactable;
    List<CharacterCore> currentCharactersColliding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCharactersColliding = new();
        interactable = GetComponent<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactable != null)
        {
            
            if (other.TryGetComponent<CharacterCore>(out CharacterCore player))
            {
                currentCharactersColliding.Add(player);
                player.currentInteractables.Add(interactable);
            }

            interactable.OnEnterRadius(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactable != null)
        {
            if (other.TryGetComponent<CharacterCore>(out CharacterCore player))
            {
                currentCharactersColliding.Remove(player);
                player.currentInteractables.Remove(interactable);
            }

            interactable.OnExitRadius(other);
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            foreach (CharacterCore c in currentCharactersColliding)
            {
                c.currentInteractables.Remove(interactable);
            }
        }
    }
}
