using System.Collections.Generic;
using UnityEngine;

public class CharacterCore : MonoBehaviour
{
    public List<IInteractable> currentInteractables;


    private void Awake()
    {
        currentInteractables = new();
    }
}
