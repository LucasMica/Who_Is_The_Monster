using System.Collections;
using UnityEngine;

public class SpawnVisitors : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject visitor;
    [SerializeField] private OpenDoor door;

    public AudioSource audioSource;
    public AudioClip doorbell;
    public AudioClip doorKnocking;

    private bool canActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Objectives.Instance.GetObjective() == "TalkWife")
        {
            if (canActive) 
            {
                StartCoroutine(Activate());
            }
        }
    }

    private IEnumerator Activate()
    {
        canActive = false;
        if(door.GetIsOpen())
        {
            door.Interact();
        }

        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(doorbell);

        Objectives.Instance.CallVisitors();

        GameObject visitorInstanced = Instantiate(visitor, spawnLocation.position, spawnLocation.rotation);

        Dialogs visitorDialog = visitorInstanced.GetComponent<Dialogs>();
        visitorDialog.dialogCanvas = UIManager.UIInstance.dialogCanvas;
        visitorDialog.playerHUD = UIManager.UIInstance.playerHUD;
        visitorDialog.personName = UIManager.UIInstance.personName;
        visitorDialog.dialogText = UIManager.UIInstance.dialogText;

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
