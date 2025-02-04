using UnityEngine;

public class CubeScrollTest : Interactable
{
    [SerializeField] private float f1, f2, f3;
    private Rigidbody rb;
    public bool isScrolling = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected override void StartInteract(int scroll)
    {
        Debug.Log("Scrolling the cube");
        MainInteraction(scroll);
    }
    void MainInteraction(int scroll) {
        f2 = scroll * f3;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - f2, this.transform.position.z);
    }
}
