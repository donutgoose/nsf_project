using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NPCInteraction : MonoBehaviour
{
    public float bubbleWidth = 200f;
    public float bubbleHeight = 100f;
    public float fontSize = 14f;
    public string[] dialogueSequence;

    private Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>
    {
        { "Green", Color.green },
        { "Violet", new Color(0.5f, 0, 0.5f) },
        { "Blue", Color.blue },
        { "Orange", new Color(1f, 0.647f, 0) },
        { "Purple", new Color(0.5f, 0, 0.5f) },
        { "Pink", Color.magenta },
        { "Amber", new Color(1f, 0.75f, 0) },
        { "Amethyst", new Color(0.6f, 0.4f, 0.8f) },
        { "White", Color.white },
        { "Black", Color.black },
        { "Aqua", Color.cyan },
        { "Gray", Color.gray },
        { "Beige", new Color(0.96f, 0.96f, 0.86f) },
        { "Bronze", new Color(0.8f, 0.5f, 0.2f) },
        { "Brown", new Color(0.65f, 0.16f, 0.16f) },
        { "Chocolate", new Color(0.82f, 0.41f, 0.12f) },
        { "Cobalt", new Color(0.0f, 0.28f, 0.67f) },
        { "Cyan", Color.cyan },
        { "Pearl", new Color(1f, 0.9f, 0.8f) },
        { "Grape", new Color(0.5f, 0.0f, 0.5f) },
        { "Magenta", Color.magenta },
        { "Turquoise", new Color(0.25f, 0.88f, 0.82f) },
        { "Emerald", new Color(0.0f, 0.5f, 0.5f) },
        { "Flame", new Color(1f, 0.5f, 0.0f) },
        { "Raspberry", new Color(0.9f, 0.3f, 0.5f) }
    };

    void Start()
    {
        if (dialogueSequence.Length == 0)
        {
            dialogueSequence = new string[]
            {
                "Hi!,1,White",
                "How are you?,1,Blue",
                "$hide,1",
                "$show,1",
                "I'm fine,2,Green"
            };
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                StartCoroutine(ShowDialogue());
            }
        }
    }

    IEnumerator ShowDialogue()
    {
        GameObject textObject = new GameObject("SpeechBubbleText");
        Canvas canvas = textObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform canvasRect = textObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(bubbleWidth, bubbleHeight);
        canvasRect.position = transform.position + Vector3.up * 1.5f;

        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.fontSize = fontSize;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.white;

        textObject.AddComponent<CanvasRenderer>();

        foreach (string dialogue in dialogueSequence)
        {
            string[] parts = dialogue.Split(',');
            if (parts.Length > 0)
            {
                string text = parts[0];
                float duration = parts.Length > 1 && float.TryParse(parts[1], out float tempDuration) ? tempDuration : 1f;
                Color textColor = Color.white;

                if (parts.Length > 2 && colorDictionary.TryGetValue(parts[2], out textColor))
                {
                    textComponent.color = textColor;
                }
                else
                {
                    textComponent.color = Color.white;
                }

                if (text == "$hide")
                {
                    textObject.SetActive(false);
                }
                else if (text == "$show")
                {
                    textObject.SetActive(true);
                }
             
                else
                {
                    textComponent.text = text;
                    textObject.SetActive(true);
                    yield return new WaitForSeconds(duration);
                    textObject.SetActive(false);
                }
            }
        }

        Destroy(textObject);
    }
}
