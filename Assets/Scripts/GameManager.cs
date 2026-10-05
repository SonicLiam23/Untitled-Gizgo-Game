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

    private Dictionary<CHARACTER, CharacterStats> CharacterStat;

    public Slider temperatureSlider;
    public Slider hungerSlider;
    private Dictionary<CHARACTER, float> characterTemp;
    private Dictionary<CHARACTER, float> characterHunger;

    public GameObject Elephant => CharacterObject[(int)CHARACTER.ELEPHANT];
    public GameObject Human => CharacterObject[(int)CHARACTER.HUMAN];

    private float humanElephantDistance;

    public bool isCampfireLit;
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
        humanElephantDistance = Vector3.Distance(Human.transform.position, Elephant.transform.position);
    }

    private void FixedUpdate()
    {
        if (ActiveCharacter.type == CHARACTER.HUMAN)
        {
            temperatureSlider.maxValue = CharacterStat[CHARACTER.HUMAN].MaxTemperature;
            hungerSlider.maxValue = CharacterStat[CHARACTER.HUMAN].MaxHunger;

            temperatureSlider.value = characterTemp[CHARACTER.HUMAN];
            hungerSlider.value = characterHunger[CHARACTER.HUMAN];
        }

        else
        {
            temperatureSlider.maxValue = CharacterStat[CHARACTER.ELEPHANT].MaxTemperature;
            hungerSlider.maxValue = CharacterStat[CHARACTER.ELEPHANT].MaxHunger;

            temperatureSlider.value = characterTemp[CHARACTER.ELEPHANT];
            hungerSlider.value = characterHunger[CHARACTER.ELEPHANT];
        }
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

            if (ActiveCharacter.type == CHARACTER.HUMAN && humanElephantDistance <= 5f)
            {
                waitingTime = 5f;
            }

            if(isCampfireActive)
            {
                waitingTime = 2f;
            }

            yield return new WaitForSeconds(waitingTime);

            if(isCampfireActive && isCampfireLit)
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
