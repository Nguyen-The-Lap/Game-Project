using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpPower = 7f;
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float crouchTransitionSpeed = 15f; 

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private bool isCurrentlyRunning = false; 
    private float targetHeight; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetHeight = defaultHeight;
    }

    void Update()
{
    Vector3 forward = transform.TransformDirection(Vector3.forward);
    Vector3 right = transform.TransformDirection(Vector3.right);

    bool isGrounded = characterController.isGrounded;

    if (Input.GetKey(KeyCode.LeftControl) && canMove)
    {
        targetHeight = crouchHeight;
    }
    else
    {
        targetHeight = defaultHeight;
    }

    characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

    bool isCrouching = Mathf.Abs(characterController.height - crouchHeight) < 0.1f;
    if (isCrouching)
    {
        isCurrentlyRunning = false;
    }

    if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching)
    {
        isCurrentlyRunning = true; 
    }
    else if (isGrounded)
    {
        isCurrentlyRunning = false; 
    }

    float currentSpeed = isCrouching ? crouchSpeed : (isCurrentlyRunning ? runSpeed : walkSpeed);
    float curSpeedX = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0;
    float curSpeedY = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;

    float movementDirectionY = moveDirection.y;

    if (isGrounded)
    {
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
    }
    else
    {
        Vector3 horizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
        Vector3 inputVelocity = (forward * curSpeedX) + (right * curSpeedY);

        if (inputVelocity != Vector3.zero)
        {
            horizontalVelocity = inputVelocity;
        }

        moveDirection = horizontalVelocity;
    }

    moveDirection.y = movementDirectionY;

    if (Input.GetButton("Jump") && canMove && isGrounded)
    {
        moveDirection.y = jumpPower;
    }

    if (!isGrounded)
    {
        moveDirection.y -= gravity * Time.deltaTime;
    }

    characterController.Move(moveDirection * Time.deltaTime);
    if (canMove)
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
  }
}
