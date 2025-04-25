using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] public DSDialogueSO startingDialogue;
    [SerializeField] public TextMeshProUGUI textUI;
    [SerializeField] public TextMeshProUGUI nameUI;
    [SerializeField] public Image Textbox;

    private DSDialogueSO currentDialogue;
    public bool isPlayerInRange = false;
    public bool Tutrial = false;

    private float inputCooldown = 0.25f; // Cooldown between allowed inputs
    private float lastInputTime = -1f;

    private void Awake()
    {
        currentDialogue = startingDialogue;
        HideText();
    }

    private void Update()
    {
        // Only allow input if enough time has passed
        if (Time.time - lastInputTime > inputCooldown)
        {
            if (isPlayerInRange && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Talk")))
            {
                ShowText();
                lastInputTime = Time.time;
            }

            if (isPlayerInRange && Tutrial)
            {
                ShowText();
                lastInputTime = Time.time;
            }
        }
    }

    public void ShowText()
    {
        if (currentDialogue != null)
        {
            textUI.text = currentDialogue.Text;
            nameUI.text = currentDialogue.Speaker;
            textUI.gameObject.SetActive(true);
            nameUI.gameObject.SetActive(true);
            Textbox.gameObject.SetActive(true);
            CursorManager.Instance.ShowCursor();
        }
    }

    public void HideText()
    {
        textUI.gameObject.SetActive(false);
        nameUI.gameObject.SetActive(false);
        Textbox.gameObject.SetActive(false);
        CursorManager.Instance.HideCursor();
    }

    public void OnOptionChosen(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= currentDialogue.Choices.Count)
        {
            Debug.LogError("Invalid choice index");
            return;
        }

        DSDialogueSO nextDialogue = currentDialogue.Choices[choiceIndex].NextDialogue;

        if (nextDialogue == null)
        {
            Debug.Log("End of dialogue");
            HideText();
            CursorManager.Instance.HideCursor();
            return;
        }

        currentDialogue = nextDialogue;
        ShowText();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            currentDialogue = startingDialogue;
            HideText();
        }
    }

    public void Reset()
    {
        currentDialogue = startingDialogue;
        HideText();
    }
}
