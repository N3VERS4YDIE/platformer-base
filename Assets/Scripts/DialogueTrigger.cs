using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField][TextArea] private List<string> dialogue;
    
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = FindFirstObjectByType<DialogueController>(FindObjectsInactive.Include);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueController.StartDialogue(dialogue);
        }
    }
}
