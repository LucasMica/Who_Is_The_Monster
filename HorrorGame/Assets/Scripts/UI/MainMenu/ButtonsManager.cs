using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private FadeEffect fadeCanvas;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        StartCoroutine(FadeToStart());
    }
     
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeToStart()
    {
        fadeCanvas.CallFadeOut();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Day1");
    }
}
