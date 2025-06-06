using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : UtilityScript
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }
    public GameObject door;
    public bool debugBool;
    public float openRot, closeRot, duration;
    public Vector3 openPos, closePos;
    private bool open = false, debounce = false; 
    // Update is called once per frame

    void Update()
    {
        if ((OnClickActivation(door) || debugBool)) {
            OpenDoor();
        }

    }
    public void OpenDoor()
    {
        AudioManager.Instance.Play(AudioManager.SoundType.DoorOpen);
        if (!debounce)
        {
            debounce = true;

            Vector3 currentRot = door.transform.localEulerAngles;
            Vector3 currentPos = door.transform.position;

            if (!open)
            {
                Debug.Log("Opening door");
                Vector3 newRot = new Vector3(currentRot.x, openRot, currentRot.z);
                Vector3 newPos = new Vector3(openPos.x, currentPos.y, openPos.z);

                StartCoroutine(TweenGameObjectRotation(door, currentRot, newRot, duration));
                StartCoroutine(TweenGameObject(door, currentPos, newPos, duration, null, callbackResult =>
                {
                    debounce = callbackResult;
                }));
                //door.transform.localEulerAngles = Vector3.Lerp(currentRot, , duration * Time.deltaTime);
                //door.transform.position = Vector3.Lerp(currentPos, new Vector3(openPos.x, currentPos.y, openPos.z), duration * Time.deltaTime);
            }
            else
            {
                Debug.Log("Closing door");
                Vector3 newRot = new Vector3(currentRot.x, closeRot, currentRot.z);
                Vector3 newPos = new Vector3(closePos.x, currentPos.y, closePos.z);

                StartCoroutine(TweenGameObjectRotation(door, currentRot, newRot, duration));
                StartCoroutine(TweenGameObject(door, currentPos, newPos, duration, null, callbackResult =>
                {
                    debounce = callbackResult;
                }));
                //door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, closeRot, currentRot.z), duration * Time.deltaTime);
                //door.transform.position = Vector3.Lerp(currentPos, new Vector3(closePos.x, currentPos.y, closePos.z), duration * Time.deltaTime);
            }

            open = !open;
        }
        
    }
}
