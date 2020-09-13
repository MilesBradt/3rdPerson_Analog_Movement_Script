using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Most of this code is from Brackeys on YouTube
public class ThirdPersonMovement : MonoBehaviour
   
{
    public CharacterController controller;
    public Transform cam;

    public float speed;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float speedMultiplier = 10f;
    public float groundDistance = 0.4f;

    public Transform groundCheck;
    public LayerMask groundMask;

    float turnSmoothVelocity;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Debug.Log("horizontal:" + horizontal);
        Debug.Log("vertical:" + vertical);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {   
            // Modified code
            float verticalConvert = Mathf.Abs(vertical);
            float horizontalConvert = Mathf.Abs(horizontal);

            speed = ((verticalConvert + horizontalConvert) * speedMultiplier);

            // Stops max speed from going above the highest speed value if vertical or horizontal is fully pushed
            if(speed >= speedMultiplier)
                {
                    speed = speedMultiplier;
                }

            Debug.Log(speed);
            // Brackeys' code continued
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
