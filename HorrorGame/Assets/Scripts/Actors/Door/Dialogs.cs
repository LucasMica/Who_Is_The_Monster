using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogs : MonoBehaviour, IInteractable
{
    public Canvas playerHUD;

    [Header("Dialog UI Settings")]
    public Canvas dialogCanvas;
    public TextMeshProUGUI personName;
    public TextMeshProUGUI dialogText;
    protected TextMeshProUGUI explanationText;

    [Header("Dialog Content")]
    public string[] dialogLines;
    public string speakerName = "NPC";

    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private InputActionMap interactionMap;
    private InputAction skipAction;

    private bool isDialogActive;
    private int currentLineIndex;
    private Coroutine typingCoroutine;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;

    protected bool hasPlayed = false;

    public event System.Action OnDialogComplete;

    protected virtual void Start()
    {
        dialogCanvas.enabled = false;
        playerMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMovement>();

        playerInput = GameObject.FindWithTag("MainCamera")?.GetComponent<PlayerInput>();
        interactionMap = playerInput.actions.FindActionMap("Interaction");
        skipAction = interactionMap.FindAction("Dialog");

        explanationText = GameObject.FindGameObjectWithTag("ExplanationText").GetComponent<TextMeshProUGUI>();
        explanationText.enabled = false;
    }

    public virtual void Interact()
    {
        if (isDialogActive) return;

        dialogCanvas.enabled = true;
        isDialogActive = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        playerHUD.enabled = false;

        personName.text = speakerName;

        if (hasPlayed)
        {
            dialogText.text = "...";
        }
        else
        {
            currentLineIndex = 0;
            ShowDialog();
            hasPlayed = true;
        }
    }

    private void ShowDialog()
    {
        if (dialogLines.Length == 0 || currentLineIndex >= dialogLines.Length)
        {
            dialogText.text = "...";
        }
        else
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(dialogLines[currentLineIndex]));
        }
    }

    private IEnumerator TypeLine(string line)
    {
        dialogText.text = "";
        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void LeaveDialog()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;

        playerHUD.enabled = true;
        dialogCanvas.enabled = false;
        isDialogActive = false;
    }

    void Update()
    {
        if (!isDialogActive) return;

        if (skipAction.WasPerformedThisFrame())
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogLines[currentLineIndex];
                typingCoroutine = null;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < dialogLines.Length)
                {
                    ShowDialog();
                }
                else
                {
                    LeaveDialog();
                    OnDialogComplete?.Invoke();
                }
            }

        }
    }

    public IEnumerator ShowExplanation()
    {
        explanationText.enabled = true;
        yield return new WaitForSeconds(2f);
        explanationText.enabled = false;
    }

    public int GetCurrentLineIndex()
    {
        return currentLineIndex;
    }
}
