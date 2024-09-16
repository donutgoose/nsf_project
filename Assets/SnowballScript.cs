using UnityEngine;
using System.Collections; 

public class SnowballFade : MonoBehaviour
{
    public float delayBeforeDestruction = 1.5f; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDestruction);

        Destroy(gameObject);
    }
}
