using System.Collections.Generic;
using UnityEngine;

public class InputActionsManager
{
    public static InputActionsManager Instance { get; private set; } = null;

    //i dont want this serialized so ignore the warning
    [HideInInspector] public Dictionary<CHARACTER, CharacterControls> characterController;
    public InputSystem_Actions InputActions { get; private set; }
    public Vector3 MovementInput;

    private InputActionsManager()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Player.Enable();
        characterController = new();
    }

    public static void Init()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = new InputActionsManager();

        Instance.characterController[CHARACTER.ELEPHANT] = GameManager.Instance.Elephant.GetComponent<CharacterControls>();
        Instance.characterController[CHARACTER.HUMAN] = GameManager.Instance.Human.GetComponent<CharacterControls>();
        Instance.characterController[CHARACTER.ELEPHANT].enabled = false;
        Instance.characterController[CHARACTER.HUMAN].enabled = true;
        Instance.InputActions.Player.SetCallbacks(Instance.characterController[CHARACTER.HUMAN]);
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
