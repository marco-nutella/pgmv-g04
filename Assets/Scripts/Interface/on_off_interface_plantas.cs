using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class openCloseInterface : MonoBehaviour
{
    // O painel da interface para manipulação das plantas
    public GameObject interfacePanel; 

    // O painel da interface para instruções do robô
    public GameObject interfaceHelp;

    // Gameobject do robô
    [SerializeField] GameObject robot;
    [SerializeField] Camera targetCamera;

    [SerializeField] CinemachineVirtualCamera targetPlanta;


    public LayerMask[] cullingOptions;
    private int currentIndex = 0;

    private PlayerController playerController;
    private GenerateArm roboInteration;
    private RoboOscilação roboOscilacao;

    private GameObject holdPoint;

    private Vector3 holdPointOriginalLocalPos;
    private float deslocamentoLateral = 2.5f;

    void Start()
    {
        interfacePanel.SetActive(false);
        interfaceHelp.SetActive(true);
        targetPlanta.gameObject.SetActive(false);

        GameObject mainObject = robot; // O objeto principal
        playerController = mainObject.GetComponent<PlayerController>();
        roboInteration = mainObject.GetComponent<GenerateArm>();
        roboOscilacao = mainObject.GetComponentInChildren<RoboOscilação>();
        holdPoint = GameObject.Find("Ponto_Agarrar");

        if (playerController == null) Debug.LogError("PlayerController não encontrado!");
        if (roboInteration == null) Debug.LogError("GenerateArm não encontrado!");
        if (roboOscilacao == null) Debug.LogError("RoboOscilação não encontrado!");
        if (holdPoint == null) Debug.LogError("HoldPoint não encontrado!");
        else holdPointOriginalLocalPos = holdPoint.transform.localPosition;
    }
    
    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Exemplo: Abrir/fechar interface com ESC
        {
            if (holdPoint.transform.childCount > 0)
            {
                currentIndex = (currentIndex + 1) % cullingOptions.Length;
                targetCamera.cullingMask = cullingOptions[currentIndex];
                Debug.Log("Culling Mask changed to option: " + currentIndex);    

                if(!interfacePanel.activeSelf){
                    //holdPoint.layer = LayerMask.NameToLayer("ManipulacaoPlanta");
                    SetLayerRecursively(holdPoint,LayerMask.NameToLayer("ManipulacaoPlanta"));
                    //holdPoint.transform.localPosition = holdPointOriginalLocalPos + new Vector3(deslocamentoLateral, 0f, 0f);

                    Transform planta = holdPoint.transform.GetChild(0); // A planta dentro do holdPoint

                    targetPlanta.Follow = planta;
                    targetPlanta.LookAt = planta;
                    //targetPlanta.Follow = null;
                    //targetPlanta.LookAt = null;

                    //targetPlanta.transform.position = planta.position + new Vector3(-4,0,-8);
                    //targetPlanta.transform.rotation = planta.rotation;

                    var transposer = targetPlanta.GetCinemachineComponent<CinemachineTransposer>();
                    if (transposer != null)
                    {
                        transposer.m_FollowOffset = new Vector3(0, 1, 8f); // você pode ajustar o Y conforme necessário
                    }

                    targetPlanta.gameObject.SetActive(true);


                } else {
                    //holdPoint.layer = LayerMask.NameToLayer("Default");
                    SetLayerRecursively(holdPoint,LayerMask.NameToLayer("Default"));
                    //holdPoint.transform.localPosition = holdPointOriginalLocalPos;
                    targetPlanta.Follow = null;
                    targetPlanta.LookAt = null;
                    targetPlanta.gameObject.SetActive(false);
                }

                PlantInterface plantInterface = interfacePanel.GetComponent<PlantInterface>();
                plantInterface.getDadosPlanta(holdPoint);

                bool isActive = interfacePanel.activeSelf;
                interfacePanel.SetActive(!isActive);

                // Bloqueia ou desbloqueia o movimento do player
                playerController.blockMoveInteraction(!interfacePanel.activeSelf);
                roboInteration.blockMoveInteraction(!interfacePanel.activeSelf);
                roboOscilacao.blockMoveInteraction(!interfacePanel.activeSelf);

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