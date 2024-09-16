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
    public bool enableSprinting = true;

    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;
    public float upperLookLimit = -80f;
    public float lowerLookLimit = 80f;

    public GameObject snowballPrefab;  
    public float snowballForce = 500f;  
    public KeyCode throwSnowballKey = KeyCode.S;  

    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float ySpeed = 0f;
    private float rotationX = 0f;
    private float currentStamina;

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

        currentStamina = stamina;
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

        rotationX -= mouseY * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, upperLookLimit, lowerLookLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX * lookSpeedX, 0);
    }

    private void HandleMovement()
    {
        float moveDirectionX = Input.GetAxis("Horizontal") + (Input.GetKey(KeyCode.RightArrow) ? 1f : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1f : 0);
        float moveDirectionZ = Input.GetAxis("Vertical") + (Input.GetKey(KeyCode.UpArrow) ? 1f : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1f : 0);

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 move = (forward * moveDirectionZ + right * moveDirectionX).normalized;

        float effectiveMoveSpeed = moveSpeed;
        if (enableSprinting && Input.GetKey(sprintKey) && currentStamina > 0)
        {
            effectiveMoveSpeed *= sprintMultiplier;
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            currentStamina = Mathf.Min(stamina, currentStamina + staminaRechargeRate * Time.deltaTime);
        }

        move *= effectiveMoveSpeed;

        if (characterController.isGrounded)
        {
            ySpeed = -0.1f;
        }
        else
        {
            ySpeed -= gravity * Time.deltaTime;
        }

        moveDirection = new Vector3(move.x, ySpeed, move.z);

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                ySpeed = jumpSpeed;
            }
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
