using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerInteract playerInteractScript;
    [SerializeField] RectTransform crosshair;
    [SerializeField] float crosshairInteractSize;
    private Vector3 crosshairInteractOriginalSize;

    private void Start() {
        crosshairInteractOriginalSize = crosshair.localScale;
    }
    private void FixedUpdate() {
        if(playerInteractScript.currentInteractable != null) {
            crosshair.localScale = crosshairInteractOriginalSize * crosshairInteractSize;
        } else {
            crosshair.localScale = crosshairInteractOriginalSize;
        }
    }
}
