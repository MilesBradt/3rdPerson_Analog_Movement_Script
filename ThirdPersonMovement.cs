using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Most of this code is from Brackeys on YouTube
public class ThirdPersonMovement : MonoBehaviour

{
    Animator anim;
    Vector3 velocity;
    public CharacterController controller;
    public Transform cam;
   
    public float speed;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float walkingSpeed = 3f;
    public float runningSpeed = 12f;
    public float groundDistance = 0.4f;

    public Transform groundCheck;
    public LayerMask groundMask;

    float turnSmoothVelocity;

    bool isGrounded;
    public bool isRunning = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
       
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude < 0.1f)
        {
            MovementTypeForAnimations("idle");
        }

        if (direction.magnitude >= 0.1f)
        {
            
            float verticalConvert = Mathf.Abs(vertical);
            float horizontalConvert = Mathf.Abs(horizontal);

            if (isRunning == false && isGrounded)
            {
                MovementTypeForAnimations("walking");
                speed = ((verticalConvert + horizontalConvert) * walkingSpeed);
                
                if (speed >= walkingSpeed)
                {
                    speed = walkingSpeed;
                }
            }

            if (Input.GetButtonDown("Run") && isGrounded)
            {
                MovementTypeForAnimations("running");
                isRunning = true;
                speed = runningSpeed;
            }
            else if (Input.GetButtonUp("Run"))
            {
                isRunning = false;
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void MovementTypeForAnimations(string type)
    {

        if (type == "idle")
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }
        if (type == "walking")
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Walking", true);
            anim.SetBool("Running", false);
        }
        if (type == "running")
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
        }
    }

}