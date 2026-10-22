using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CHARACTER
{
    HUMAN = 0,
    ELEPHANT = 1,
    NONE = -1
}

public class GameManager : MonoBehaviour
{
    // PUBLIC/API
    public static GameManager Instance { get; private set; } = null;
    public CurrentCharacter ActiveCharacter { get; private set; } = new();
    public CurrentCharacter OtherCharacter { get; private set; } = new();
    public GameObject Elephant { get; private set; }
    public GameObject Human { get; private set; }
    public bool CanSwitchOverride = true;
    public float HumanElephantDistance { get; private set; }

    // PUBLIC END

    [Header("MUST BE HUMAN AT INDEX 0 THEN ELEPHANT AT INDEX 1")]
    [SerializeField] private GameObject[] CharacterObject;
    [SerializeField] private Slider[] HoveringFoodSlider;
    [SerializeField] private Slider[] HoveringTempSlider;
    private Dictionary<CHARACTER, CharacterCore> characterCore;
    private Dictionary<CHARACTER, CharacterStats> characterStat;
    [SerializeField] private Slider temperatureSlider;
    [SerializeField] private Slider hungerSlider;
    private Dictionary<CHARACTER, float> characterTemp;
    private Dictionary<CHARACTER, float> characterHunger;
    private bool isFollowActive = true;
    private bool isACharacterFrozen = false;
    private float tempToReduce;
    private float hungerToReduce;
    private float speedChange;
    [SerializeField] private float CampfireTempIncrease;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private CameraAdjust camera;
    [SerializeField] private TimerScirpt dayNightTimer;
    private IEnumerator hungerCoroutine;
    private IEnumerator tempCoroutine;
    private bool inMinigame;

    


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
        Human = CharacterObject[(int)CHARACTER.HUMAN];
        Elephant = CharacterObject[(int)CHARACTER.ELEPHANT];

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
        HoveringFoodSlider[(int)CHARACTER.HUMAN].maxValue = characterStat[CHARACTER.HUMAN].MaxHunger;
        HoveringTempSlider[(int)CHARACTER.HUMAN].maxValue = characterStat[CHARACTER.HUMAN].MaxTemperature;
        HoveringFoodSlider[(int)CHARACTER.ELEPHANT].maxValue = characterStat[CHARACTER.ELEPHANT].MaxHunger;
        HoveringTempSlider[(int)CHARACTER.ELEPHANT].maxValue = characterStat[CHARACTER.ELEPHANT].MaxTemperature;
        characterCore[OtherCharacter.type].OverHeadUI.enabled = true;
        characterCore[ActiveCharacter.type].OverHeadUI.enabled = false;


        InputActionsManager.Init();

        hungerCoroutine = HungerCoroutine();
        tempCoroutine = TempCoroutine();

        inMinigame = false;

        StartCoroutine(hungerCoroutine);
        StartCoroutine(tempCoroutine);

    }

    private void Update()
    {
        HumanElephantDistance = Vector3.Distance(Human.transform.position, Elephant.transform.position);
    }

    private void FixedUpdate()
    {

        temperatureSlider.value = characterTemp[ActiveCharacter.type];
        hungerSlider.value = characterHunger[ActiveCharacter.type];

        HoveringFoodSlider[(int)OtherCharacter.type].value = characterHunger[OtherCharacter.type];
        HoveringTempSlider[(int)OtherCharacter.type].value = characterTemp[OtherCharacter.type];

        characterCore[OtherCharacter.type].AgentController?.SetTarget(ActiveCharacter.gameObject);

        //Checks current tile the player is on and changes the rate at which their temp and hunger are reduced based on the tile
        BiomeTile tile = mapManager?.GetBiomeTileAtPosition(ActiveCharacter.gameObject.transform.position);

        if(tile != null)
        {
            tempToReduce = tile.TempRate;
            hungerToReduce = tile.HungerRate;
            characterCore[ActiveCharacter.type].Controls.ChangeInMovementSpeed = tile.PlayerSpeedChange; 
        }


    }

    public void SwitchCharacter()
    {
        if (!CanSwitch()) return;
        ForceSwitch();

    }

    private void ForceSwitch()
    {
        // Swaps them
        (ActiveCharacter, OtherCharacter) = (OtherCharacter, ActiveCharacter);

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

        characterCore[OtherCharacter.type].OverHeadUI.enabled = true;
        characterCore[ActiveCharacter.type].OverHeadUI.enabled = false;
        
    }

    public void EnterMinigame()
    {
        if (inMinigame) return;

        inMinigame = true;
        StopCoroutine(hungerCoroutine);
        StopCoroutine(tempCoroutine);
        // disables the UI, jank asf i actually really hate this, but it works.
        temperatureSlider.transform.parent.gameObject.SetActive(false);
        HoveringFoodSlider[0].transform.parent.gameObject.SetActive(false);
        HoveringFoodSlider[1].transform.parent.gameObject.SetActive(false);
        dayNightTimer.SetTimePasses(false);

    }

    public void ExitMinigame()
    {
        if (!inMinigame) return;
        
        inMinigame = false;
        StartCoroutine(hungerCoroutine);
        StartCoroutine(tempCoroutine);
        // disables the UI, jank asf i actually really hate this, but it works.
        temperatureSlider.transform.parent.gameObject.SetActive(true); 
        HoveringFoodSlider[0].transform.parent.gameObject.SetActive(true);
        HoveringFoodSlider[1].transform.parent.gameObject.SetActive(true);
        dayNightTimer.SetTimePasses(true);
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

            characterHunger[CHARACTER.HUMAN] = Mathf.Max(characterHunger[CHARACTER.HUMAN] - 1f, 0f);
            characterHunger[CHARACTER.ELEPHANT] = Mathf.Max(characterHunger[CHARACTER.ELEPHANT] - 1f, 0f);
        }
    }

    public void ToggleFollow()
    {
        isFollowActive = !isFollowActive;
        // only set it for the other character
        characterCore[OtherCharacter.type].SetAIEnabled(isFollowActive);
    }

    public bool CanSwitch()
    {
        return CanSwitchOverride && !(characterTemp[CHARACTER.ELEPHANT] <= 0f || characterTemp[CHARACTER.HUMAN] <= 0f /*|| characterHunger[CHARACTER.ELEPHANT] <= 0f || characterHunger[CHARACTER.HUMAN] <= 0f*/);
    }

    IEnumerator TempCoroutine()
    {
        float waitingTime = 3f;

        while (true)
        {
            yield return new WaitForSeconds(waitingTime);
            float tempRate = tempToReduce + DayCycleStateMachine.Instance.CurrentState.TempRate;

            if (characterCore[CHARACTER.HUMAN].InLitCampfireRadius)
            {
                characterTemp[CHARACTER.HUMAN] += CampfireTempIncrease;
            }
            else
            {
                characterTemp[CHARACTER.HUMAN] -= tempRate;
            }
            characterTemp[CHARACTER.HUMAN] = Mathf.Clamp(characterTemp[CHARACTER.HUMAN], 0f, characterStat[CHARACTER.HUMAN].MaxTemperature);


            if (characterCore[CHARACTER.ELEPHANT].InLitCampfireRadius)
            {
                characterTemp[CHARACTER.ELEPHANT] += CampfireTempIncrease;
            }
            else
            {
                characterTemp[CHARACTER.ELEPHANT] -= tempRate;
            }
            characterTemp[CHARACTER.ELEPHANT] = Mathf.Clamp(characterTemp[CHARACTER.ELEPHANT], 0f, characterStat[CHARACTER.ELEPHANT].MaxTemperature);

            // the jankiest, most horrible solution ever. But... it works...
            if (ActiveCharacter.type == CHARACTER.HUMAN && HumanElephantDistance <= 5f)
            {
                characterTemp[CHARACTER.HUMAN] = Mathf.Min(characterTemp[CHARACTER.HUMAN] + 0.5f, characterStat[CHARACTER.HUMAN].MaxTemperature);
            }


            if (isACharacterFrozen && (characterTemp[CHARACTER.HUMAN] <= 0f || characterTemp[CHARACTER.ELEPHANT] <= 0f))
            {
                CanSwitchOverride = false;
                isACharacterFrozen = true;
                ForceSwitch();
            }

            if (!isACharacterFrozen)
            {
                if (characterTemp[OtherCharacter.type] > 0f)
                {
                    CanSwitchOverride = true;
                    isACharacterFrozen = false;
                }
            }

            if (characterTemp[CHARACTER.HUMAN] <= 0f && characterTemp[CHARACTER.ELEPHANT] <= 0f)
            {
                SceneManager.LoadScene("MainMenu");
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
