using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public CharacterController characterController; 
    
    public float initialSpeed = 5f;
    public float speed = 5f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;

    bool isGrounded;

    public float jumpHeigth = 2f;

    Vector3 velocity;

    // Sprint
    public bool isSprinting = false;
    public float sprintingSpeedMultiplier = 2f;
    private float sprintSpeed = 10f;

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
        if (photonView.IsMine)
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
            animator.SetBool("isSprinting", isSprinting);

            Vector3 move = transform.right * x + transform.forward * z;

            JumpCheck();

            RunCheck();

            characterController.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime * sprintSpeed);
        }
    }

    public void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigth * -2 * gravity);

            animator.SetBool("isJumping", true);
        }

        if (!isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }


    public void RunCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprintSpeed = 10f;
            isSprinting = true;
            staminaSlider.UseStamina(staminaUseAmount);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintSpeed = initialSpeed; // Restablecer la velocidad inicial
            isSprinting = false;
            staminaSlider.UseStamina(0);
        }

        if (isSprinting)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = initialSpeed; // Usar la velocidad inicial si no se est� sprintando
        }
    }
}
