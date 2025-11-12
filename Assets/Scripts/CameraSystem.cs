using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Transform cameraTransform;
    public float cameraRotationSpeed = 50f;
    public bool isInverted;
    
    void Start()
    {
        offset = new Vector3(0, 2, -5);
        Cursor.lockState = CursorLockMode.Locked;

    }

        
    void Update()
    {
        Vector3 direction = (target.position - cameraTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        cameraTransform.rotation = targetRotation;
        transform.position = target.position;
        cameraTransform.localPosition = offset;

        var rotationDelta = Input.mousePositionDelta.x * Time.deltaTime * cameraRotationSpeed;
        var sign = isInverted ? -1 : 1;
        transform.Rotate(0, rotationDelta, 0f);
    }
}