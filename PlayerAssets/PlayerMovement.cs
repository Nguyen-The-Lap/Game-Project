using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 3f, runSpeed = 6f, jumpPower = 7f, gravity = 20f;
    public float lookSpeed = 2f, lookXLimit = 90f;
    public float defaultHeight = 2f, crouchHeight = 1f, crouchSpeed = 3f, crouchTransitionSpeed = 15f;

    private Vector3 moveDirection;
    private float rotationX, targetHeight;
    private CharacterController characterController;
    private bool canMove = true, isRunning, isCrouching;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetHeight = defaultHeight;
    }

    void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.forward, right = transform.right;
        bool isGrounded = characterController.isGrounded;

        targetHeight = Input.GetKey(KeyCode.LeftControl) && canMove ? crouchHeight : defaultHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        isCrouching = Mathf.Abs(characterController.height - crouchHeight) < 0.1f;
        isRunning = Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching;

        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        Vector3 inputVelocity = currentSpeed * (Input.GetAxis("Vertical") * forward + Input.GetAxis("Horizontal") * right);

        moveDirection = isGrounded ? inputVelocity : new Vector3(inputVelocity.x, moveDirection.y, inputVelocity.z);
        if (isGrounded && Input.GetButton("Jump") && canMove) moveDirection.y = jumpPower;
        if (!isGrounded) moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        if (!canMove) return;
        rotationX = Mathf.Clamp(rotationX - Input.GetAxis("Mouse Y") * lookSpeed, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
