using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteractable
{
    public float duration = 1.0f;

    private bool isOpen = false;
    private bool isOpening = false;

    private Quaternion initialRotation;
    private Quaternion opened;
    private Quaternion closed;


    [Header("Sons da Porta")]
    private AudioSource doorSource;
    private AudioClip openingDoor;
    private AudioClip closingDoor;
    private AudioClip knockingDoor;

    public virtual void Start()
    {
        if (doorSource == null)
        {
            doorSource = gameObject.AddComponent<AudioSource>();
        }

        openingDoor = Resources.Load<AudioClip>("Door/OpenDoor");
        closingDoor = Resources.Load<AudioClip>("Door/CloseDoor");
        knockingDoor = Resources.Load<AudioClip>("Door/KnockDoor");

        closed = transform.rotation;
    }

    public virtual void Interact()
    {        
        if (!isOpening)
        {
            if (!isOpen)
            {
                doorSource.PlayOneShot(openingDoor);
            }
            else { doorSource.PlayOneShot(closingDoor); }

            CallRotation();
        }
    }

    private IEnumerator DoorRotation()
    {
        isOpening = true;

        opened = Quaternion.Euler(
            closed.eulerAngles.x,
            closed.eulerAngles.y - 90f,
            closed.eulerAngles.z
        );

        Quaternion start = transform.rotation;
        Quaternion end = isOpen ? closed : opened;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = end;

        isOpen = !isOpen;
        isOpening = false;
    }


    public bool GetIsOpen()
    {
        return isOpen;
    }

    public void CallRotation()
    {
        StartCoroutine(DoorRotation());
    }
}
