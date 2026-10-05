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

    private Dictionary<CHARACTER, CharacterStats> CharacterStat;

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
        CharacterStat = new();
        characterTemp = new();
        characterHunger = new();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        CharacterStat[CHARACTER.ELEPHANT] = Elephant.GetComponent<CharacterStats>();
        CharacterStat[CHARACTER.HUMAN] = Human.GetComponent<CharacterStats>();

        characterTemp[CHARACTER.HUMAN] = CharacterStat[CHARACTER.HUMAN].MaxTemperature;
        characterHunger[CHARACTER.HUMAN] = CharacterStat[CHARACTER.HUMAN].MaxHunger;
        characterTemp[CHARACTER.ELEPHANT] = CharacterStat[CHARACTER.ELEPHANT].MaxTemperature;
        characterHunger[CHARACTER.ELEPHANT] = CharacterStat[CHARACTER.ELEPHANT].MaxHunger;

        ActiveCharacter.type = CHARACTER.HUMAN;
        ActiveCharacter.gameObject = Human;

        camera.SetTarget(ActiveCharacter);

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
    }

    public void OnSwitch()
    {
        if (ActiveCharacter.type == CHARACTER.HUMAN)
        {
            ActiveCharacter.type = CHARACTER.ELEPHANT;
            ActiveCharacter.gameObject = Elephant;
        }
        else
        {
            ActiveCharacter.type = CHARACTER.HUMAN;
            ActiveCharacter.gameObject = Human;
        }

        InputActionsManager.Instance.SwitchCharacter(ActiveCharacter);
        camera.SetTarget(ActiveCharacter);

        temperatureSlider.maxValue = CharacterStat[ActiveCharacter.type].MaxTemperature;
        hungerSlider.maxValue = CharacterStat[ActiveCharacter.type].MaxHunger;
    }

    public void RestoreHunger(float pointsToRestore)
    {

        characterHunger[ActiveCharacter.type] = Mathf.Min(characterHunger[ActiveCharacter.type] + pointsToRestore, CharacterStat[ActiveCharacter.type].MaxHunger);
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
                if(characterTemp[CHARACTER.HUMAN] <= CharacterStat[CHARACTER.HUMAN].MaxTemperature)
                {
                    characterTemp[CHARACTER.HUMAN] += 2;
                }

                if(characterTemp[CHARACTER.ELEPHANT] <= CharacterStat[CHARACTER.ELEPHANT].MaxTemperature)
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
