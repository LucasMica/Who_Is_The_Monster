using System.Collections;
using UnityEngine;

public class CompleteVisitor : MonoBehaviour
{
    private Dialogs visitorDialog;

    void Start()
    {
        visitorDialog = GetComponent<Dialogs>();

        visitorDialog.OnDialogComplete += HandleDialogComplete;
    }

    private void HandleDialogComplete()
    {
        StartCoroutine(DestroyVisitor());
    }

    private IEnumerator DestroyVisitor()
    {
        UIManager.UIInstance.fadeCanvas.GetComponent<FadeEffect>().CallFadeOut();
        yield return new WaitForSeconds(2f);

        Objectives.Instance.CompleteObjective("Visitor");
        transform.position = new Vector3(9999, 9999, 9999);
        yield return new WaitForSeconds(1f);

        UIManager.UIInstance.fadeCanvas.GetComponent<FadeEffect>().CallFadeIn();
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (visitorDialog != null)
            visitorDialog.OnDialogComplete -= HandleDialogComplete;
    }
}
