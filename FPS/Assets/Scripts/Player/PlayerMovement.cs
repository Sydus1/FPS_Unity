using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController; 
    
    public float speed = 10f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;

    bool isGrounded;

    public float jumpHeigth = 2f;

    Vector3 velocity;

    // Sprint
    public bool isSprinting;
    public float sprintingSpeedMultiplier = 2f;
    private float sprintSpeed = 1f;

    // Stamina
    public float staminaUseAmount = 5;
    private StaminaBar staminaSlider;

    public Animator animator;


    private void Start()
    {
        staminaSlider = FindAnyObjectByType<StaminaBar>();
    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelZ", z);

        Vector3 move = transform.right * x + transform.forward * z;

        JumpCheck();

        RunCheck();

        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime * sprintSpeed);
    }

    public void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigth * -2 * gravity);
        }
    }


    public void RunCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;

            if (isSprinting == true)
            {
                staminaSlider.UseStamina(staminaUseAmount);
            }

            else
            {
                staminaSlider.UseStamina(0);
            }
        }

        if (isSprinting == true)
        {
            sprintSpeed = 15;
            speed = sprintSpeed;
        }

        else
        {
            sprintSpeed = 10;
            speed = sprintSpeed;
        }
    }
}
