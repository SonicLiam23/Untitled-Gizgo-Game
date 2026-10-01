using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public UnityEngine.UI.Slider temperatureSlider;
    public UnityEngine.UI.Slider hungerSlider;
    private Dictionary<CHARACTER, float> characterTemp;
    private Dictionary<CHARACTER, float> characterHunger;

    public GameObject Elephant => CharacterObject[(int)CHARACTER.ELEPHANT];
    public GameObject Human => CharacterObject[(int)CHARACTER.HUMAN];

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

        StartCoroutine(TimerCoroutine());
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

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (ActiveCharacter.type == CHARACTER.HUMAN)
            {
                temperatureSlider.maxValue = CharacterStat[CHARACTER.HUMAN].MaxTemperature;
                hungerSlider.maxValue = CharacterStat[CHARACTER.HUMAN].MaxHunger;

                temperatureSlider.value = characterTemp[CHARACTER.HUMAN];
                hungerSlider.value = characterHunger[CHARACTER.HUMAN];

                --characterTemp[CHARACTER.HUMAN];
                --characterHunger[CHARACTER.HUMAN];
            }

            else
            {
                temperatureSlider.maxValue = CharacterStat[CHARACTER.ELEPHANT].MaxTemperature;
                hungerSlider.maxValue = CharacterStat[CHARACTER.ELEPHANT].MaxHunger;

                temperatureSlider.value = characterTemp[CHARACTER.ELEPHANT];
                hungerSlider.value = characterHunger[CHARACTER.ELEPHANT];

                --characterTemp[CHARACTER.ELEPHANT];
                --characterHunger[CHARACTER.ELEPHANT];
            }
        }
    }
}
