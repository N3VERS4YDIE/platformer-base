using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;

    private PlayerController playerController;
    private InputActions inputActions;

    private List<string> dialogue;
    private float lastTimeScale;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>(FindObjectsInactive.Include);
        inputActions = new();

        inputActions.UI.Next.performed += _ => NextText();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    public void StartDialogue(List<string> dialogue)
    {
        this.dialogue = new(dialogue);
        gameObject.SetActive(true);
        NextText();

        playerController.enabled = false;
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void NextText()
    {
        if (dialogue.Count > 0)
        {
            dialogueText.text = dialogue[0];
            dialogue.RemoveAt(0);
        }
        else
        {
            gameObject.SetActive(false);
            playerController.enabled = true;
            Time.timeScale = lastTimeScale;
        }
    }
}
