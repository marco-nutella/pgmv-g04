using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosiçãoFilho : MonoBehaviour
{
    public Transform childObject; // Arraste o objeto filho para este campo no Inspector

    void Update()
    {
        if (childObject != null)
        {
            Vector3 worldPosition = childObject.position; // Posição no mundo
            Debug.Log("Posição do filho no mundo: " + worldPosition);
        }
    }
}
