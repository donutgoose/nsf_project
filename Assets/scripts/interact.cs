using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMpro;

public class interact : MonoBehaviour
{
    public LayerMask interactLayer;
    public Collider interactableObject;
    private interact_recieve receptive;
    [SerializeField] private TextMeshProUGUI indicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactableObject != null && Input.GetKeyDown(KeyCode.F))
        {
            receptive = interactableObject.GetComponent<interact_recieve>();
            if (receptive != null)
            {
                receptive.run();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (true)
        {
            interactableObject = other;

        }
        indicator.setActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        interactableObject = null;
        indicator.setActive(false);
    }
}
