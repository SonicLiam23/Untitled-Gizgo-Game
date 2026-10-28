using System.Collections.Generic;
using Unity.VisualScripting;
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


    [SerializeField] private float hungerValue;
    [SerializeField] private float tempValue;
    [SerializeField] private float maxHunger;
    [SerializeField] private float maxTemp;

    public float HungerValue => hungerValue;
    public float TempValue => tempValue;
    public float MaxTemp => maxTemp;
    public float MaxHunger => maxHunger;


    private void Awake()
    {
        currentInteractables = new();
        AgentController = GetComponent<AgentController>();
        Controls = GetComponent<CharacterControls>();
        OverHeadUI = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        hungerValue = maxHunger;
        tempValue = maxTemp;
    }

    public void SetAIEnabled(bool enabled)
    {
        isAIEnabled = enabled;
        AgentController.enabled = enabled;
    }

    public void RestoreHunger(float pointsToRestore)
    {
        hungerValue += pointsToRestore;
        hungerValue = Mathf.Clamp(HungerValue, 0f, MaxHunger);
    }

    public void RemoveHunger(float pointsToRemove)
    {
        RestoreHunger(-pointsToRemove); 
    }


    public void RestoreTemp(float pointsToRestore)
    {
        tempValue += pointsToRestore;
        tempValue = Mathf.Clamp(TempValue, 0f, MaxTemp);
    }

    public void RemoveTemp(float pointsToRemove)
    {
        RestoreTemp(-pointsToRemove);
    }
}


