using Unity.VisualScripting;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [Header("Referências")]
    public PlayerMovement player;         
    public AudioSource footstepsSource;

    [Header("Sons de Passos")]
    public AudioClip[] footstepsOnConcrete;
    public AudioClip[] footstepsOnGrass;
    public AudioClip[] footstepsOnWood;

    private float stepDelay = 0.32f;         
    private float stepTimer = 0f;

    void Update()
    {
        if (player.enabled == false)
        {
            return;
        }

        if (player != null && player.isMoving)
        {
            RaycastHit hit;

            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 2f))
            {
                string tag = hit.collider.tag;

                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    switch (tag)
                    {
                        case "Concrete":
                            PlayRandomStep(footstepsOnConcrete);
                            stepTimer = stepDelay; break;
                        case "Grass":
                            PlayRandomStep(footstepsOnGrass);
                            stepTimer = stepDelay; break;
                        case "Wood":
                            PlayRandomStep(footstepsOnWood);
                            stepTimer = stepDelay; break;
                    }
                }
            }
        }

        void PlayRandomStep(AudioClip[] clips)
        {
            if (clips.Length == 0 || footstepsSource == null)
                return;

            int index = Random.Range(0, clips.Length);
            footstepsSource.PlayOneShot(clips[index]);
        }
    }
}
