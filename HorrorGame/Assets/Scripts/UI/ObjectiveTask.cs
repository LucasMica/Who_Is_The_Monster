using UnityEngine;

public class ObjectiveTask : MonoBehaviour
{
    [Tooltip("The ID of the objective this task completes.")]
    public string objectiveID;

    [Tooltip("Complete the objective when this GameObject is interacted with or triggered.")]
    public bool completeOnTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!completeOnTrigger) return;

        if (other.CompareTag("Player"))
        {
            Objectives.Instance.CompleteObjective(objectiveID);
            gameObject.SetActive(false);
        }
    }

    public void CompleteManually()
    {
        Objectives.Instance.CompleteObjective(objectiveID);
    }
}
