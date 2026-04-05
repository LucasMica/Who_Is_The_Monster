using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Readable : MonoBehaviour, IInteractable
{
    private PlayerMovement player;

    [Header("UI text")]
    public GameObject noteCanvas;

    [Header("Note Sound")]
    private AudioSource noteSource;
    public AudioClip noteSound;

    [Header("Particles")]
    public GameObject particles;
    private Boolean wasRead = false;

    private GameObject UI;

    private bool isOpen = false;

    public void Awake()
    {
        UI = GameObject.Find("Crosshair");
    }

    public void Start()
    {
        noteCanvas.SetActive(false);

        if (noteSound != null)
        {
            noteSource = GetComponent<AudioSource>();

            if (noteSource == null)
            {
                noteSource = gameObject.AddComponent<AudioSource>();
            }
        }

        player = GameObject.Find("Main Camera").GetComponentInChildren<PlayerMovement>();

    }

    private void Update()
    {
        if (isOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            disableNote();
            
        }
    }

    public void Interact()
    {
        showNote();
    }

    public void showNote()
    {
        if (noteSound != null)
        {
            noteSource.PlayOneShot(noteSound);
        }

        noteCanvas.SetActive(true);
        UI.SetActive(false);
        isOpen = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!wasRead)
        {
            wasRead = true;
            particles.SetActive(false);
        }
    }

    public void disableNote()
    {
        noteCanvas.SetActive(false);
        UI.SetActive(true);
        isOpen = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
