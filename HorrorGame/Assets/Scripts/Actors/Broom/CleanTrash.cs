using System;
using System.Collections;
using UnityEngine;

public class CleanTrash : MonoBehaviour, IHolder
{
    public GameObject playerHand;
    private PlayerInteractions playerInteractions;
    private bool canClean = false;

    [Header("Audio Source")]
    public AudioSource sweepSource;
    private AudioClip[] sweep = new AudioClip[3];
    private AudioClip currentClip;
    private Boolean cleaning = false;

    public float HoldDuration => 3f;


    void Start()
    {
        playerInteractions = GameObject.FindWithTag("MainCamera").GetComponent<PlayerInteractions>();

        sweep[0] = Resources.Load<AudioClip>("Sweeping/sweep1");
        sweep[1] = Resources.Load<AudioClip>("Sweeping/sweep2");
        sweep[2] = Resources.Load<AudioClip>("Sweeping/sweep3");
    }

    public void OnHoldEnd()
    {
        if (canClean)
        {
            Objectives.Instance.incrementBroom();
            if (Objectives.Instance.getBroom() == 3)
            {
                cleaning = false;
                sweepSource.Stop();
                Destroy(playerHand.transform.GetChild(0).gameObject);
                Objectives.Instance.CompleteObjective("CleanTrash");
            }
            Destroy(gameObject);
        }
    }

    public void OnHoldStart()
    {
        if (playerInteractions.holdingBroom)
        {
            canClean = true;
            cleaning = true;
            currentClip = sweep[UnityEngine.Random.Range(0, 3)];
            sweepSource.clip = currentClip;
            sweepSource.Play();
        }
    }

    public void OnHoldUpdate(float holdTime)
    {
        
    }

    public void OnHoldCancel()
    {
        if (cleaning)
        {
            cleaning = false;

            if (sweepSource.isPlaying)
            {
                sweepSource.Stop();
            }
        }
    }
}
