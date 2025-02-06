using UnityEngine;

public class InteractHighlightManager : MonoBehaviour
{
    [SerializeField] Material interactMaterial;
    private PlayerInteract playerInteractScript;
    private Interactable currnetInteractable;
    void Start()
    {
        playerInteractScript = GetComponent<PlayerInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInteractScript.currentInteractable != null) {
            currnetInteractable = playerInteractScript.currentInteractable;
            currnetInteractable.SetOutlineMaterial(interactMaterial);
        } else {
            if(currnetInteractable != null) {
                currnetInteractable.RemoveOutlineMaterial(interactMaterial);
                currnetInteractable = null;
            }
        }
    }
}
