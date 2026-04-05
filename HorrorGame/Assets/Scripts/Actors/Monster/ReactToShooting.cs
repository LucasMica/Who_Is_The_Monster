using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ReactToShooting : MonoBehaviour
{
    public float scareDistance = 30f;

    private Animator animator;
    private Collider monsterCollider;
    private NavMeshAgent agent;
    //private Rigidbody rb;

    private bool isDead = false;
    private bool isScared = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterCollider = GetComponent<Collider>();
        //rb = GetComponent<Rigidbody>();
        

        MonsterManager.Instance.MonsterList.Add(this);
    }

    void Update()
    {
        if (isScared && !isDead)
        {
            // If reached destination, pick a new one
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(WaitTime());
            }
        }
    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.4f);
        PickNewEscapePosition();
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            agent.isStopped = true;
            agent.enabled = false;
            monsterCollider.enabled = false;
            MonsterManager.Instance.MonsterList.Remove(this);
            animator.SetBool("Dead", true);
            //rb.freezeRotation = true;

            if (MonsterManager.Instance.MonstersLeft() <= 0)
            {
                Objectives.Instance.CompleteObjective("Kill");
            }
        }
    }

    // Called externally when the player fires a gun
    public void GetScared()
    {
        if (isDead) return;

        animator.SetBool("Run", true);
        isScared = true;

        PickNewEscapePosition();
    }

    private void PickNewEscapePosition()
    {
        // Create a random direction around the AI
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * scareDistance;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, scareDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }

    private void OnEnable()
    {
        Shoot.OnShoot += GetScared;
    }

    private void OnDisable()
    {
        Shoot.OnShoot -= GetScared;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isScared && !isDead)
        {
            // Immediate redirect on collision
            PickNewEscapePosition();
        }
    }
}
