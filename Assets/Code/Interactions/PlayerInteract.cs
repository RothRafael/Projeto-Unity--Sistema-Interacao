using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 2f; // Distance for interaction
    [SerializeField] private LayerMask interactLayer; // LayerMask for interactable objects

    public Interactable currentInteractable;
    private bool isInteracting;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isInteracting)
        {
            DetectInteractable();
            
        }
        
        if (currentInteractable != null)
        {
            HandleInteraction();
        }
    }

    private void DetectInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactDistance, interactLayer))
        {
            Debug.DrawLine(mainCamera.transform.position, hit.point, Color.green);
            
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            
            if (interactable != null)
            {
                currentInteractable = interactable;
            }
        }
        else
        {   
            currentInteractable = null;
        }
    }

    private void HandleInteraction()
    {
        switch (currentInteractable.interactionType)
        {
            case Interactable.InteractionType.Hold:
                HandleHoldInteraction();
                break;

            case Interactable.InteractionType.Press:
                HandlePressInteraction();
                break;

            case Interactable.InteractionType.Scroll:
                HandleScrollInteraction();
                break;
        }
    }

    private void HandleHoldInteraction()
    {
        float distance = Vector3.Distance(transform.position, currentInteractable.transform.position);

        if (Input.GetKey(KeyCode.E) && distance <= interactDistance)
        {
            if (!isInteracting)
            {
                currentInteractable.BaseStartInteract();
                isInteracting = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) || distance > interactDistance)
        {
            if (isInteracting)
            {
                currentInteractable.BaseStopInteract();
                isInteracting = false;
            }
        }
    }

    private void HandlePressInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.BaseStartInteract();
        }
    }

    private void HandleScrollInteraction()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            currentInteractable.BaseStartInteract((int)Input.mouseScrollDelta.y);
        }
    }
}
