using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [Tooltip("Put the Dialogue JSON file here.")]
    public TextAsset dialogueJson;
    private static Dictionary<string, DialogueEntry> dialogueDict;

    private void OnValidate()
    {
        if (dialogueJson == null)
        {
            Debug.LogWarning($"{nameof(dialogueJson)} must be assigned on {gameObject.name}", this);
        }
    }

    private void Awake()
    {
        dialogueDict = JsonConvert.DeserializeObject<Dictionary<string, DialogueEntry>>(dialogueJson.text);
    }

    public static DialogueEntry GetDialogue(string ID)
    {
        if (!dialogueDict.TryGetValue(ID, out DialogueEntry entry))
        {
            Debug.LogWarning($"Dialogue ID '{ID}' not found.");
            return null;
        }

        return entry;
    }
}



[System.Serializable]
public class DialogueEntry
{
    // display text
    public string text;
    // when this dialogue is hit, should it become the new default?
    public bool setDefault;
}
