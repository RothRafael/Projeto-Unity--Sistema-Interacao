using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerInteract playerInteractScript;
    [SerializeField] RawImage crosshair;
    [SerializeField] Texture2D crosshairInteract;
    [SerializeField] Texture2D crosshairInteractYes;
    [SerializeField] float crosshairInteractSize;
    private Vector3 crosshairInteractOriginalSize;

    private void Start() {
        crosshair.texture = crosshairInteract;
    }
    private void FixedUpdate() {
        if(playerInteractScript.currentInteractable != null) {
            crosshair.texture = crosshairInteractYes;
        } else {
            crosshair.texture = crosshairInteract;
        }
    }
}
