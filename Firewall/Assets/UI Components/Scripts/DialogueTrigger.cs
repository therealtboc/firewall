using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue diaglogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(diaglogue);
    }
}
