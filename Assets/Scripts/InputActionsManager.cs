using System.Collections.Generic;
using UnityEngine;

public class InputActionsManager : MonoBehaviour
{
    public static InputActionsManager Instance { get; private set; } = null;

    [SerializeField] private CameraAdjust camera;
    private Dictionary<CHARACTER, CharacterController> CharacterController;
    public InputSystem_Actions InputActions { get; private set; }
    private bool isElephantActive = false;
    public Vector3 MovementInput;

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
        CharacterController[CHARACTER.ELEPHANT] = GameManager.Instance.Elephant.GetComponent<CharacterController>();
        CharacterController[CHARACTER.HUMAN] = GameManager.Instance.Human.GetComponent<CharacterController>();
        CharacterController[CHARACTER.ELEPHANT].enabled = false;
        CharacterController[CHARACTER.HUMAN].enabled = true;
        InputActions.Player.SetCallbacks(CharacterController[CHARACTER.HUMAN]);
        camera.SetTarget(GameManager.Instance.Human, CHARACTER.HUMAN);
    }

    public void SwitchCharacter()
    {
        isElephantActive = !isElephantActive;
        if (isElephantActive)
        {
            CharacterController[CHARACTER.HUMAN].enabled = false;
            CharacterController[CHARACTER.ELEPHANT].enabled = true;

            InputActions.Player.SetCallbacks(CharacterController[CHARACTER.ELEPHANT]);
            camera.SetTarget(GameManager.Instance.Elephant, CHARACTER.ELEPHANT);
        }
        else
        {
            CharacterController[CHARACTER.HUMAN].enabled = true;
            CharacterController[CHARACTER.ELEPHANT].enabled = false;

            InputActions.Player.SetCallbacks(CharacterController[CHARACTER.HUMAN]);
            camera.SetTarget(GameManager.Instance.Human, CHARACTER.HUMAN);
        }
    }
}
