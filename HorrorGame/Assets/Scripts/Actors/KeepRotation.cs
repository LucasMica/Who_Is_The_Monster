using UnityEngine;

public class KeepRotation : MonoBehaviour
{
    private Quaternion initialRotation;
    private AudioSource audioSource;
    public AudioClip rainSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialRotation = transform.rotation;
        if (rainSound != null && audioSource != null)
        {
            audioSource.clip = rainSound;
            audioSource.loop = true;
            audioSource.volume = 0.6f;
            audioSource.Play();
        }
    }

    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
