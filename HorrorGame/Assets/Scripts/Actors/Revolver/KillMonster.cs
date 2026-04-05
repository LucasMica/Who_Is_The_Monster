using System.Xml.Serialization;
using UnityEngine;

public class KillMonster : MonoBehaviour
{
    public static event System.Action OnKillMonster;

    public LayerMask monsterLayer;
    public float maxLifetime;
    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            ReactToShooting reactToShooting = collision.gameObject.GetComponent<ReactToShooting>();
            if (reactToShooting != null)
            {
                reactToShooting.Die();
            }
            TriggerOnDead();
            Invoke("DestroyBullet", 0.5f);
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public static void TriggerOnDead()
    {
        Debug.Log("Triggered");
        OnKillMonster?.Invoke();
    }
}
