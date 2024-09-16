using UnityEngine;
using UnityEngine.UI; 

public class UIDistanceChecker : MonoBehaviour
{
    public Transform object1; 
    public Transform object2; 
    public float distanceThreshold = 10f; 
    public GameObject uiElement;

    private CanvasGroup canvasGroup; 

    private void Start()
    {
        if (uiElement != null)
        {
            canvasGroup = uiElement.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
 
                canvasGroup = uiElement.AddComponent<CanvasGroup>();
            }
        }
    }

    private void Update()
    {
        if (object1 != null && object2 != null && uiElement != null)
        {
            float distance = Vector3.Distance(object1.position, object2.position);
            if (distance > distanceThreshold)
            {
                SetUIVisibility(false);
            }
            else
            {
                SetUIVisibility(true);
            }
        }
    }

    private void SetUIVisibility(bool isVisible)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isVisible ? 1f : 0f;
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }
}
