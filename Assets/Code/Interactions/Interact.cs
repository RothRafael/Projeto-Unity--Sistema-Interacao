using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class Interactable : MonoBehaviour
{
    public bool isOverlapping = false;
    public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    public enum InteractionType
    {
        Press, Hold, Scroll, Move
    }
    public InteractionType interactionType;

    public void BaseStartInteract() { StartInteract(); }
    public void BaseStartInteract(int scroll) { StartInteract(scroll); }
    public void BaseStopInteract() { StopInteract(); }

    protected virtual void StartInteract() { }
    protected virtual void StopInteract() { }
    protected virtual void StartInteract(int scroll) { }

    protected void GetMeshRenderers()
    {
        meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
    }

    public void SetOutlineMaterial(Material material)
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            List<Material> materials = new List<Material>(meshRenderer.materials);
            if (materials.Count < 2)
            {
                materials.Add(material);
                meshRenderer.materials = materials.ToArray();
            }
        }
    }

    public void RemoveOutlineMaterial(Material material)
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            List<Material> materials = new List<Material>(meshRenderer.materials);
            if (materials.Contains(material))
            {
            materials.Remove(material);
            meshRenderer.materials = materials.ToArray();
            }
        }
        Debug.Log("Removed");
    }
}