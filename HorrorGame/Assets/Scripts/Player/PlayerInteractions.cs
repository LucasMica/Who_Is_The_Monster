using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction interactAction;
    private InputActionMap interactionMap;

    [Header("Interaction Settings")]
    public float maxInteractionDistance = 5f;
    private IHolder currentHolder;
    private float holdTimer;
    private bool isHolding;

    private GameObject interactText;

    private PlayerMovement playerMovement;
    private bool canMove = true;

    public bool holdingBroom = false;
    public bool holdingTrash = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();

        interactionMap = playerInput.actions.FindActionMap("Interaction");
        interactAction = interactionMap.FindAction("Interact");
        interactionMap.Enable();

        interactText = GameObject.Find("InteractText");
        interactText.SetActive(false);
    }

    private void Update()
    {
        playerMovement.setCanMove(canMove);
        if (!canMove) return;

        bool hitSomething = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxInteractionDistance);
        IInteractable interactable = null;
        IHolder holder = null;

        if (hitSomething)
        {
            hit.collider.TryGetComponent<IInteractable>(out interactable);
            hit.collider.TryGetComponent<IHolder>(out holder);
        }

        if (interactable != null || holder != null)
        {
            interactText.SetActive(true);
            interactText.GetComponent<TextMeshProUGUI>().text = holder != null ? "Hold (E)" : "Interact (E)";
        }
        else
        {
            interactText.SetActive(false);
        }

        if (interactAction.WasPerformedThisFrame() && interactable != null)
        {
            interactable.Interact();
        }

        if (interactAction.WasPressedThisFrame() && holder != null)
        {
            currentHolder = holder;
            holdTimer = 0f;
            isHolding = true;
            currentHolder.OnHoldStart();
        }

        if (isHolding && currentHolder != null)
        {
            if (interactAction.IsPressed() && holder == currentHolder)
            {
                holdTimer += Time.deltaTime;
                currentHolder.OnHoldUpdate(holdTimer);

                if (holdTimer >= currentHolder.HoldDuration)
                {
                    currentHolder.OnHoldEnd();
                    ResetHold();
                }
            }
            else
            {
                ResetHold();
            }
        }
    }

    private void ResetHold()
    {
        if (currentHolder != null) {
            currentHolder.OnHoldCancel(); 
        }

        holdTimer = 0f;
        isHolding = false;
        currentHolder = null;
    }
}
