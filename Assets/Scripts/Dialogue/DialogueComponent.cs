using EasyTextEffects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogueComponent : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private TextEffect effect;
    private string text;
    private DialogueEntry entry;

    [SerializeField] private string dialogueID;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        effect = GetComponent<TextEffect>();
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(dialogueID))
        {
            entry = DialogueManager.GetDialogue(dialogueID);
            text = (entry == null) ? dialogueID + " not found" : entry.text;
            tmp.text = text;
            tmp.enabled = false;
            effect.enabled = false;
        }
    }

    public void ChangeTextID(string newID)
    {
        dialogueID = newID;
    }

    public void ShowDialogue(string newID = "")
    {
        // checks the newID entered is valid, then sets that to the new dialogue.
        if (!string.IsNullOrEmpty(newID) && newID != dialogueID)
        {
            ClearDialogue();
            dialogueID = newID;
            entry = DialogueManager.GetDialogue(dialogueID);
            text = (entry == null) ? dialogueID + " not found" : entry.text;
            tmp.text = text;
        }
        tmp.enabled = true;
        effect.enabled = true;
    }

    // "force" clears it, with no effect
    public void ClearDialogue()
    {
        tmp.enabled = false;
        effect.enabled = false;
    }

    // hides the dialogue with its intended effect, if it has one
    public void HideDialogue()
    {
        if (effect == null)
        {
            ClearDialogue();
            return;
        }

        effect.StartManualEffects();
    }
}