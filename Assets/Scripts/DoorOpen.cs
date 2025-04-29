using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }
    public GameObject door;
    public float openRot, closeRot, speed;
    public bool open; 
    // Update is called once per frame
    void Update()
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        if (open){
            if (currentRot.y < openRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openRot, currentRot.z), speed * Time.deltaTime);
            }
        }
        else
        {
            if (currentRot.y > closeRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, closeRot, currentRot.z), speed * Time.deltaTime);
            }
        }
    }
    public void OpenDoor()
    {
        open = !open;
    }
}
