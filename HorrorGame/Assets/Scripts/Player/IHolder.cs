using UnityEngine;

public interface IHolder
{
    float HoldDuration { get; }
    void OnHoldStart();
    void OnHoldEnd();
    void OnHoldUpdate(float holdTime);
    void OnHoldCancel();
}
