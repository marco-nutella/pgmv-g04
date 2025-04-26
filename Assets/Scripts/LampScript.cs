using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScript : MonoBehaviour
{

    
    public GameObject Lamp;
    private bool isLightOn = false;
    private Collider lampCollider; 

    void Start()
    {
        
        //Encontrar o candeeiro na scene
        Lamp = GameObject.Find("Lamp");
        if (Lamp == null)
        {
            Debug.LogError("Lamp object not found in the scene.");
            return;
        }
        // lampCollider = Lamp.GetComponent<Collider>();
        // if (lampCollider == null)
        // {
        //     Debug.LogError("O objeto Lamp não possui um Collider. Adicione um Collider (ex: Box Collider) ao objeto.");
        //     enabled = false; 
        //     return;
        // }

        Lamp.SetActive(isLightOn);

    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // 0 é lado esquerdo do rato
        {
            // Check if the mouse is over the lamp
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if (hit.transform.gameObject == Lamp)
                // {
                //     isLightOn = !isLightOn;
                //     Lamp.SetActive(isLightOn);
                // }
                if (hit.collider == lampCollider)
                {
                    isLightOn = !isLightOn;
                    Lamp.SetActive(isLightOn);
                }
            }
        }
        // if (myLight != null) 
        // {
        //     isLightOn = !isLightOn; 
        //     myLight.enabled = isLightOn; // Turn the light on or off
        //     // GameObject.SetActive(isLightOn); to toggle the whole GameObject if the light is a child object
        // }

    }
}

