using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField][TextArea] string[] dialogue;

    DialogueController dialogueController;

    void Awake()
    {
        dialogueController = FindFirstObjectByType<DialogueController>(FindObjectsInactive.Include);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueController.StartDialogue(dialogue);
        }
    }
}
