using UnityEngine;

public class TogglePanelsOnKeyPress : MonoBehaviour
{
    public GameObject panel;
    public GameObject panel2;
    public KeyCode toggleKey = KeyCode.X;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        if (panel != null)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                if (panel2 != null)
                {
                    panel2.SetActive(true);
                }
            }
            else
            {
                panel.SetActive(true);
                if (panel2 != null)
                {
                    panel2.SetActive(false);
                }
            }
        }
    }
}
