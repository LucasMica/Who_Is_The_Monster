using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private PlayerMovement playerMovement;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMovement>();
    }

    public void CallFadeIn() => StartCoroutine(FadeIn());
    public void CallFadeOut() => StartCoroutine(FadeOut());

    private IEnumerator FadeOut()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;
        float duration = 1.0f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        float duration = 1.0f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}
