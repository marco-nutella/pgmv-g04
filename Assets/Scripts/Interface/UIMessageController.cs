using UnityEngine;
using UnityEngine.UI;

public class UIMessageController : MonoBehaviour
{
    public Text messageText;

    void Start()
    {
        messageText.enabled = false;
    }

    public void HideMessage()
    {
        messageText.enabled = false;
    }

    public void ShowMessage(){
        messageText.enabled = true;
    }

    public void SetMessage(string msg)
    {
        messageText.text = msg;
    }
}
