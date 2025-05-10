using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ArmGenerator : MonoBehaviour
{
    [SerializeField] GameObject armPrefab;
    [SerializeField] GameObject handPrefab;

    [SerializeField] bool isLeft;
    public KeyCode grabKey = KeyCode.E;
    private bool isHoldObject = false;
    private GameObject shoulder;
    private GameObject arm;

    private GameObject hand;
    private Quaternion originalRotationShoulder;
    private Quaternion targetRotation;



    // Start is called before the first frame update
    void Start()
    {
        //Verificar se GameObject is null
        gerarArm();
        originalRotationShoulder = shoulder.transform.rotation;

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(grabKey)) {
            isHoldObject = !isHoldObject;

            // Definir a rotação base do ombro com base na rotação do corpo
            Quaternion baseRotation = transform.rotation * originalRotationShoulder;
            
            if (isLeft){        
                targetRotation = originalRotationShoulder * Quaternion.Euler(-45,-10, 0);
                Quaternion grabRotation = isHoldObject ? targetRotation : originalRotationShoulder;
                shoulder.transform.rotation = baseRotation * Quaternion.Inverse(originalRotationShoulder) * grabRotation;

            } else {
                targetRotation = originalRotationShoulder * Quaternion.Euler(-45, 10, 0);
                Quaternion grabRotation = isHoldObject ? targetRotation : originalRotationShoulder;
                shoulder.transform.rotation = baseRotation * Quaternion.Inverse(originalRotationShoulder) * grabRotation;
            }
        }   
    }

    void gerarArm(){

        shoulder = Instantiate(armPrefab);
        arm = Instantiate(armPrefab);
        hand = Instantiate(handPrefab);

        shoulder.transform.SetParent(this.transform);
        arm.transform.SetParent(shoulder.transform);
        hand.transform.SetParent(arm.transform);
        

        if (isLeft){
            shoulder.transform.position = transform.parent.position + new Vector3(-0.7f, 1.5f, 0);
            shoulder.transform.rotation = Quaternion.Euler(0, 0, -30);
            
            arm.transform.position = shoulder.transform.position + new Vector3(-0.5f, -0.9f, 0);
            arm.transform.rotation = Quaternion.Euler(-45, 0, 0);

            hand.transform.position = arm.transform.position + new Vector3 (0,-0.8f,0.8f);
            hand.transform.rotation = Quaternion.Euler(-45,0,0);

        } else {
            shoulder.transform.position = transform.parent.position + new Vector3(0.7f, 1.5f, 0);
            shoulder.transform.rotation = Quaternion.Euler(0, 0, 30);

            arm.transform.position = shoulder.transform.position + new Vector3(0.5f, -0.9f, 0);
            arm.transform.rotation = Quaternion.Euler(-45, 0, 0);

            hand.transform.position = arm.transform.position + new Vector3 (0,-0.8f,0.8f);
            hand.transform.rotation = Quaternion.Euler(-45,0,0);

        }
        


    }
}
