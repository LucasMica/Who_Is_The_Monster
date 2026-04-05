using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextDay : MonoBehaviour, IInteractable
{
    private int sceneIndex;
    public bool canSkip = false;

    private TextMeshProUGUI explanationText;

    void Start()
    {
        Objectives.Instance.ResetObjectives();
        explanationText = GameObject.Find("ExplanationText").GetComponent<TextMeshProUGUI>();
    }

    public void Interact()
    {
        if (canSkip)
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            StartCoroutine(ShowExplanation());
        }
    }

    public IEnumerator ShowExplanation()
    {
        explanationText.enabled = true;
        yield return new WaitForSeconds(2f);
        explanationText.enabled = false;
    }
}
