using UnityEngine;

public class InputActionsManager : MonoBehaviour
{
    public static InputActionsManager Instance { get; private set; } = null;

    [SerializeField] private CameraAdjust camera;
    public GameObject Elephant;
    public GameObject Human;
    private CharacterController elephantController;
    private CharacterController humanController;
    public InputSystem_Actions InputActions { get; private set; }
    private bool isElephantActive = false;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InputActions = new InputSystem_Actions();
        InputActions.Player.Enable();
    }

    private void Start()
    {
        elephantController = Elephant.GetComponent<CharacterController>();
        humanController = Human.GetComponent<CharacterController>();
        InputActions.Player.SetCallbacks(humanController);
        camera.SetTarget(Human, TARGET_TYPE.HUMAN);
    }

    public void SwitchCharacter()
    {
        isElephantActive = !isElephantActive;
        if (isElephantActive)
        {
            InputActions.Player.SetCallbacks(elephantController);
            camera.SetTarget(Elephant, TARGET_TYPE.HUMAN);
        }
        else
        {
            InputActions.Player.SetCallbacks(humanController);
            camera.SetTarget(Human, TARGET_TYPE.HUMAN);
        }
    }
}
