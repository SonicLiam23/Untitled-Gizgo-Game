using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableTrigger : MonoBehaviour
{
    private IInteractable interactable;
    List<CharacterCore> currentCharactersColliding;

    [SerializeField] Canvas interactPopUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCharactersColliding = new();
        interactable = GetComponent<IInteractable>();
        if (interactPopUp)
            interactPopUp.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactable != null)
        {
            
            if (other.TryGetComponent<CharacterCore>(out CharacterCore player))
            {
                currentCharactersColliding.Add(player);
                player.currentInteractables.Add(interactable);
                interactable.OnEnterRadius(other);
                if (interactPopUp)
                    interactPopUp.enabled = true;
            }

            
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
                interactable.OnExitRadius(other);
                if (interactPopUp)
                    interactPopUp.enabled = false;
            }

            
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

        if (interactPopUp)
        {
            Destroy(interactPopUp.gameObject);
            interactPopUp = null;
        }
    }
}
