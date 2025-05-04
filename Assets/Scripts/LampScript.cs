using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScript : UtilityScript
{
    // Start is called before the first frame update

    public GameObject Lamp;
    private bool isLightOn = false;

    void Start()
    {
        
        // Encontrar o candeeiro na scene
        Lamp = GameObject.Find("Lamp");
        if (Lamp == null)
        {
            Debug.LogError("Lamp object not found in the scene.");
            return;
        }

        // Light myLight = Lamp.GetComponent<Light>();
        // if (myLight == null)
        // {
        //     Debug.LogError("No Light component found on the lamp object.");
        //     return;
        // }

    }

    void Update()
    {
        // Check for mouse click
        if (OnClickActivation(Lamp))
        {
            isLightOn = !isLightOn;
            Lamp.SetActive(isLightOn);
        }
        // if (myLight != null) 
        // {
        //     isLightOn = !isLightOn; 
        //     myLight.enabled = isLightOn; // Turn the light on or off
        //     // GameObject.SetActive(isLightOn); to toggle the whole GameObject if the light is a child object
        // }

    }
}

