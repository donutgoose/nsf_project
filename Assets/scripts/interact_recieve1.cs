using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_recieve : MonoBehaviour
{
    public GameObject Skibidi;
    public GameObject Skibidi2;
    ///public interact interactEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void run()
    {
        Skibidi.SetActive(false);
        //Skibidi2.SetActive(false);
        Debug.Log("SKibidi");
    }
}
