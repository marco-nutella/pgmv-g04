using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateArmOld : MonoBehaviour
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

    [SerializeField] Transform holdPoint;
    public float grabRange = 0.1f;

    private GameObject heldObject;
    private Collider heldObjectCollider;
    private Collider[] playerColliders;

    private Quaternion targetRotationLeft;
    private Quaternion targetRotationRight;

    private bool isMovingArms = false;
    private bool shouldGrabAfterMove = false;
    private float armMoveProgress = 0f;
    private float armMoveDuration = 0.3f;
    public Text mensage;

    void Start()
    {
        GenerateArms();
        originalRotationLeftShoulder = leftShoulderPoint.transform.rotation;
        originalRotationRightShoulder = rightShoulderPoint.transform.rotation;
        playerColliders = GetComponentsInChildren<Collider>();

        targetRotationLeft = leftShoulder.transform.rotation;
        targetRotationRight = rightShoulder.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (heldObject == null && !isMovingArms) //&& canGrabPlanta()
            {
                moveArmsMotion(true);
            }

            if (heldObject != null)
            {
                releaseObject();
                moveArmsMotion();
            }
        }

        Ray ray = new Ray(holdPoint.position, holdPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange) && heldObject != null)
        {
            if (hit.collider.CompareTag("Planta"))
            {
                mensage.enabled = true;
            }
        } else 
        {
            mensage.enabled = false;
        }

        if (isMovingArms)
        {
            armMoveProgress += Time.deltaTime;
            float t = Mathf.Clamp01(armMoveProgress / armMoveDuration);

            leftShoulderPoint.transform.rotation = Quaternion.Slerp(leftShoulderPoint.transform.rotation, targetRotationLeft, t);
            rightShoulderPoint.transform.rotation = Quaternion.Slerp(rightShoulderPoint.transform.rotation, targetRotationRight, t);

            if (t >= 1f)
            {
                isMovingArms = false;
                if (shouldGrabAfterMove)
                {
                    shouldGrabAfterMove = false;
                    grabObject();
                }
            }
        }
    }

    private bool canGrabPlanta()
    {
        Ray ray = new Ray(holdPoint.position, holdPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.collider.CompareTag("Planta"))
            {
                //Debug.Log("Encontrou a planta");
                return true;
            }
        }
        return false;
    }

    void grabObject()
    {
        Ray ray = new Ray(holdPoint.position, holdPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.collider.CompareTag("Planta"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectCollider = hit.collider;

                foreach (var playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(heldObjectCollider, playerCol, true);
                }

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void releaseObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.isKinematic = false;
        }

        if (heldObjectCollider != null)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics.IgnoreCollision(heldObjectCollider, playerCol, false);
            }
        }

        heldObject.transform.SetParent(null);
        heldObject = null;
        heldObjectCollider = null;
    }

    void moveArmsMotion(bool grabAfter = false)
    {
        isHoldObject = !isHoldObject;

        Quaternion baseRotationLeft = transform.rotation * originalRotationLeftShoulder;
        Quaternion baseRotationRight = transform.rotation * originalRotationRightShoulder;

        Quaternion grabRotation = isHoldObject ? Quaternion.Euler(-50, 0, 0) : Quaternion.identity;

        targetRotationLeft = baseRotationLeft * grabRotation;
        targetRotationRight = baseRotationRight * grabRotation;

        isMovingArms = true;
        armMoveProgress = 0f;
        shouldGrabAfterMove = grabAfter;
    }

    void GenerateArms()
    {
        Quaternion rotationArmHand = Quaternion.Euler(-45, 0, 0);

        leftShoulder = Instantiate(armPrefab);
        leftArm = Instantiate(armPrefab);
        leftHand = Instantiate(handPrefab);

        leftShoulder.transform.SetParent(leftShoulderPoint.transform);
        leftArm.transform.SetParent(leftShoulder.transform);
        leftHand.transform.SetParent(leftArm.transform);

        Vector3 shoulderPosition = new Vector3(-0.7f, 0.2f, 0);
        Vector3 armPosition = new Vector3(-0.5f, -0.9f, 0);
        Vector3 handPosition = new Vector3(0, -0.8f, 0.8f);

        leftShoulder.transform.position = leftShoulderPoint.transform.parent.position + shoulderPosition;
        leftShoulder.transform.rotation = Quaternion.Euler(0, 0, -30);

        leftArm.transform.position = leftShoulder.transform.position + armPosition;
        leftArm.transform.rotation = rotationArmHand;

        leftHand.transform.position = leftArm.transform.position + handPosition;
        leftHand.transform.rotation = rotationArmHand;

        rightShoulder = Instantiate(armPrefab);
        rightArm = Instantiate(armPrefab);
        rightHand = Instantiate(handPrefab);

        rightShoulder.transform.SetParent(rightShoulderPoint.transform);
        rightArm.transform.SetParent(rightShoulder.transform);
        rightHand.transform.SetParent(rightArm.transform);

        rightShoulder.transform.position = rightShoulderPoint.transform.parent.position + new Vector3(-shoulderPosition.x, shoulderPosition.y, shoulderPosition.z);
        rightShoulder.transform.rotation = Quaternion.Euler(0, 0, 30);

        rightArm.transform.position = rightShoulder.transform.position + new Vector3(-armPosition.x, armPosition.y, armPosition.z);
        rightArm.transform.rotation = rotationArmHand;

        rightHand.transform.position = rightArm.transform.position + handPosition;
        rightHand.transform.rotation = rotationArmHand;
    }

    void OnDrawGizmosSelected()
    {
        if (holdPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(holdPoint.position, holdPoint.position + holdPoint.forward * grabRange);
        }
    }
}
