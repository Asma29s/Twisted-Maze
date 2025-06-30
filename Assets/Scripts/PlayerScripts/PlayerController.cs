using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    

    [Header("--- Movement & Physics ---")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    private Vector3 velocity;
    private bool isGrounded;

 
    [Header("--- Crouch Settings ---")]
    public KeyCode crouchKey = KeyCode.LeftControl; 
    public float standingHeight = 2.0f;
    public float crouchingHeight = 1.0f;

    [Header("--- Speed Settings ---")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    private float currentSpeed;
    private bool isCrouching = false;
    public KeyCode sprintKey = KeyCode.LeftShift;




    [Header("Components")]
    public CharacterController controller;
    public Transform cameraTransform;
    public Image StaminaBarUI;
    public GameObject staminaOjbectUI;
   
    [Header("Stamina Settings")]
    public float stamina = 50f;
    public float maxStamina = 50f;
    public float Stamina_drainRate = 10f;
    public float Stamina_RechargeRate = 10f; // Recharge
    bool Stamina_isFatigued; // 1) wouldn't allow player to sprint. 2) true when timer less than 10s
    private Coroutine rechargeCR;
    bool isRunning;





    void Start()
    {
        // Ensure required components are assigned
        if (controller == null)
            controller = GetComponent<CharacterController>();


        currentSpeed = walkSpeed;
        // currentStamina = maxStamina;
    }

    void Update()
    {
        HandleSprinting();
        HandleMovement();
        HandleCrouching();
        HandleJump();
        ApplyGravity();
        UpdateStamina();
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
        //animator.SetFloat("xVelocity", Mathf.Abs(x) + Mathf.Abs(z));
    }

    void HandleCrouching()
    {
        if (Input.GetKeyDown(crouchKey))
        {

            // Change CharacterController height
            controller.height = isCrouching ? crouchingHeight : standingHeight;
            isCrouching = !isCrouching; // Toggle crouch

            // Adjust center if needed to avoid clipping
            controller.center = new Vector3(0, controller.height / 2f, 0);

            // Optional: Set crouch animation
            //animator.SetBool("isCrouching", isCrouching);
        }
    }


    // Handles sprinting input
    void HandleSprinting()
    {
        if (Input.GetKey(sprintKey) && !Stamina_isFatigued)
        {
            currentSpeed = sprintSpeed;
            isRunning = true;
        }
        else if (isCrouching)
            currentSpeed = crouchSpeed;
        
        if (Input.GetKeyUp(sprintKey)) 
        { 
            isRunning = false;        // will stop the stamina from decreasing 
            currentSpeed = walkSpeed;
        }


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

    void UpdateStamina()
    {
        if (isRunning)
        {
            staminaOjbectUI.SetActive(true);
            // start lowering the stamina based on time 
            stamina -= Time.deltaTime * Stamina_drainRate;
            if (stamina < 0) stamina = 0;
            StaminaBarUI.fillAmount = stamina / maxStamina; // UI effect: drain

            if (stamina <= 0) 
            {
                if (rechargeCR != null) StopCoroutine(rechargeCR); // if recharge is working, stop it and start a new one
                rechargeCR = StartCoroutine(RechargeStamina());
            }
        }
        else if (stamina >= maxStamina)
        {
            staminaOjbectUI.SetActive(false);
        }


    }

    private IEnumerator RechargeStamina()
    {
        Stamina_isFatigued = true; 
        yield return new WaitForSeconds(3f); //fixed delay is better than Stamina_fatigueTimer apperantly
        // is it going to wait then excute or wut
        // TODO : add an effect warning for 3 seconds 
        while (stamina < maxStamina)
        {
            stamina += Stamina_RechargeRate / 10f;
            if (stamina > maxStamina) 
                { stamina = maxStamina; } 
            StaminaBarUI.fillAmount = stamina / maxStamina;

            yield return new WaitForSeconds(0.1f); // try to set null idk

        }

        Stamina_isFatigued = false; // reset after done recharge, player will be able to sprint again

    }


}
