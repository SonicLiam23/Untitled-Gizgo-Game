using System;
using Unity.VisualScripting;
using UnityEngine;

public class CampfireInteract : MonoBehaviour, IInteractable
{

    [SerializeField] Material litMaterial;
    [SerializeField] Material unlitMaterial;
    bool isLit;
    [SerializeField] private int sticksToLight = 0;

    private void Start()
    {
        GetComponent<Renderer>().material = unlitMaterial;
        isLit = false;
    }
    public GameObject OnEnterRadius(Collider other)
    {
        if (isLit && other.TryGetComponent<CharacterCore>(out CharacterCore player))
        {
            player.InLitCampfireRadius = true;
        }

        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        if (isLit && other.TryGetComponent<CharacterCore>(out CharacterCore player))
        {
            player.InLitCampfireRadius = false;
        }

        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {

        if(!isLit && ScoreManager.Instance.GetSticks() >= sticksToLight && GameManager.Instance.ActiveCharacter.type == CHARACTER.HUMAN)
        {
            ScoreManager.Instance.RemoveSticks(sticksToLight);
            GetComponent<Renderer>().material = litMaterial;
            isLit = true;
        }
    }
}
