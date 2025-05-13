using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private KeyCode switchKey = KeyCode.V;

    private bool isFirstPerson = true;

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            ToggleCamera();
        }
    }

    void ToggleCamera()
    {
        isFirstPerson = !isFirstPerson;

        if (isFirstPerson)
        {
            firstPersonCam.Priority = 10;
            thirdPersonCam.Priority = 5;
        }
        else
        {
            firstPersonCam.Priority = 5;
            thirdPersonCam.Priority = 10;
        }
    }
}