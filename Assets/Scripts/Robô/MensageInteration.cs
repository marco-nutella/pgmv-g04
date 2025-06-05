using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MensageInteration : MonoBehaviour
{
    //public Text mensage;
    public UIMessageController messageController;


    void Start()
    {
        //mensage.enabled = false;
        //messageController.HideMessage();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PontoAgarrar") && transform.parent == null)
        {
            //mensage.enabled = false;
            messageController.HideMessage();
        }
    }

    public void showMensage(){
        //messageController.ShowMessage();
        messageController.ShowMessage();

        if (transform.parent != null)
        {
            messageController.SetMessage("Clicar 'E' para largar planta ou 'Esc' para manipular");
        }
        else
        {
            messageController.SetMessage("Clicar 'E' para apanhar planta");
        }
        
    }

    
    /**
    public void SetMessage(string newText)
    {
        mensage.text = newText;
    }*/

    void Update(){
        /**
        if (transform.parent != null)
        {
            messageController.ShowMessage();
            messageController.SetMessage("Clicar 'E' para largar planta ou 'Esc' para manipular");

        }
        else
        {
            messageController.SetMessage("Clicar 'E' para apanhar planta");
        }*/

        /**
        if (transform.parent != null)
        {
            mensage.enabled = true;
            SetMessage("Clicar 'E' para largar planta ou 'Esc' para manipular");
        }
        else
        {
            SetMessage("Clicar 'E' para apanhar planta");
        }
        */
    }
}
        
