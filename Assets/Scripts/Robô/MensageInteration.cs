using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MensageInteration : MonoBehaviour
{
    public Text mensage;

    void Start()
    {
        mensage.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //Verificar se o pontoAgarrar está vazio
        if (other.CompareTag("PontoAgarrar") && other.transform.childCount == 0)
        {
            mensage.enabled = true;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PontoAgarrar"))
        {
            mensage.enabled = false;
        }
    }
}