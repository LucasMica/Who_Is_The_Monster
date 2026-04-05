using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Day UI")]
    public Canvas dayCanvas;
    public TextMeshProUGUI dayText;
    public int currentDay = 1;

    private CanvasGroup canvasGroup;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        playerMovement = GameObject.FindWithTag("MainCamera")?.GetComponent<PlayerMovement>();

        canvasGroup = dayCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = dayCanvas.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        dayText.text = $"Day {currentDay}";

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        playerMovement.enabled = false;
        yield return new WaitForSeconds(2f);
        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        playerMovement.enabled = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}