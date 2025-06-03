using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class openCloseInterface : MonoBehaviour
{
    // O painel da interface para manipulação das plantas
    public GameObject interfacePanel; 

    // O painel da interface para instruções do robô
    public GameObject interfaceHelp;

    // Gameobject do robô
    [SerializeField] public GameObject robot;

    private PlayerController playerController;
    private GenerateArm roboInteration;
    private GameObject holdPoint;

    void Start()
    {
        interfacePanel.SetActive(false);
        interfaceHelp.SetActive(true);

        GameObject mainObject = robot; // O objeto principal
        playerController = mainObject.GetComponent<PlayerController>();
        roboInteration = mainObject.GetComponent<GenerateArm>();
        holdPoint = GameObject.Find("Ponto_Agarrar");

        if (playerController == null) Debug.LogError("PlayerController não encontrado!");
        if (roboInteration == null) Debug.LogError("GenerateArm não encontrado!");
        if (holdPoint == null) Debug.LogError("HoldPoint não encontrado!");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Exemplo: Abrir/fechar interface com ESC
        {
            if (holdPoint.transform.childCount > 0)
            {
                bool isActive = interfacePanel.activeSelf;
                interfacePanel.SetActive(!isActive);

                // Bloqueia ou desbloqueia o movimento do player
                playerController.blockMoveInteraction(!interfacePanel.activeSelf);
                roboInteration.blockMoveInteraction(!interfacePanel.activeSelf);

                //Desativa a interface de ajuda
                interfaceHelp.SetActive(false);

            } else {
                Debug.LogError("Robô não está a segurar numa planta");
            }

        }

        if (Input.GetKeyDown(KeyCode.H)) // Exemplo: Abrir/fechar interface com ESC
        {
            //Bloqueia a interface de ajuda enquanto que a interface da manipulação das plantas está ativa
            if(!interfacePanel.activeSelf){
                bool isActive = interfaceHelp.activeSelf;
                interfaceHelp.SetActive(!isActive);
            }
        }
       
    }

}