using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Transform cameraTransform;
    public float cameraRotationSpeed = 80f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    private float currentVerticalAngle = 0f;
    public bool isInverted;
    
    void Start()
    {
        offset = new Vector3(0, 2, -5);
        Cursor.lockState = CursorLockMode.Locked;

    }

        
    void Update()
    {
        // Rotación horizontal y vertical con el mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        var sign = isInverted ? -1 : 1;
        float rotationDeltaX = mouseX * Time.deltaTime * cameraRotationSpeed * sign;
        float rotationDeltaY = mouseY * Time.deltaTime * cameraRotationSpeed;

        // Rotar horizontalmente el target (personaje)
        transform.Rotate(0, rotationDeltaX, 0f);

        // Rotar verticalmente la cámara (limitada)
        currentVerticalAngle += rotationDeltaY;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        Quaternion verticalRotation = Quaternion.Euler(currentVerticalAngle, 0, 0);

        // Calcular posición deseada de la cámara
        Vector3 desiredCameraPos = target.position + verticalRotation * (transform.rotation * offset);

        // Raycast para evitar atravesar objetos
        RaycastHit hit;
        Vector3 directionToCamera = (desiredCameraPos - target.position).normalized;
        float distance = offset.magnitude;
        if (Physics.Raycast(target.position, directionToCamera, out hit, distance))
        {
            cameraTransform.position = hit.point - directionToCamera * 0.2f; // Un poco antes del obstáculo
        }
        else
        {
            cameraTransform.position = desiredCameraPos;
        }
        cameraTransform.LookAt(target.position + Vector3.up * 1.5f); // Enfoca siempre al jugador
    }
}