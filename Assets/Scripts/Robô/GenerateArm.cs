using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateArm : MonoBehaviour
{
    [SerializeField] GameObject armPrefab;
    [SerializeField] GameObject handPrefab;

    [SerializeField] GameObject leftShoulderPoint;
    [SerializeField] GameObject rightShoulderPoint;

    public KeyCode grabKey = KeyCode.E;
    private bool isHoldObject = false;

    private GameObject leftShoulder;
    private GameObject rightShoulder;

    private GameObject leftArm;
    private GameObject rightArm;
    private GameObject leftHand;
    private GameObject rightHand;

    private Quaternion originalRotationLeftShoulder;
    private Quaternion originalRotationRightShoulder;
    private Quaternion targetRotation;

    public Transform holdPoint;
    public float grabRange = 2f;

    private GameObject heldObject;
    private Collider heldObjectCollider;
    private Collider[] playerColliders;

    void Start()
    {
        GenerateArms();
        originalRotationLeftShoulder = leftShoulderPoint.transform.rotation;
        originalRotationRightShoulder = rightShoulderPoint.transform.rotation;
        playerColliders = GetComponentsInChildren<Collider>();

    }

    void Update() {
        if (Input.GetKeyDown(grabKey))
        {
            if (heldObject == null)
            {
                TryGrabObject();
                if(heldObject != null){moveArms();}
            }
            else
            {
                ReleaseObject();
                if(heldObject == null){moveArms();}
            }
        }
        
    }

    void TryGrabObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange);

        foreach (var col in colliders)
        {
            if (col.CompareTag("Planta"))
            {
                heldObject = col.gameObject;
                heldObjectCollider = col;

                // Ignora colisões com TODOS os colliders do jogador
                foreach (var playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(heldObjectCollider, playerCol, true);
                }

                // Desativa física
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                // Anexa ao holdPoint (parenting)
                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                break;
            }
        }
    }

    void ReleaseObject()
    {
        if (heldObject == null) return;

        // Reativa física
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Restaura colisões com todos os colliders do jogador
        if (heldObjectCollider != null)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(heldObjectCollider, playerCol, false);
            }
        }

        // Solta o objeto
        heldObject.transform.SetParent(null);
        heldObject = null;
        heldObjectCollider = null;
    }


    void moveArms()
    {
        if (Input.GetKeyDown(grabKey)) {
            isHoldObject = !isHoldObject;

            Quaternion baseRotationLeft = transform.rotation * originalRotationLeftShoulder;
            Quaternion baseRotationRight = transform.rotation * originalRotationRightShoulder;

            targetRotation = Quaternion.Euler(-50, 0, 0);

            Quaternion grabRotation = isHoldObject ? targetRotation : Quaternion.identity;

            leftShoulderPoint.transform.rotation = baseRotationLeft * grabRotation;
            rightShoulderPoint.transform.rotation = baseRotationRight * grabRotation;
        }   
    }
    void GenerateArms(){
  
        leftShoulder = Instantiate(armPrefab);
        leftArm = Instantiate(armPrefab);
        leftHand = Instantiate(handPrefab);

        leftShoulder.transform.SetParent(leftShoulderPoint.transform);
        leftArm.transform.SetParent(leftShoulder.transform);
        leftHand.transform.SetParent(leftArm.transform);
        
        leftShoulder.transform.position = leftShoulderPoint.transform.parent.position + new Vector3(-0.7f, 1.5f, 0);
        leftShoulder.transform.rotation = Quaternion.Euler(0, 0, -30);
            
        leftArm.transform.position = leftShoulder.transform.position + new Vector3(-0.5f, -0.9f, 0);
        leftArm.transform.rotation = Quaternion.Euler(-45, 0, 0);

        leftHand.transform.position = leftArm.transform.position + new Vector3 (0,-0.8f,0.8f);
        leftHand.transform.rotation = Quaternion.Euler(-45,0,0);

        rightShoulder = Instantiate(armPrefab);
        rightArm = Instantiate(armPrefab);
        rightHand = Instantiate(handPrefab);

        rightShoulder.transform.SetParent(rightShoulderPoint.transform);
        rightArm.transform.SetParent(rightShoulder.transform);
        rightHand.transform.SetParent(rightArm.transform);

        rightShoulder.transform.position = rightShoulderPoint.transform.parent.position + new Vector3(0.7f, 1.5f, 0);
        rightShoulder.transform.rotation = Quaternion.Euler(0, 0, 30);

        rightArm.transform.position = rightShoulder.transform.position + new Vector3(0.5f, -0.9f, 0);
        rightArm.transform.rotation = Quaternion.Euler(-45, 0, 0);

        rightHand.transform.position = rightArm.transform.position + new Vector3 (0,-0.8f,0.8f);
        rightHand.transform.rotation = Quaternion.Euler(-45,0,0);
    }
}
