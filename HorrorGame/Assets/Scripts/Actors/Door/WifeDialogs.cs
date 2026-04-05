using TMPro;
using UnityEngine;

public class WifeDialogs : Dialogs
{


    protected override void Start()
    {
        base.Start();
    }
    
    public override void Interact()
    {
        if (Objectives.Instance.GetObjective() == "TalkWife")
        {
            base.Interact();
            Objectives.Instance.CompleteObjective("TalkWife");
        }
        else
        {
            StartCoroutine(ShowExplanation());
        }
    }
}
