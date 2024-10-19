using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCharacterController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float sprintMultiplier = 2f;
    public float jumpSpeed = 12f;
    public float gravity = 30f;
    public float stamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRechargeRate = 0.5f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public GameObject snowballPrefab;
    public float snowballForce = 500f;
    public KeyCode throwSnowballKey = KeyCode.S;

    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float ySpeed = 0f;
    private float rotationX = 0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            return;
        }

        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleSnowballThrowing();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * 2f; // Use the same look speed as before
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX * 2f, 0);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);
        moveDirection = (forward * moveZ + right * moveX).normalized * moveSpeed;

        if (characterController.isGrounded)
        {
            ySpeed = -gravity * Time.deltaTime;
        }
        else
        {
            ySpeed -= gravity * Time.deltaTime;
        }

        moveDirection.y = ySpeed;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (characterController.isGrounded && Input.GetKeyDown(jumpKey))
        {
            ySpeed = jumpSpeed;
        }
    }

    private void HandleSnowballThrowing()
    {
        if (Input.GetKeyDown(throwSnowballKey) && snowballPrefab != null)
        {
            GameObject snowball = Instantiate(snowballPrefab, playerCamera.transform.position + playerCamera.transform.forward, Quaternion.identity);
            Rigidbody rb = snowball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(playerCamera.transform.forward * snowballForce);
            }
        }
    }
}