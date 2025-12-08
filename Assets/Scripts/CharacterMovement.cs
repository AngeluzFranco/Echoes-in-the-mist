
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public Transform cameraTransform;

    public float movementSpeed = 5f;
    private float currentSpeed;
    private bool isCrouching = false;
    private CharacterController controller;
    public float gravity;
    public float jumpForce = 7f;
    private bool isJumping = false;

    public Animator animator;
    public readonly int movementSpeedHash = Animator.StringToHash("MovementSpeed");
        private bool wasGrounded = true;

     void Start()
    {
    controller = GetComponent<CharacterController>();
    currentSpeed = movementSpeed;
        
    }

    void Update()
    {
        // Agacharse con CTRL izquierdo
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            currentSpeed = movementSpeed / 2f;
            animator.CrossFadeInFixedTime("Agachado", 0.1f);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            currentSpeed = movementSpeed;
            animator.CrossFadeInFixedTime("Blend Tree", 0.1f);
        }

        if (controller.isGrounded)
        {
            gravity = Physics.gravity.y * Time.deltaTime;
            if (!wasGrounded)
            {
                // El personaje acaba de aterrizar
                animator.CrossFadeInFixedTime("Blend Tree", 0.1f); // Vuelve al Blend Tree
                isJumping = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravity = jumpForce;
                isJumping = true;
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            }
        }
        else
        {
            gravity += Physics.gravity.y * Time.deltaTime;
        }
        wasGrounded = controller.isGrounded;

        var gravityVector = Vector3.up * gravity;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
        var cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z);
        var direction = cameraForward * vertical + cameraRight * horizontal;
    controller.Move((direction.normalized + gravityVector)  *(currentSpeed * Time.deltaTime));

        if (direction != Vector3.zero)
        { 
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        animator.SetFloat(movementSpeedHash, direction.normalized.magnitude);
    }
}
