using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorWife : OpenDoor
{
    private bool canOpenDoor = false;
    private WifeDialogs wifeDialogs;

    public override void Start()
    {
        base.Start();
        wifeDialogs = GetComponent<WifeDialogs>();
    }

    public override void Interact()
    {
        if (canOpenDoor)
        {
            base.Interact();
            StartCoroutine(FinalMoment());
        }
    }

    public void AllowOpening()
    {
        canOpenDoor = true;
        Destroy(wifeDialogs);
    }

    private IEnumerator FinalMoment()
    {
        yield return new WaitForSeconds(5f);
        UIManager.UIInstance.fadeCanvas.GetComponent<FadeEffect>().CallFadeOut();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
