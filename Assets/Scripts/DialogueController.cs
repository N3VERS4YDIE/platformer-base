using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;

    Queue<string> dialogue = new();
    float lastTimeScale;

    void Awake()
    {
        GameManager.Instance.InputActions.UI.Next.performed += OnNextPerformed;
    }

    void OnEnable()
    {
        GameManager.Instance.InputActions.UI.Enable();
    }

    void OnDisable()
    {
        GameManager.Instance.InputActions.UI.Disable();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InputActions.UI.Next.performed -= OnNextPerformed;
        }
    }

    void OnNextPerformed(InputAction.CallbackContext _)
    {
        NextText();
    }

    public void StartDialogue(IEnumerable<string> dialogue)
    {
        this.dialogue = new(dialogue);
        lastTimeScale = Time.timeScale;
        NextText();
    }

    void NextText()
    {
        if (dialogue.Count > 0)
        {
            dialogueText.text = dialogue.Dequeue();
            GameManager.Instance.InputActions.Player.Disable();
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.InputActions.Player.Enable();
            Time.timeScale = lastTimeScale;
            gameObject.SetActive(false);
        }
    }
}
