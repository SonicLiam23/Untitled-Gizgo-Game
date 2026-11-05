using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.VolumeComponent;

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
    public CurrentCharacter ActiveCharacter { get; private set; }
    public CurrentCharacter OtherCharacter { get; private set; }
    public GameObject Elephant { get; private set; }
    public GameObject Human { get; private set; }
    public bool CanSwitchOverride = true;
    public float HumanElephantDistance { get; private set; }
    public Dictionary<CHARACTER, CharacterCore> characterCore { get; private set; }

    // PUBLIC END

    [Header("MUST BE HUMAN AT INDEX 0 THEN ELEPHANT AT INDEX 1")]
    [SerializeField] private GameObject[] CharacterObject;
    [SerializeField] private Slider[] HoveringFoodSlider;
    [SerializeField] private Slider[] HoveringTempSlider;
    
    [SerializeField] private Slider temperatureSlider;
    [SerializeField] private Slider hungerSlider;
    private bool isFollowActive = true;
    private bool isACharacterFrozen = false;
    private float tempToReduce;
    private float hungerToReduce;
    private float speedChange;
    [SerializeField] private float CampfireTempIncrease;

    [SerializeField] private CameraAdjust camera;
    [SerializeField] private TimerScirpt dayNightTimer;
    [SerializeField] private MapManager mapManager;

    [SerializeField] private Image FreezingImg;
    [SerializeField] private CanvasGroup DeathImgGroup;
    [SerializeField] private GameObject humanControlUI;
    [SerializeField] private GameObject elephantControlUI;



    private BiomeSwapper swapper;

    private IEnumerator hungerCoroutine;
    private IEnumerator tempCoroutine;
    private bool inMinigame;
    private bool inDeath;

    


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        characterCore = new();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Human = CharacterObject[(int)CHARACTER.HUMAN];
        Elephant = CharacterObject[(int)CHARACTER.ELEPHANT];

        characterCore[CHARACTER.HUMAN] = Human.GetComponent<CharacterCore>();
        characterCore[CHARACTER.ELEPHANT] = Elephant.GetComponent<CharacterCore>();
        ActiveCharacter = new CurrentCharacter(Human, CHARACTER.HUMAN);
        OtherCharacter = new CurrentCharacter(Elephant, CHARACTER.ELEPHANT);

        characterCore[CHARACTER.HUMAN].AgentController.enabled = false;
        characterCore[CHARACTER.ELEPHANT].AgentController.enabled = true;


        camera.SetTarget(ActiveCharacter);

        temperatureSlider.maxValue = characterCore[ActiveCharacter.type].MaxTemp;
        hungerSlider.maxValue = characterCore[ActiveCharacter.type].MaxHunger;
        HoveringFoodSlider[(int)CHARACTER.HUMAN].maxValue = characterCore[CHARACTER.HUMAN].MaxHunger;
        HoveringTempSlider[(int)CHARACTER.HUMAN].maxValue = characterCore[CHARACTER.HUMAN].MaxTemp;
        HoveringFoodSlider[(int)CHARACTER.ELEPHANT].maxValue = characterCore[CHARACTER.ELEPHANT].MaxHunger;
        HoveringTempSlider[(int)CHARACTER.ELEPHANT].maxValue = characterCore[CHARACTER.ELEPHANT].MaxTemp;
        characterCore[OtherCharacter.type].OverHeadUI.enabled = true;
        characterCore[OtherCharacter.type].SelectedArrowUI.enabled = false;
        characterCore[ActiveCharacter.type].OverHeadUI.enabled = false;
        characterCore[ActiveCharacter.type].SelectedArrowUI.enabled = true;

        swapper = GetComponent<BiomeSwapper>();
        InputActionsManager.Init();

        hungerCoroutine = HungerCoroutine();
        tempCoroutine = TempCoroutine();

        inMinigame = false;
        inDeath = false;
        
        FreezingImg.enabled = false;
        DeathScreenState(false);

        StartCoroutine(hungerCoroutine);
        StartCoroutine(tempCoroutine);

    }

    private void Update()
    {
        HumanElephantDistance = Vector3.Distance(Human.transform.position, Elephant.transform.position);
    }

    private void FixedUpdate()
    {

        temperatureSlider.value = characterCore[ActiveCharacter.type].TempValue;
        hungerSlider.value = characterCore[ActiveCharacter.type].HungerValue;

        HoveringFoodSlider[(int)OtherCharacter.type].value = characterCore[OtherCharacter.type].HungerValue;
        HoveringTempSlider[(int)OtherCharacter.type].value = characterCore[OtherCharacter.type].TempValue;

        //if current character temp is at 30% or below from the total temp
        if (characterCore[ActiveCharacter.type].TempValue <= characterCore[ActiveCharacter.type].MaxTemp * 30 / 100)
        {
            FreezingImg.enabled = true;
        }
        else
        {
            FreezingImg.enabled = false;
        }

        characterCore[OtherCharacter.type].AgentController?.SetTarget(ActiveCharacter.gameObject);

        BiomeConfig.BiomeEntry biome = swapper.CheckBiomeAtPos(ActiveCharacter.gameObject.transform.position);


        if (biome != null)
        {
            tempToReduce = biome.TempRate;
            hungerToReduce = biome.HungerRate;
            characterCore[ActiveCharacter.type].Controls.ChangeInMovementSpeed = biome.PlayerSpeedIncrease;

        }


        if (characterCore[CHARACTER.ELEPHANT].TempValue <= 0 && characterCore[CHARACTER.HUMAN].TempValue <= 0)
        {
            EnterDeathState();
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

        // adding this in to swap the control scheme UI based on the active character
        SwapControlUI();

        // temp whilst i fix the "switching pushes the character down a bit" bug (its to do with the navmesh agent)
        TEMP_fixNavmeshMovement();

        InputActionsManager.Instance.SwitchCharacter(ActiveCharacter);
        camera.SetTarget(ActiveCharacter);
        characterCore[OtherCharacter.type].AgentController.SetTarget(ActiveCharacter.gameObject);

        temperatureSlider.maxValue = characterCore[ActiveCharacter.type].MaxTemp;
        hungerSlider.maxValue = characterCore[ActiveCharacter.type].MaxHunger;

        characterCore[OtherCharacter.type].OverHeadUI.enabled = true;
        characterCore[OtherCharacter.type].SelectedArrowUI.enabled = false;
        characterCore[ActiveCharacter.type].OverHeadUI.enabled = false;
        characterCore[ActiveCharacter.type].SelectedArrowUI.enabled = true;

    }

    public void EnterDeathState() 
    {
        inDeath = true;
        DeathScreenState(true);
        StopCoroutine(hungerCoroutine);
        StopCoroutine(tempCoroutine);
        // disables the UI, jank asf i actually really hate this, but it works.
        temperatureSlider.transform.parent.gameObject.SetActive(false);
        HoveringFoodSlider[0].transform.parent.gameObject.SetActive(false);
        HoveringFoodSlider[1].transform.parent.gameObject.SetActive(false);
        dayNightTimer.SetTimePasses(false);

    }

    public void ExitDeathState()
    {
        inDeath = false;
        DeathScreenState(false);

        InputActionsManager.Instance.InputActions.Player.Disable();


        SceneManager.LoadScene("MainMenu");
        //return to main menu
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
    IEnumerator HungerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            characterCore[CHARACTER.HUMAN].RemoveHunger(1f);
            characterCore[CHARACTER.ELEPHANT].RemoveHunger(1f);
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
        return CanSwitchOverride && !(characterCore[CHARACTER.ELEPHANT].TempValue <= 0f || characterCore[CHARACTER.HUMAN].TempValue <= 0f);
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
                characterCore[CHARACTER.HUMAN].RestoreTemp(CampfireTempIncrease);
            }
            else
            {
                characterCore[CHARACTER.HUMAN].RemoveTemp(tempRate);
            }


            if (characterCore[CHARACTER.ELEPHANT].InLitCampfireRadius)
            {
                characterCore[CHARACTER.ELEPHANT].RestoreTemp(CampfireTempIncrease);
            }
            else
            {
                characterCore[CHARACTER.ELEPHANT].RemoveTemp(tempRate);
            }

            // the jankiest, most horrible solution ever. But... it works...
            if (ActiveCharacter.type == CHARACTER.HUMAN && HumanElephantDistance <= 5f)
            {
                characterCore[CHARACTER.HUMAN].RestoreTemp(0.5f);
            }


            if (isACharacterFrozen && (characterCore[CHARACTER.HUMAN].TempValue <= 0f || characterCore[CHARACTER.ELEPHANT].TempValue <= 0f))
            {
                CanSwitchOverride = false;
                isACharacterFrozen = true;
                ForceSwitch();
            }

            if (!isACharacterFrozen)
            {
                if (characterCore[OtherCharacter.type].TempValue > 0f)
                {
                    CanSwitchOverride = true;
                    isACharacterFrozen = false;
                }
            }

            if (characterCore[CHARACTER.HUMAN].TempValue <= 0f && characterCore[CHARACTER.ELEPHANT].TempValue <= 0f)
            {
                Debug.Log("YOU DIED, RETURNING TO MENU");
                yield return new WaitForSeconds(2);

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

    // Handles swapping the UI that shows the different controls
    private void SwapControlUI()
    {
        if(humanControlUI != null && elephantControlUI !=null)
        {
            CanvasGroup hGroup = humanControlUI.GetComponent<CanvasGroup>();
            CanvasGroup eGroup = elephantControlUI.GetComponent<CanvasGroup>();

            if (ActiveCharacter.type == CHARACTER.HUMAN)
            {
                hGroup.alpha = 1f;
                eGroup.alpha = 0f;
            }
            else
            {
                hGroup.alpha = 0f;
                eGroup.alpha = 1f;
            }
        }
    }

    // Handles showing/hiding the death screen
    private void DeathScreenState(bool active)
    {

        DeathImgGroup.alpha = active ? 1f : 0f;
        DeathImgGroup.blocksRaycasts = active;
        DeathImgGroup.interactable = active;
    }
}
