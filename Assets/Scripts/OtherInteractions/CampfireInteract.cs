using System;
using Unity.VisualScripting;
using UnityEngine;

public class CampfireInteract : MonoBehaviour, IInteractable
{

    [SerializeField] Material litMaterial;
    [SerializeField] Material unlitMaterial;
    bool islit;
    [SerializeField] private int sticksToLight = 0;

    private void Start()
    {
        GetComponent<Renderer>().material = unlitMaterial;
        islit = false;
    }
    public GameObject OnEnterRadius(Collider other)
    {
        GameManager.Instance.IsCampfireLit = islit;
        GameManager.Instance.IsCampfireActive = true;
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        GameManager.Instance.IsCampfireLit = false;
        GameManager.Instance.IsCampfireActive = false;
        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {
        Debug.Log("Interacted with Campfire");


        if(ScoreManager.Instance.GetSticks() >= sticksToLight && !GameManager.Instance.IsCampfireLit)
        {
            ScoreManager.Instance.RemoveSticks(sticksToLight);
            GameManager.Instance.IsCampfireLit = true;
            GetComponent<Renderer>().material = litMaterial;
            islit = true;
        }
        else
        {
            Debug.Log("Not enough sticks in inventory to light up the campfire.");
        }
    }
}
