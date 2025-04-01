using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 3f;
    private float moveSpeed;

    [Header("Key Bindings")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Crouch Settings")]
    public float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Jump & Gravity")]
    public float jumpHeight = 3f;
    public float gravity = -19.62f;
    
    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    private Vector3 velocity;
    private bool isGrounded;
    
    private MovementState state;
    private enum MovementState { Walking, Crouching, Sprinting, Air }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        GroundCheck();
        HandleMovement();
        HandleJumping();
        ApplyGravity();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        moveSpeed = GetCurrentSpeed();
        
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(crouchKey))
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        else if (Input.GetKeyUp(crouchKey))
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }

    private float GetCurrentSpeed()
    {
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.Crouching;
            return crouchSpeed;
        }
        if (isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.Sprinting;
            return sprintSpeed;
        }
        if (isGrounded)
        {
            state = MovementState.Walking;
            return walkSpeed;
        }
        state = MovementState.Air;
        return walkSpeed; // Default air movement speed
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
