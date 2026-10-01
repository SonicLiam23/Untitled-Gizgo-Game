using System.Collections.Generic;
using UnityEngine;

public class InputActionsManager : MonoBehaviour
{
    public static InputActionsManager Instance { get; private set; } = null;

    //i dont want this serialized so ignore the warning
    [HideInInspector] public Dictionary<CHARACTER, CharacterControls> characterController;
    public InputSystem_Actions InputActions { get; private set; }
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
        characterController = new();
    }

    private void Start()
    {
        characterController[CHARACTER.ELEPHANT] = GameManager.Instance.Elephant.GetComponent<CharacterControls>();
        characterController[CHARACTER.HUMAN] = GameManager.Instance.Human.GetComponent<CharacterControls>();
        characterController[CHARACTER.ELEPHANT].enabled = false;
        characterController[CHARACTER.HUMAN].enabled = true;
        InputActions.Player.SetCallbacks(characterController[CHARACTER.HUMAN]);
    }

    public void SwitchCharacter(CurrentCharacter character)
    {
        if (character.type == CHARACTER.ELEPHANT)
        {
            characterController[CHARACTER.HUMAN].enabled = false;
            characterController[CHARACTER.ELEPHANT].enabled = true;

            InputActions.Player.SetCallbacks(characterController[CHARACTER.ELEPHANT]);
        }
        else
        {
            characterController[CHARACTER.HUMAN].enabled = true;
            characterController[CHARACTER.ELEPHANT].enabled = false;

            InputActions.Player.SetCallbacks(characterController[CHARACTER.HUMAN]);
        }
    }
}
