using System;
using Unity.Mathematics;
using UnityEngine;

public class CubeHoldTest : Interactable
{
    [SerializeField] private float f1, f2, f3;
    private Rigidbody rb;
    public bool isHolding = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected override void StartInteract()
    {
        Debug.Log("Pressing the cube");
        isHolding = true;
    }
    protected override void StopInteract()
    {
        Debug.Log("Releasing the cube");
        isHolding = false;
        
    }
    private void FixedUpdate()
    {
        if (isHolding)
        {
            // Math.Lerp
        }
    }

}
