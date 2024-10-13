using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Grapple range of 0 is unlimited
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
    public KeyCode grappleKey = KeyCode.X;
    public GameObject grappleMarkerPrefab;
    public float grappleSpeed = 5f;
    public float grappleSegmentDistance = 1f;
    public float grappleRange = 0f;

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
    private List<Vector3> grapplePoints = new List<Vector3>();
    private List<GameObject> grappleMarkers = new List<GameObject>();
    private int currentGrappleIndex = 0;
    private bool isGrappling = false;

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
        HandleGrapple();
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
        if (isGrappling) return;

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
        if (characterController.isGrounded && Input.GetKeyDown(jumpKey) && !isGrappling)
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

    private void HandleGrapple()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            if (grapplePoints.Count > 0)
            {
                grapplePoints.Clear();
                foreach (var marker in grappleMarkers)
                {
                    Destroy(marker);
                }
                grappleMarkers.Clear();
            }

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, hit.point);
                if (grappleRange > 0 && distance > grappleRange)
                {
                    distance = grappleRange;
                }

                int segments = Mathf.FloorToInt(distance / grappleSegmentDistance);

                for (int i = 0; i <= segments; i++)
                {
                    Vector3 grapplePoint = transform.position + direction * (i * grappleSegmentDistance);
                    grapplePoints.Add(grapplePoint);
                    GameObject marker = Instantiate(grappleMarkerPrefab, grapplePoint, Quaternion.identity);
                    grappleMarkers.Add(marker);
                }

                currentGrappleIndex = 0;
                isGrappling = true;
                StartCoroutine(MoveAlongGrapple());
            }
        }
    }

    private IEnumerator MoveAlongGrapple()
    {
        while (currentGrappleIndex < grapplePoints.Count)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = grapplePoints[currentGrappleIndex];
            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float journeyProgress = 0f;

            while (journeyProgress < journeyLength)
            {
                journeyProgress += grappleSpeed * Time.deltaTime;
                float t = journeyProgress / journeyLength;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t * t);
                yield return null;
            }

            Destroy(grappleMarkers[currentGrappleIndex]);
            grappleMarkers[currentGrappleIndex] = null;

            currentGrappleIndex++;
        }

        isGrappling = false;
    }
}
