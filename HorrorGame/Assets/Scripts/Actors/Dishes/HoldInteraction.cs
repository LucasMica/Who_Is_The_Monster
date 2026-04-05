using System;
using UnityEngine;

public class HoldInteraction : MonoBehaviour, IHolder
{
    private GameObject player;
    private PlayerInteractions pi;


    private GameObject garbage;
    private TaskInteraction garbageInteraction;

    [Header("Audio Source")]
    public AudioSource dishesSource;
    private AudioClip[] dishes = new AudioClip[3];
    private AudioClip currentClip;

    [Header("Particles")]
    public ParticleSystem particles;
    private Boolean doingDishes = false;

    void Start()
    {
        if (dishesSource == null)
        {
            dishesSource = gameObject.AddComponent<AudioSource>();
        }

        dishes[0] = Resources.Load<AudioClip>("Dishes/dishes1");
        dishes[1] = Resources.Load<AudioClip>("Dishes/dishes2");
        dishes[2] = Resources.Load<AudioClip>("Dishes/dishes3");

        player = GameObject.FindWithTag("Player");
        pi = player.GetComponentInChildren<PlayerInteractions>();

        garbage = GameObject.FindGameObjectWithTag("Garbage");
        garbageInteraction = garbage.GetComponent<TaskInteraction>();
    }

    public float HoldDuration => 5f;

    public void OnHoldEnd()
    {
        if (Objectives.Instance.GetObjective() == "Dishes")
        {
            doingDishes = false;
            particles.Stop();
            dishesSource.Stop();
            Objectives.Instance.CompleteObjective("Dishes");
            garbageInteraction.canCollect = true;
            Destroy(gameObject);
        }
    }

    public void OnHoldStart()
    {
        if (Objectives.Instance.GetObjective() == "Dishes")
        {
            if (!doingDishes)
            {
                doingDishes = true;
                particles.Play();
                currentClip = dishes[UnityEngine.Random.Range(0, 3)];
                dishesSource.clip = currentClip;
                dishesSource.Play();
            }
        }
    }

    public void OnHoldCancel()
    {
        if (doingDishes)
        {
            doingDishes = false;
            particles.Stop();

            if (dishesSource.isPlaying)
                dishesSource.Stop();
        }
    }

    public void OnHoldUpdate(float holdTime)
    {
       
    }
}
