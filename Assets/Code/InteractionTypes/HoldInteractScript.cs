using System;
using UnityEngine;

public class HoldInteractScript : Interactable
{
    // Wheight == Speed
    [SerializeField] private float Weight;
    [SerializeField] private float distanceTreshold = .3f;
    public bool isHolding = false;
    private Rigidbody rb;
    private Transform PlayerHand;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerHand = GameObject.Find("PlayerHand").transform;
    }
    protected override void StartInteract()
    {
        isHolding = true;
    }
    protected override void StopInteract()
    {
        isHolding = false;
    }
    private void FixedUpdate()
    {
        if (isHolding)
        {
            PlayerHold();
        }
        else if (!isHolding && rb.useGravity == false)
        {
            PlayerRelease();
        }
    }
    private void PlayerHold()
    {
        Vector3 direction = (PlayerHand.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, PlayerHand.transform.position);

        // Apply Rotation to the object
        // rb.rotation = Quaternion.Lerp(Camera.main.transform.rotation, transform.rotation, Time.deltaTime); // targetRotation;

        // DEBUG
        Debug.DrawLine(transform.position, PlayerHand.transform.position, Color.red);

        // Disable gravity while holding
        rb.useGravity = false;

        // Apply force to move the object toward the PlayerHand
        rb.linearVelocity = direction * distance;

        // Check if the object is close enough to the PlayerHand
        if (distance < distanceTreshold)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Apply force to move the object precisely to the PlayerHand position
            Vector3 snapForce = (PlayerHand.transform.position - transform.position) * 10;
            rb.AddForce(snapForce, ForceMode.VelocityChange);
        }
    }
    private void PlayerRelease()
    {
        // Enable gravity
        rb.useGravity = true;

        // // Apply impulse to simulate momentum
        Vector3 releaseVelocity = rb.linearVelocity;
        // rb.AddForce(releaseVelocity * 100, ForceMode.Impulse);

        Debug.Log("Release  " + releaseVelocity);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceTreshold);
    }
    private void callFunction()
    {
        Debug.Log("Pressing the cube");
    }
}
