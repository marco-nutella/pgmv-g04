using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampScriptTiago : UtilityScript
{
    // Update is called once per frame
    private Light myLight;

    void Start()
    {
        myLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (OnClickActivation(transform.gameObject)) {
            if (myLight != null)
            {
                myLight.enabled = !myLight.enabled;
            }
        }
    }

}
