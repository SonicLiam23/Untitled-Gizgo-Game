using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool isCampfireActive;
  


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
        characterCore[CHARACTER.HUMAN].agentController.enabled = false;

        OtherCharacter.type = CHARACTER.ELEPHANT;
        OtherCharacter.gameObject = Elephant;
        characterCore[CHARACTER.ELEPHANT].agentController.enabled = true;


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
        characterCore[OtherCharacter.type].agentController?.SetTarget(ActiveCharacter.gameObject);
    }

    public void OnSwitch()
    {
        // Swaps them
        (ActiveCharacter, OtherCharacter) = (OtherCharacter, ActiveCharacter);

        // disable the agent for the character we are controlling
        characterCore[OtherCharacter.type].agentController.enabled = true;
        characterCore[ActiveCharacter.type].agentController.enabled = false;

        // temp whilst i fix the "switching pushes the character down a bit" bug (its to do with the navmesh agent)
        ActiveCharacter.gameObject.transform.position += new Vector3(0f, 0.2f, 0f);

        InputActionsManager.Instance.SwitchCharacter(ActiveCharacter);
        camera.SetTarget(ActiveCharacter);
        characterCore[OtherCharacter.type].agentController.SetTarget(ActiveCharacter.gameObject);

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
            if (ActiveCharacter.type == CHARACTER.HUMAN)
            {
                --characterHunger[CHARACTER.HUMAN];
            }
            else
            {
                --characterHunger[CHARACTER.ELEPHANT];
            }
        }
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

            if(isCampfireActive)
            {
                waitingTime = 2f;
            }

            yield return new WaitForSeconds(waitingTime);

            if(isCampfireActive)
            {
                if(characterTemp[CHARACTER.HUMAN] <= characterStat[CHARACTER.HUMAN].MaxTemperature)
                {
                    characterTemp[CHARACTER.HUMAN] += 2;
                }

                if(characterTemp[CHARACTER.ELEPHANT] <= characterStat[CHARACTER.ELEPHANT].MaxTemperature)
                {
                    characterTemp[CHARACTER.ELEPHANT] += 2;
                }

            }
            else if (ActiveCharacter.type == CHARACTER.HUMAN)
            {
                --characterTemp[CHARACTER.HUMAN];
            }
            else
            {
                --characterTemp[CHARACTER.ELEPHANT];
            }
        }
    }
}
