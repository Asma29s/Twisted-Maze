using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // --- Movement & Physics ---
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    private Vector3 velocity;
    private bool isGrounded;

    // --- Crouch Settings ---
    public KeyCode crouchKey = KeyCode.LeftControl; 
    public float standingHeight = 2.0f;
    public float crouchingHeight = 1.0f;

    // --- Speed Settings ---
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    private float currentSpeed;
    private bool isCrouching = false;
    public KeyCode sprintKey = KeyCode.LeftShift;

    // --- Components ---
    public CharacterController controller;
    public Transform cameraTransform;
    public Animator animator;

    void Start()
    {
        // Ensure required components are assigned
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();

        currentSpeed = walkSpeed;
    }

    void Update()
    {
        HandleSprinting();
        HandleMovement();
        HandleCrouching();
        HandleJump();
        ApplyGravity();
    }

    // Handles movement input and animation syncing
    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Use camera direction if needed
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        // Update animation blend parameter (assumes "xVelocity" is used in Animator)
        animator.SetFloat("xVelocity", Mathf.Abs(x) + Mathf.Abs(z));
    }

    void HandleCrouching()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching; // Toggle crouch

            // Change CharacterController height
            controller.height = isCrouching ? crouchingHeight : standingHeight;

            // Adjust center if needed to avoid clipping
            controller.center = new Vector3(0, controller.height / 2f, 0);

            // Optional: Set crouch animation
            animator.SetBool("isCrouching", isCrouching);
        }
    }


    // Handles sprinting input
    void HandleSprinting()
    {
        if (Input.GetKey(sprintKey))
            currentSpeed = sprintSpeed;
        else if (isCrouching)
            currentSpeed = crouchSpeed;
        else
            currentSpeed = walkSpeed;

        // Optional: Set animation bool here if needed
        // animator.SetBool("isSprinting", Input.GetKey(sprintKey));
    }

    // Handles jump input
    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    // Applies gravity every frame
    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
