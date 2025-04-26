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
        
        Lamp.SetActive(false); 

    }

    void Update()
    {
        // // Check for mouse click
        // if (Input.GetMouseButtonDown(0)) // 0 é lado esquerdo do rato
        // {
        //     // Check if the mouse is over the lamp
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         // if (hit.transform.gameObject == Lamp)
        //         // {
        //         //     isLightOn = !isLightOn;
        //         //     Lamp.SetActive(isLightOn);
        //         // }
        //         if (hit.collider == lampCollider)
        //         {
        //             isLightOn = !isLightOn;
        //             Lamp.SetActive(isLightOn);
        //         }
        //     }
        // }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Tecla L pressionada!"); // Adicione esta linha
            isLightOn = !isLightOn;
            if (Lamp != null)
            {
                Lamp.SetActive(isLightOn);
            }
        }
        
    }
}

