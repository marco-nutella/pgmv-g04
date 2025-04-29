using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScriptTiago : MonoBehaviour
{
    // Update is called once per frame
    private Light myLight;

    void Start()
    {
        myLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique com o botão esquerdo do mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    if (myLight != null)
                    {
                        myLight.enabled = !myLight.enabled;
                    }
                }
            }
        }
    }

}
