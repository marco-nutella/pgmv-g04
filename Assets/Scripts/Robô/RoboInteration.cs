using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateArm : MonoBehaviour
{
    [SerializeField] GameObject armPrefab;
    [SerializeField] GameObject handPrefab;
    [SerializeField] GameObject leftShoulderPoint;
    [SerializeField] GameObject rightShoulderPoint;
    private bool canMove = true;

    public void blockMoveInteraction(bool value)
    {
        canMove = value;
        Debug.Log("Interações do robô está " + canMove );
    }

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

    [SerializeField] Transform holdPoint;
    public float grabRange = 1f;

    private GameObject heldObject;
    private Collider heldObjectCollider;
    private Collider[] playerColliders;

    private Quaternion targetRotationLeft;
    private Quaternion targetRotationRight;

    private bool isMovingArms = false;
    private bool shouldGrabAfterMove = false;
    private float armMoveProgress = 0f;
    private float armMoveDuration = 0.3f; // duração da animação dos braços em segundos

    private GameObject[] validMounts;
    private GameObject? nearestMount;
    private float? nearestMountDistance;
    private float maxMountDistance = 8.0f;

    private bool IsPlantInsideMount(GameObject mount) { // https://discussions.unity.com/t/how-to-find-child-with-tag/129880/2
		for (int i = 0; i < mount.transform.childCount; i++) 
		{
			if(mount.transform.GetChild(i).gameObject.tag == "Planta")
			{
                return true;
			}	
		}	
		return false;
    }
    private IEnumerator LoopUpdateNearestMount()
    {
        yield return new WaitForSeconds(3f);
        validMounts = GameObject.FindGameObjectsWithTag("PlantMount");

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool foundNear = false;
            if (Physics.Raycast(ray, out hit))
            {
                foreach (GameObject mount in validMounts)
                {
                    float dist = (mount.transform.position - hit.point).magnitude;
                    float distBody = (mount.transform.position - transform.position).magnitude;

                    if (dist > maxMountDistance || distBody > maxMountDistance) // Although we want the selected mount to be based on what the player is looking at, we don't want it to be too far from their body either.
                    {
                        continue;
                    }
                    foundNear = true; // We want to clear the data on the nearest mount if the player is not near any

                    if (nearestMountDistance == null || dist < nearestMountDistance)
                    {
                        if (nearestMount && !IsPlantInsideMount(nearestMount))
                        {
                            nearestMount.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/PlantOutline", typeof(Sprite)) as Sprite;
                        }
                        nearestMount = mount;
                        if (!IsPlantInsideMount(nearestMount)) {
                            nearestMount.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/PlantPlacement", typeof(Sprite)) as Sprite;
                        }
                        nearestMountDistance = dist;
                    }
                }
            }

            if (!foundNear)
            {
                if (nearestMount && !IsPlantInsideMount(nearestMount))
                {
                    nearestMount.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/PlantOutline", typeof(Sprite)) as Sprite;
                }
                nearestMount = null;
                nearestMountDistance = null;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    void Start()
    {
        GenerateArms();
        originalRotationLeftShoulder = leftShoulderPoint.transform.rotation;
        originalRotationRightShoulder = rightShoulderPoint.transform.rotation;
        playerColliders = GetComponentsInChildren<Collider>();

        targetRotationLeft = leftShoulder.transform.rotation;
        targetRotationRight = rightShoulder.transform.rotation;

        StartCoroutine(LoopUpdateNearestMount());
    }

    void Update(){
        
        if(canMove){

        

        if (Input.GetKeyDown(grabKey)){
             
            //Apanhar o objeto: Ver se consegue, Move os braços, agarra o objeto
            if(heldObject == null && canGrabPlanta() && !isMovingArms){
                moveArmsMotion(true);
            }

            if(heldObject != null){
                releaseObject();
                moveArmsMotion();
            }

        }
        
        if (isMovingArms){
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
    }

    private bool canGrabPlanta(){
        Collider[] colliders = Physics.OverlapSphere(holdPoint.position, grabRange);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Planta")){return true;}
        }

        return false;
    }

/**
    void grabObject()
    {        
        Collider[] colliders = Physics.OverlapSphere(holdPoint.position, grabRange);

        foreach (var col in colliders)
        {
            if (col.CompareTag("Planta"))
            {
                heldObject = col.gameObject;
                heldObjectCollider = col;

                // Ignora colisões com TODOS os colliders do jogador
                foreach (var playerCol in playerColliders){
                    Physics.IgnoreCollision(heldObjectCollider, playerCol, true);
                }

                // Desativa física
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null){
                    rb.isKinematic = true;
                }

                // Anexa ao holdPoint (parenting)
                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
                break;
            }
        }
    }*/
    void grabObject()
    {
        Collider[] colliders = Physics.OverlapSphere(holdPoint.position, grabRange);

        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;
        Collider closestCollider = null;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Planta"))
            {
                float distance = Vector3.Distance(holdPoint.position, col.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = col.gameObject;
                    closestCollider = col;
                }
            }
        }

        if (closestObject != null)
        {
            heldObject = closestObject;
            heldObjectCollider = closestCollider;

            // Ignora colisões com todos os colliders do jogador
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
            if (heldObject.transform.parent && heldObject.transform.parent.tag == "PlantMount")
            {
                heldObject.transform.parent.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/PlantPlacement", typeof(Sprite)) as Sprite;
            }
            heldObject.transform.SetParent(holdPoint, true);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
        }
    }

    void releaseObject()
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
        heldObject.transform.SetParent(nearestMount?.transform, true);
        if (nearestMount != null)
        {
            heldObject.transform.position = nearestMount.transform.position;
            nearestMount.GetComponent<SpriteRenderer>().sprite = null;
        }
        heldObject = null;
        heldObjectCollider = null;
    }

    void moveArmsMotion(bool grabAfter = false){
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
    
    /** void moveArms(){
        if (Input.GetKeyDown(grabKey)) {
            isHoldObject = !isHoldObject;

            Quaternion baseRotationLeft = transform.rotation * originalRotationLeftShoulder;
            Quaternion baseRotationRight = transform.rotation * originalRotationRightShoulder;

            targetRotation = Quaternion.Euler(-50, 0, 0);

            Quaternion grabRotation = isHoldObject ? targetRotation : Quaternion.identity;

            leftShoulderPoint.transform.rotation = baseRotationLeft * grabRotation;
            rightShoulderPoint.transform.rotation = baseRotationRight * grabRotation;
        }   
    }*/

    void GenerateArms(){
  
        Quaternion rotationArmHand = Quaternion.Euler(-45, 0, 0);

        leftShoulder = Instantiate(armPrefab);
        leftArm = Instantiate(armPrefab);
        leftHand = Instantiate(handPrefab);

        leftShoulder.transform.SetParent(leftShoulderPoint.transform);
        leftArm.transform.SetParent(leftShoulder.transform);
        leftHand.transform.SetParent(leftArm.transform);
        
        Vector3 shoulderPosition = new Vector3(-0.7f, 0.2f, 0);
        Vector3 armPosition = new Vector3(-0.5f, -0.9f, 0);
        Vector3 handPosition = new Vector3(0,-0.8f,0.8f);


        leftShoulder.transform.position = leftShoulderPoint.transform.parent.position + shoulderPosition;

        //new Vector3(-0.7f, 1.5f, 0);
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
        
        //new Vector3(0.7f, 1.5f, 0);
        rightShoulder.transform.rotation = Quaternion.Euler(0, 0, 30);

        rightArm.transform.position = rightShoulder.transform.position + new Vector3(-armPosition.x, armPosition.y, armPosition.z);
        rightArm.transform.rotation = rotationArmHand;

        rightHand.transform.position = rightArm.transform.position + handPosition;
        rightHand.transform.rotation = rotationArmHand;
    }
}