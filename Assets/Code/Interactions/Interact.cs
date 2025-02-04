using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Press, Hold, Scroll, Move
    }
    public InteractionType interactionType;

    public void BaseStartInteract() { StartInteract(); }
    public void BaseStartInteract(int scroll) { StartInteract(scroll); }
    public void BaseStopInteract() { StopInteract(); }

    // Classe que implementa faz o override
    protected virtual void StartInteract() { }

    // Classe que implementa faz o override
    protected virtual void StopInteract() { }
    //Scroll
    protected virtual void StartInteract(int scroll) { }
}
