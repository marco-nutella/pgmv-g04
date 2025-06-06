using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MensageInteration : MonoBehaviour
{
    //public Text mensage;
    private UIMessageController messageController;

    void Start()
    {       
        messageController = FindObjectOfType<UIMessageController>();
        if (messageController == null)
        {
            Debug.LogWarning("UIMessageController não foi encontrado na cena.");
        }
        
    }

    public void showMensage(bool isHeld){
        messageController.ShowMessage();

        if (isHeld)
        {
            messageController.SetMessage("Clicar 'E' para largar planta ou 'Esc' para manipular");
        }
        else
        {
            messageController.SetMessage("Clicar 'E' para apanhar planta");
        }
    }
    
    void Update(){
    }
}
        
