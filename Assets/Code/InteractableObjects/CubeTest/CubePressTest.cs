using UnityEngine;

public class CubePressTest : Interactable
{
    [SerializeField] private float f1, f2, f3;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected override void StartInteract()
    {
        Debug.Log("Pressing the cube");
        rb.AddForce(transform.up * f1, ForceMode.Impulse);
    }
    protected override void StopInteract()
    {
        Debug.Log("Releasing the cube");
    }
}
