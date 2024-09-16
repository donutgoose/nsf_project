using UnityEngine;

public class SnowballFade : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Renderer rend;
    private Color startColor;
    private bool isFading = false;
    private float fadeTime = 0f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            startColor = rend.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on snowball.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFading)
        {
            if (collision.gameObject.CompareTag("Ground")) 
            {
                isFading = true;
            }
        }
    }

    private void Update()
    {
        if (isFading)
        {
            FadeOut();
        }
    }

    private void FadeOut()
    {
        fadeTime += Time.deltaTime;
        float lerpValue = fadeTime / fadeDuration;
        Color newColor = Color.Lerp(startColor, Color.clear, lerpValue);

        if (rend != null)
        {
            rend.material.color = newColor;
        }

        if (lerpValue >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
