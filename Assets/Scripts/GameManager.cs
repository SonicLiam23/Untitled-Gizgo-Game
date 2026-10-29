using System.Collections;
using System.Collections.Generic;
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
    public static GameManager Instance { get; private set; } = null;

    [Header("MUST BE HUMAN AT INDEX 0 THEN ELEPHANT AT INDEX 1")]
    public GameObject[] CharacterObject;
    public Slider[] HoveringFoodSlider;
    public Slider[] HoveringTempSlider;

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
 
    private bool isFollowActive = true;
    public bool CanSwitchOverride = true;
    private bool isACharacterFrozen = false;

    private float tempToReduce;
    private float hungerToReduce;

    private float speedChange;

    [SerializeField] private float CampfireTempIncrease;

    [SerializeField] private CameraAdjust camera;
    [SerializeField] private TimerScirpt dayNightTimer;

    private BiomeSwapper swapper;

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
        characterCore[OtherCharacter.type].SelectedArrowUI.enabled = false;
        characterCore[ActiveCharacter.type].OverHeadUI.enabled = false;
        characterCore[ActiveCharacter.type].SelectedArrowUI.enabled = true;

        swapper = GetComponent<BiomeSwapper>();
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

        BiomeConfig.BiomeEntry biome = swapper.CheckBiomeAtPos(ActiveCharacter.gameObject.transform.position);


        if (biome != null)
        {
            tempToReduce = biome.TempRate;
            hungerToReduce = biome.HungerRate;
            characterCore[ActiveCharacter.type].Controls.ChangeInMovementSpeed = biome.PlayerSpeedIncrease; 
        }
    }

    public void OnSwitch()
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
        characterCore[OtherCharacter.type].SelectedArrowUI.enabled = false;
        characterCore[ActiveCharacter.type].OverHeadUI.enabled = false;
        characterCore[ActiveCharacter.type].SelectedArrowUI.enabled = true;

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


            if (CanSwitchOverride && (characterTemp[CHARACTER.HUMAN] <= 0f || characterTemp[CHARACTER.ELEPHANT] <= 0f))
            {
                CanSwitchOverride = false;
                ForceSwitch();
            }

            if (!CanSwitchOverride)
            {
                if (characterTemp[OtherCharacter.type] > 0f)
                {
                    CanSwitchOverride = true;
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
