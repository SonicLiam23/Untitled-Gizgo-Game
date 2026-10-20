using System.Collections.Generic;
using UnityEngine;

public class CharacterCore : MonoBehaviour
{
    public List<IInteractable> currentInteractables;
    // agentController should be safe to be null, we may want the core but no agent attached.
    public AgentController AgentController { get; private set; }
    public CharacterControls Controls { get; private set; }
    public bool isAIEnabled { get; private set; } = true;

    public Canvas OverHeadUI { get; private set; }

    public bool InLitCampfireRadius = false;
    public bool HeatLossGainEnabled = true;

    public bool TempLossEnabled = true;


    private void Awake()
    {
        currentInteractables = new();
        AgentController = GetComponent<AgentController>();
        Controls = GetComponent<CharacterControls>();
        OverHeadUI = GetComponentInChildren<Canvas>();
    }

    public void SetAIEnabled(bool enabled)
    {
        isAIEnabled = enabled;
        AgentController.enabled = enabled;
    }
}


