using UnityEngine;

public class TaskInteraction : MonoBehaviour, IInteractable
{
    public Transform playerHand;
    public bool IsHeld { get; private set; } = false;
    public bool canCollect = false;
    public Vector3 heldOffset;

    [Header("Audio Source")]
    private AudioSource audioSource;
    public AudioClip audioClip;

    private PlayerInteractions playerInteractions;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        playerInteractions = GameObject.FindWithTag("MainCamera").GetComponent<PlayerInteractions>();
    }

    public void Interact()
    {
        if(canCollect)
        {
            if (gameObject.tag == "Broom" && Objectives.Instance.GetObjective() == "CleanTrash") 
            {
                playerInteractions.holdingBroom = true;
                HoldItem();
            }
            else if (gameObject.tag == "Garbage" && Objectives.Instance.GetObjective() == "TrashBag")
            {
                playerInteractions.holdingTrash = true;
                HoldItem();
            }
            else if(gameObject.tag == "Revolver" && Objectives.Instance.GetObjective() == "Kill")
            {
                HoldItem();
                Shoot revolver = playerHand.GetComponentInChildren<Shoot>();
                revolver.readyToShoot = true;
            }
        }
    }

    private void HoldItem()
    {
        audioSource.PlayOneShot(audioClip);
        GetComponent<Collider>().enabled = false;
        transform.position = playerHand.position + heldOffset;
        transform.SetParent(playerHand);
        transform.rotation = playerHand.transform.rotation;
        IsHeld = true;
    }
}
