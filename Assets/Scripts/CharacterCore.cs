using System.Collections.Generic;
using UnityEngine;

public class CharacterCore : MonoBehaviour
{
    public List<IInteractable> currentInteractables;
    public AgentController agentController { get; private set; }


    private void Awake()
    {
        currentInteractables = new();
        agentController = GetComponent<AgentController>();
    }
}


