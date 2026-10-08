using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CHARACTER
{
    HUMAN = 0,
    ELEPHANT = 1,
    NONE = -1
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    [Header("MUST BE HUMAN AT INDEX 0 THEN ELEPHANT AT INDEX 1")]
    public GameObject[] CharacterObject;
    public CurrentCharacter ActiveCharacter { get; private set; } = new();
    public CurrentCharacter OtherCharacter { get; private set; } = new();
    private Dictionary<CHARACTER, CharacterCore> characterCore;

    private Dictionary<CHARACTER, CharacterStats> characterStat;

    public Slider temperatureSlider;
    public Slider hungerSlider;
    private Dictionary<CHARACTER, float> characterTemp;
    private Dictionary<CHARACTER, float> characterHunger;

    public GameObject Elephant => CharacterObject[(int)CHARACTER.ELEPHANT];
    public GameObject Human => CharacterObject[(int)CHARACTER.HUMAN];

    public float HumanElephantDistance { get; private set; }

    public bool IsCampfireLit;
    public bool IsCampfireActive;
 
    private bool isFollowActive = true;


    [SerializeField] private CameraAdjust camera;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        characterStat = new();
        characterTemp = new();
        characterHunger = new();
        characterCore = new();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        characterStat[CHARACTER.ELEPHANT] = Elephant.GetComponent<CharacterStats>();
        characterStat[CHARACTER.HUMAN] = Human.GetComponent<CharacterStats>();

        characterTemp[CHARACTER.HUMAN] = characterStat[CHARACTER.HUMAN].MaxTemperature;
        characterHunger[CHARACTER.HUMAN] = characterStat[CHARACTER.HUMAN].MaxHunger;
        characterTemp[CHARACTER.ELEPHANT] = characterStat[CHARACTER.ELEPHANT].MaxTemperature;
        characterHunger[CHARACTER.ELEPHANT] = characterStat[CHARACTER.ELEPHANT].MaxHunger;

        characterCore[CHARACTER.HUMAN] = Human.GetComponent<CharacterCore>();
        characterCore[CHARACTER.ELEPHANT] = Elephant.GetComponent<CharacterCore>();

        ActiveCharacter.type = CHARACTER.HUMAN;
        ActiveCharacter.gameObject = Human;
        characterCore[CHARACTER.HUMAN].AgentController.enabled = false;

        OtherCharacter.type = CHARACTER.ELEPHANT;
        OtherCharacter.gameObject = Elephant;
        characterCore[CHARACTER.ELEPHANT].AgentController.enabled = true;


        camera.SetTarget(ActiveCharacter);

        temperatureSlider.maxValue = characterStat[ActiveCharacter.type].MaxTemperature;
        hungerSlider.maxValue = characterStat[ActiveCharacter.type].MaxHunger;

        StartCoroutine(HungerCoroutine());
        StartCoroutine(TempCoroutine());
    }

    private void Update()
    {
        HumanElephantDistance = Vector3.Distance(Human.transform.position, Elephant.transform.position);
    }

    private void FixedUpdate()
    {

        temperatureSlider.value = characterTemp[ActiveCharacter.type];
        hungerSlider.value = characterHunger[ActiveCharacter.type];
        characterCore[OtherCharacter.type].AgentController?.SetTarget(ActiveCharacter.gameObject);
    }

    public void OnSwitch()
    {
        // Swaps them
        (ActiveCharacter, OtherCharacter) = (OtherCharacter, ActiveCharacter);

        Debug.Log(isFollowActive);
        // disable the agent for the character we are controlling, and enable the one for the one we are not (unless follow has been disabled)
        if (isFollowActive) characterCore[OtherCharacter.type].AgentController.enabled = true;
        else characterCore[OtherCharacter.type].AgentController.enabled = false;

        characterCore[ActiveCharacter.type].AgentController.enabled = false;

        // temp whilst i fix the "switching pushes the character down a bit" bug (its to do with the navmesh agent)
        TEMP_fixNavmeshMovement();

        InputActionsManager.Instance.SwitchCharacter(ActiveCharacter);
        camera.SetTarget(ActiveCharacter);
        characterCore[OtherCharacter.type].AgentController.SetTarget(ActiveCharacter.gameObject);

        temperatureSlider.maxValue = characterStat[ActiveCharacter.type].MaxTemperature;
        hungerSlider.maxValue = characterStat[ActiveCharacter.type].MaxHunger;
    }

    public void RestoreHunger(float pointsToRestore)
    {

        characterHunger[ActiveCharacter.type] = Mathf.Min(characterHunger[ActiveCharacter.type] + pointsToRestore, characterStat[ActiveCharacter.type].MaxHunger);
    }

    IEnumerator HungerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            characterHunger[ActiveCharacter.type] = Mathf.Max(characterHunger[ActiveCharacter.type] - 1f, 0f);
        }
    }

    public void ToggleFollow()
    {
        isFollowActive = !isFollowActive;
        // only set it for the other character
        characterCore[OtherCharacter.type].SetAIEnabled(isFollowActive);
    }

    IEnumerator TempCoroutine()
    {
        while (true)
        {
            float waitingTime = 3f;

            if (ActiveCharacter.type == CHARACTER.HUMAN && HumanElephantDistance <= 5f)
            {
                waitingTime = 5f;
            }

            if(IsCampfireActive)
            {
                waitingTime = 2f;
            }

            yield return new WaitForSeconds(waitingTime);

            Debug.Log($"{IsCampfireActive}");
            if(IsCampfireActive && IsCampfireLit)
            {
                if(characterTemp[ActiveCharacter.type] <= characterStat[ActiveCharacter.type].MaxTemperature)
                {
                    characterTemp[ActiveCharacter.type] += 2;
                }

            }
            else
            {
                characterTemp[ActiveCharacter.type] = Mathf.Max(characterTemp[ActiveCharacter.type] - 1f, 0f);
            }
        }
    }



    // TEMPORARY
    // Enabling/disabling the navmesh agent pushes the object down, until i find out why or a fix, this function just corrects the position by a bit
    public void TEMP_fixNavmeshMovement()
    {
        Vector3 adjustment = new Vector3(0f, 0.2f, 0f);
        Elephant.transform.position += adjustment;
        Human.transform.position += adjustment;
    }
}
