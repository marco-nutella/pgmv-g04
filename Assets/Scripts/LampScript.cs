using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScript : MonoBehaviour
{
    // Start is called before the first frame update

    Light myLight;
    private bool isLightOn = false;

    void Start()
    {
        myLight = GetComponent<Light>();

    }

    // Update is called once per frame
    void OnMouseDown()
    {
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     myLight.enabled = !myLight.enabled;
        // }
        if (myLight != null) 
        {
            isLightOn = !isLightOn; 
            myLight.enabled = isLightOn; // Turn the light on or off
            // use GameObject.SetActive(isLightOn); to toggle the whole GameObject if the light is a child object
        }

    }
}
