using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("--- Crouch Settings ---")]
    public float standingHeight = 2.0f;
    public float crouchingHeight = 1.0f;

    [Header("--- Speed Settings ---")]
    [SerializeField] float walkSpeed = 13f;
    [SerializeField] float sprintSpeed = 26f;
    public float crouchSpeed = 2.5f; //////
    private float currentSpeed; //same as max
    private bool isCrouching = false; ///////

    [Header("Movement parameters")]
    public float maxSpeed => sprintInput && !Stamina_isFatigued ? sprintSpeed : walkSpeed;
    public float acceleration = 15f;
    // crouch 


    [Header("--- Components ---")]
    public CharacterController controller;
    public Transform cameraTransform;
    [SerializeField] Camera FPCamera;
    public UIManager UIManager;
    public JumpScare jumpScare; 

    [Header("UI Elements")]
    public Image StaminaBarUI;
    public GameObject staminaOjbectUI;

    [Header("Stamina Settings")]
    public float stamina = 50f;
    public float maxStamina = 50f;

    public float Stamina_drainRate = 1f;
    public float Stamina_RechargeRate = 1f; // Recharge

    bool Stamina_isFatigued; // 1) wouldn't allow player to sprint. 2) true when timer less than 10s

    private Coroutine rechargeCR;



    //bool isRunning;
    public bool isSprinting // 
    { // this for camera FOV, need it with stamina too 
        get
        {
            return sprintInput && CurrentSpeed > 0.1f && stamina != 0 ;
            // isSprinting true: sprintInput is pressed + moving + less than the period start + finish return false 
        }
    }

    [Header("Looking parameters")]
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
    public float pitchLimit = 80f; //lower it if wanted
    [SerializeField] float currentPitch = 0f;
    public float CurrentPitch
    {
        get => currentPitch;
        set
        {
            currentPitch = Math.Clamp(value, -pitchLimit, pitchLimit);
        }
    }

    [Header("Inputs")] // will be set from playerInputs scripts 
    public Vector2 MoveInput;
    public Vector2 LookInput;
    public bool sprintInput;

    [Header("Camera parameters")]
    [SerializeField] float CameraNormalFOV = 60f;
    [SerializeField] float CameraSprintFOV = 80f;
    [SerializeField] float CameraFOVSmoothing = 3f; // change it when the FOV of the sprinting feels a lil snappy

    float targetCameraFOV
    {
        get { return isSprinting ? CameraSprintFOV : CameraNormalFOV; }
    }


    [Header("Physics parameters")]
    [SerializeField] float GravityScale = 3f;
    [SerializeField] float VerticalVelocity = 0f;
    public Vector3 currentVelocity; //{ get; private set; }
    public float CurrentSpeed;

    public bool isGrounded => controller.isGrounded;
    [Space(15)]
    [Tooltip("this is how high!")]
    [SerializeField] float jumpHeight = 1.5f;



    void Start()
    {
        currentSpeed = walkSpeed;
        controller.height = standingHeight;
    }

    void OnValidate()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
        if (UIManager == null)
            UIManager = GetComponent<UIManager>();
    }

    void Update()
    {
        //HandleSprinting();
        //HandleMovement();
        MoveUpdate();
        //LookUpdate();
        if (UIManager.isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            LookUpdate(); // only call LookUpdate when not paused
        }
        CameraUpdate();
       // HandleCrouching();
        //HandleJump();
        //ApplyGravity();
        UpdateStamina();
        UpdatePause();
    }

    public void TryJump()
    {
        if (!isGrounded)
        {
            return;
        }
        VerticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * GravityScale); //jump force 
    }

    void MoveUpdate()
    {
        // read 
        Vector3 motion = transform.forward * MoveInput.y + transform.right * MoveInput.x; // reading values
        motion.y = 0f; // so all the y movement happens in plane ofc
        motion.Normalize();
        // read to check
        if (motion.sqrMagnitude >= 0.01f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, motion * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }


        // for jumping
        if (isGrounded && VerticalVelocity <= 0.01f)
        {
            VerticalVelocity = -3f; // to keep character stuck to the ground (imp for climing staris ig)
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
        }

        Vector3 fullVelocity = new Vector3(currentVelocity.x, VerticalVelocity, currentVelocity.z);

        // move 
        controller.Move(fullVelocity * Time.deltaTime);

        // update
        CurrentSpeed = currentVelocity.magnitude;

    }
    void LookUpdate()
    {
        if (!UIManager.isPaused && !EventSystem.current.IsPointerOverGameObject()) { 
        Vector2 lookInputs = new Vector2(LookInput.x * lookSensitivity.x, LookInput.y * lookSensitivity.y);
        // up and down:
        CurrentPitch -= lookInputs.y;

        FPCamera.transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);

        // left and right:
        transform.Rotate(Vector3.up * lookInputs.x);
    }
    }
    void CameraUpdate()
    {
        // changing the lens of the camera to a target FOV in a specified time period
        FPCamera.fieldOfView = Mathf.Lerp(FPCamera.fieldOfView, targetCameraFOV, CameraFOVSmoothing * Time.deltaTime);
    }

   

    public void TryCrouching()
    {
        if (!isGrounded)
        {
            return;
        }
        // Change CharacterController height
        controller.height = isCrouching ? crouchingHeight : standingHeight;
        isCrouching = !isCrouching; // Toggle crouch

        // Adjust center if needed to avoid clipping
        controller.center = new Vector3(0, controller.height / 2f, 0);
    }

    void UpdateStamina()
    {
        if (isSprinting)
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
        else if (stamina >= maxStamina) // stamina is full
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

    public void UpdatePause() {    
         
        if (Input.GetKeyUp(KeyCode.Escape))
             {
            UIManager.ActivatePauseUI();
            }
    
    }


    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyChase enemy = collision.gameObject.GetComponent<EnemyChase>(); // this for enemy animation 
            // if no animation and just an image and a sound then leave it like this and change image
            if (enemy != null)
            {
                //1. animation
                jumpScare.TriggerJumpScare();
                // timer till animation/sound is done
                StartCoroutine(HandleJumpScare(enemy)); // will set ui active when timer is up

            }
        }
    }

    private IEnumerator HandleJumpScare(EnemyChase enemy)
    {
        // a wait for the enemy's jump scare *animation* to finish or a sound to play ( you choose)
        yield return new WaitForSeconds(1f); //enemy.GetJumpScareDuration()

        // activate death UI
        UIManager.ActivateDeathUI();
    }



}
