using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontal, 0f, vertical).normalized;
        rb.AddForce(direction * (Time.deltaTime * jumpForce), ForceMode.VelocityChange);
    }
}
