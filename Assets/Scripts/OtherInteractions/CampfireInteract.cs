using System;
using Unity.VisualScripting;
using UnityEngine;

public class CampfireInteract : MonoBehaviour, IInteractable
{
    [SerializeField] Animator UIAnimator;
    [SerializeField] Material litMaterial;
    [SerializeField] Material unlitMaterial;
    [SerializeField] private bool isLit;
    [SerializeField] private int sticksToLight = 0;
    [SerializeField] private GameObject particle;
    [SerializeField] private AudioSource audio;

    private void Start()
    {
        GetComponent<Renderer>().material = unlitMaterial;
        if(isLit)
        {
            GetComponent<Renderer>().material = litMaterial;
            particle.SetActive(true);
            audio.Play();
        }
        //isLit = false;
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
            particle.SetActive(true);
            audio.Play();
            UIAnimator.SetTrigger("Accept"); //Makes the UI Disappear with the animation (sets scale to 0)
            isLit = true;

            if(interacter.TryGetComponent<CharacterCore>(out CharacterCore player))
            {
                player.InLitCampfireRadius = true;
            }
        }

        //Play UI animation if trying to interact as elephant
        if (!isLit && GameManager.Instance.ActiveCharacter.type == CHARACTER.ELEPHANT)
        {
            UIAnimator.SetTrigger("Reject");
        }
    }
}
