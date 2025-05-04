using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerScript : UtilityScript
{
    [SerializeField]
    private AnimationCurve animationCurve;
    private GameObject drawerObject;
    private bool drawerOpen = false;
    private bool debounce = false;
    private float tweenDuration = 0.5f;

    [SerializeField]
    private bool debugBool = false;

    void Start()
    {
        drawerObject = transform.Find("Gaveta (Interior)").gameObject;
        if (drawerObject == null) {
            Debug.LogError("Drawer child not found in the object.");
            return;
        }
    }

    void Update()
    {
        if ((OnClickActivation(drawerObject) || debugBool) && !debounce)
        {
            Vector3 endPosition;
            debounce = true;
            switch (drawerOpen) {
                case true:
                    endPosition = new Vector3(drawerObject.transform.position.x, drawerObject.transform.position.y, drawerObject.transform.position.z);
                    endPosition += Vector3.right*-2.005999935f;
                    StartCoroutine(TweenGameObject(drawerObject, endPosition, tweenDuration, animationCurve, callbackResult => 
                    {
                        debounce = false;
                    }));
                    break;
                case false:
                    endPosition = new Vector3(drawerObject.transform.position.x, drawerObject.transform.position.y, drawerObject.transform.position.z); 
                    endPosition += Vector3.right*2.005999935f;
                    StartCoroutine(TweenGameObject(drawerObject, endPosition, tweenDuration, animationCurve, callbackResult => 
                    {
                        debounce = false;
                    }));
                    break;
            }

           drawerOpen = !drawerOpen;
        }
    }
}
