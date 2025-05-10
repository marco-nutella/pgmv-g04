using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public Transform holdPoint;
    public float grabRange = 1f;
    public KeyCode grabKey = KeyCode.E;

    private GameObject heldObject;
    private Collider heldObjectCollider;
    private Collider[] playerColliders;

    void Start()
    {
        // Pega todos os colliders do jogador e seus filhos (corpo inteiro)
        playerColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (heldObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
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
}
