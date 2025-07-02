using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerController myPlayerController;


    // unity methods:
    void OnValidate()
    {
        if (myPlayerController == null)
            myPlayerController = GetComponent<PlayerController>();
        //animator = GetComponent<Animator>();
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //
    // input methods:

    void OnMove(InputValue inputValue)
    {
        myPlayerController.MoveInput = inputValue.Get<Vector2>();
    }
    void OnLook(InputValue inputValue)
    {
        myPlayerController.LookInput = inputValue.Get<Vector2>();
    }
    void OnSprint(InputValue inputValue)
    {
        myPlayerController.sprintInput = inputValue.isPressed;
    }
    void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
            myPlayerController.TryJump();
    }
    void OnCrouch(InputValue inputValue)
    {
        if (inputValue.isPressed)
            myPlayerController.TryCrouching();
    }



}
