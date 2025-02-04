using System;
using UnityEngine;
using UnityEngine.Events;

public class ScrollInteractScript : Interactable
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSensitivity = 1f;
    [SerializeField] private Transform axis;
    [SerializeField] private string rotationAxis;

    [Header("Resistance Settings")]
    [SerializeField] private Vector2 minMaxAngles = new Vector2(-180, 180);
    [SerializeField] private bool canMakeRevolution = false;
    [SerializeField] private float resistanceThreshold = 10f;
    [SerializeField] private float springStrength = 20f;
    [SerializeField] private float damping = 5f;
    [SerializeField] private float snapThreshold = 10f;
    [SerializeField] private float snapPoint = 0.5f;
    [SerializeField] private float scrollDecayRate = 5f; // How fast accumulated scroll resets
    [SerializeField] private float scrollIncrementRate = 1f; // How much scroll counts as one increment
    [Space]
    [Header("Total Scroll")]
    public float totalScroll = 0f;

    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private float velocity = 0f;
    private float accumulatedScroll = 0f;
    private float lastStableAngle = 0f;

    /// <summary>
    /// Audio Manager class
    /// </summary>
    private EntityAudioManager audioManager;
    
    [Header("Events")]
    public UnityEvent OnSnap;
    public UnityEvent<float> OnScroll;

    private void Start()
    {
        InitializeAxisAngle();
        InitializeComponents();
    }

    protected override void StartInteract(int scroll)
    {
        HandleScroll(scroll);
    }

    private void HandleScroll(int scroll)
    {
        accumulatedScroll -= scroll * scrollSensitivity;

        // Bujiganga

        // Define the snapping transition point (e.g., 80% of the way to the next snap point)
        float transitionPoint = snapThreshold * snapPoint;

        if (Mathf.Abs(accumulatedScroll) >= transitionPoint)
        {
            //
            totalScroll += (scroll / scrollIncrementRate) * -1;
            //
            lastStableAngle = targetAngle; // Save the last snapped position
            float direction = Mathf.Sign(accumulatedScroll);
            targetAngle += direction * snapThreshold; // Move to the next snap step
            accumulatedScroll = 0f; // Reset accumulated scroll after snapping

            PlaySound();
            OnScroll.Invoke(totalScroll);
            OnSnap.Invoke();
        }

        
    }

    private void Update()
    {
        if (!canMakeRevolution)
        {
            targetAngle = Mathf.Clamp(targetAngle, minMaxAngles.x, minMaxAngles.y);
        }

        // Reduce accumulated scroll gradually over time (returns to last stable position)
        accumulatedScroll = Mathf.MoveTowards(accumulatedScroll, 0f, scrollDecayRate * Time.deltaTime);

        // Apply both target angle and accumulated scroll influence
        float effectiveAngle = targetAngle + accumulatedScroll;

        // Smooth transition instead of instant snapping
        currentAngle = Mathf.SmoothDamp(currentAngle, effectiveAngle, ref velocity, damping / springStrength);

        // Apply rotation to the selected axis
        switch (rotationAxis)
        {
            case "X":
                axis.localEulerAngles = new Vector3(currentAngle, 0, 0);
                break;
            case "Y":
                axis.localEulerAngles = new Vector3(0, currentAngle, 0);
                break;
            case "Z":
                axis.localEulerAngles = new Vector3(0, 0, currentAngle);
                break;
        }


    }

    private void InitializeAxisAngle()
    {
        Vector3 eulerRotation = axis.eulerAngles;

        switch (rotationAxis)
        {
            case "X":
                targetAngle = eulerRotation.x;
                break;
            case "Y":
                targetAngle = eulerRotation.y;
                break;
            case "Z":
                targetAngle = eulerRotation.z;
                break;
        }

        lastStableAngle = targetAngle;
        currentAngle = targetAngle;
    }
    private void InitializeComponents()
    {
        audioManager = GetComponent<EntityAudioManager>();
    }

    private void PlaySound()
    {
        if (audioManager != null)
        {
            audioManager.PlayGunshot();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(axis.position, axis.forward * 2);

        // Draw spheres at each snap threshold point
        if (snapThreshold <= 0)
        {
            return;
        }
        float angleStep = snapThreshold;
        for (float angle = minMaxAngles.x; angle <= minMaxAngles.y; angle += angleStep)
        {
            Vector3 position = axis.position;
            switch (rotationAxis)
            {
                case "X":
                    position += Quaternion.Euler(-angle, 0, 0) * axis.right * 0.5f;
                    break;
                case "Y":
                    position += Quaternion.Euler(0, -angle, 0) * axis.forward * 0.5f;
                    break;
                case "Z":
                    position += Quaternion.Euler(0, 0, -angle) * axis.up * 0.5f;
                    break;
            }
            Gizmos.DrawWireSphere(position, 0.05f);
        }
    }
}

