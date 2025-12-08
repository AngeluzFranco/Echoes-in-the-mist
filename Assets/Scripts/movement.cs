using UnityEngine;

public class movement : MonoBehaviour
{

    public float speed = 5f;
    public Transform cameraTransform;   
    
    void Start()
    {

    }

    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;
        Vector3 movement = cameraForward * vertical + cameraRight * horizontal;


        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }

        transform.position += movement * (Time.deltaTime * speed);
        
    }
}