using System.Collections;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour
{
    public GameObject monster;
    [SerializeField] private Transform[] spawnPoints;
    private GameObject[] monsters;
    bool hasSpawned = false;

    private AudioSource audioSource;
    public AudioClip spawnSound;
    public GameObject blood;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned && Objectives.Instance.GetObjective() == "TrashBag")
        {
            hasSpawned = true;
            if (spawnSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(spawnSound, 5f);
            }
            blood.SetActive(true);
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Debug.Log("Spawning monster at: " + spawnPoints[i].position);
                Instantiate(monster, spawnPoints[i].position, spawnPoints[i].rotation);
            }
            StartCoroutine(Scare());
        }
    }

    private IEnumerator Scare()
    {
        yield return new WaitForSeconds(3f);
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject m in monsters)
        {
            Destroy(m);
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
