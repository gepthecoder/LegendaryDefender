using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }

    public void TriggerShopDialogue()
    {
        FindObjectOfType<DialogManager>().StartShopDialogue(dialogue);
    }
}
