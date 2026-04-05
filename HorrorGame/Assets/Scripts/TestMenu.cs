using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMenu : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
