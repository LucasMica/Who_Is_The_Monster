using System.Threading.Tasks;
using UnityEngine;

public class GarbageCollector : MonoBehaviour, IInteractable
{
    private GameObject garbage;
    private TaskInteraction garbageInteraction;

    [Header("Audio Source")]
    public AudioSource TrashSource;
    private AudioClip takingTrashInto;
    private AudioClip OpeningtrashCan;

    void Start()
    {
        if (TrashSource == null) { 
            TrashSource = gameObject.AddComponent<AudioSource>();
        }

        takingTrashInto = Resources.Load<AudioClip>("Trash/taking-trash-into-can");
        OpeningtrashCan = Resources.Load<AudioClip>("Trash/trash-can");

        garbage = GameObject.FindGameObjectWithTag("Garbage");
        garbageInteraction = garbage.GetComponent<TaskInteraction>();
    }

    public void Interact()
    {
        if (garbage != null && garbageInteraction.IsHeld)
        {
            TrashSource.PlayOneShot(takingTrashInto);
            TrashSource.PlayOneShot(OpeningtrashCan);
            Objectives.Instance.CompleteObjective("TrashBag");
            Destroy(garbage);
        }
    }
}
