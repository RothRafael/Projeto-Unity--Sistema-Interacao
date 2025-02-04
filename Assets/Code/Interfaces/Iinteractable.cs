using UnityEngine;

public interface Iinteractable
{
    void StartInteract();
    void StopInteract();
}
public interface IInteractableScroll : Iinteractable
{
    void StartInteract(float scroll);
}