using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    public LineRenderer lineRenderer; // component to render the grappling line
    public LayerMask grappleableLayer; // layer for grappling objects

    private bool isGrappling = false; // indicates if the player is currently grappling
    private Vector3 grapplePoint; // point where the player is grappling
    private float grappleDuration = 2f; // duration for maximum distance
    private float grappleStartTime; // time when the grapple started
    private float maxGrappleDistance = 10f; // maximum distance for grappling

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) StartGrapple();
        else if (Input.GetButtonUp("Fire1")) StopGrapple();

        if (isGrappling) UpdateGrapple();
    }

    void StartGrapple()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, grappleableLayer))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);

            float distance = Vector3.Distance(transform.position, grapplePoint);
            grappleDuration = Mathf.Clamp(distance / maxGrappleDistance * 2f, 1f, 5f);
            grappleStartTime = Time.time;
        }
    }

    void UpdateGrapple()
    {
        float elapsed = Time.time - grappleStartTime;
        if (elapsed < grappleDuration)
        {
            float t = elapsed / grappleDuration;
            transform.position = Vector3.Lerp(transform.position, grapplePoint, t);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
        else StopGrapple();
    }

    void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
    }
}