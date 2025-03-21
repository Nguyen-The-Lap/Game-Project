


using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    // advance 
    [Header("SPRINT")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public KeyCode sprintKey = KeyCode.LeftShift;


    [Header("CROUCH")]
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;
    public KeyCode crouchKey = KeyCode.LeftControl;


    public MovementState state;
    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        air
    }

    public float StateHandler()
    {
        // mode crouching
        if (Input.GetKey(crouchKey)) { 
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // mode sprint
        if (isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;

        }
        // mode walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // air
        else
        {
            state = MovementState.air;
        }
        return moveSpeed;

    }

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);


    void Start()
    {
        controller = GetComponent<CharacterController>();
        startYScale = transform.localScale.y;

    }

    void Update()
    {


        // advanced mu mần
        StateHandler();
        speed = StateHandler();


        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // reseting the default V
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        //input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        // moving vector
        Vector3 move = transform.right * x + transform.forward * z;

        // movement of player
        controller.Move(move * speed * Time.deltaTime);


        //  calculate velocity and jump height
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // falling down
        velocity.y += gravity * Time.deltaTime;

        // Exectuting the jump 
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }

        else
        {
            isMoving = false;
        }
        lastPosition = gameObject.transform.position;


        // CROUCHING
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            //velocity.y += -5f * Time.deltaTime; // Tăng lực rơi xuống
            //controller.Move(velocity * Time.deltaTime);
            //velocity.y += gravity * Time.deltaTime;

        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey)) {

            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        }
    }


}           
