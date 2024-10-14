using UnityEngine;

public class PlayerCollisionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var targetScript = other.GetComponent<FirstPersonCharacterController>();

            if (targetScript != null)
            {
                targetScript.canGrapple = true;
                Destroy(gameObject);
            }
        }
    }
}
