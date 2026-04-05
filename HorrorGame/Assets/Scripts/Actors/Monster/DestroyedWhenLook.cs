using UnityEngine;

public class DestructibleOnLook : MonoBehaviour
{
    public float maxLookDistance = 30f;
    public float viewAngle = 15f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
            enabled = false;
    }

    void Update()
    {
        if (mainCamera == null)
            return;

        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        if (distance > maxLookDistance)
            return;

        Vector3 directionToObject = transform.position - mainCamera.transform.position;
        float angle = Vector3.Angle(mainCamera.transform.forward, directionToObject);

        if (angle < viewAngle)
        {
            if (Physics.Linecast(mainCamera.transform.position, transform.position, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
