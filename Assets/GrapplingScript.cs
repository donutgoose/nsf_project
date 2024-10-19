using UnityEngine;
using System.Collections;

public class PlayerGrapple : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxGrappleDistance = 10f;
    public int teleportCount = 100;
    public float timePerMove = 0.05f;

    private Coroutine grappleCoroutine;
    private bool isGrappling = false;
    private Vector3 grapplePoint;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) StartGrapple();
        if (Input.GetButtonUp("Fire1")) StopGrapple();

        if (isGrappling) UpdateGrapple();
    }

    void StartGrapple()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance))
        {
            grapplePoint = hit.point;

            if (grappleCoroutine != null)
            {
                StopCoroutine(grappleCoroutine);
            }

            isGrappling = true;

            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grapplePoint);
            }

            grappleCoroutine = StartCoroutine(MoveToGrapplePoint());
        }
        else
        {
            StopGrapple();
        }
    }

    void UpdateGrapple()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
        }
    }

    private IEnumerator MoveToGrapplePoint()
    {
        Vector3 startPosition = transform.position;
        float totalDistance = Vector3.Distance(startPosition, grapplePoint);
        float elapsedTime = 0f;
        float duration = totalDistance / (teleportCount * timePerMove);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, grapplePoint, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = grapplePoint;
        StopGrapple();
    }

    void StopGrapple()
    {
        isGrappling = false;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
        characterController.enabled = true;
    }
}
