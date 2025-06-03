using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera_first;
    [SerializeField] CinemachineVirtualCamera virtualCamera_third;
    public float sensibilidade = 2.0f;
    private float mouseX, mouseY;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
        Cursor.visible = false; // Oculta o cursor
    }

    void Update()
    {
        // Só executa se a primeira câmera tiver prioridade maior que a terceira
        if (virtualCamera_first.Priority > virtualCamera_third.Priority)
        {
            mouseX += Input.GetAxis("Mouse X") * sensibilidade;
            mouseY -= Input.GetAxis("Mouse Y") * sensibilidade; // Inverte o eixo Y para um movimento mais natural

            transform.eulerAngles = new Vector3(mouseY, mouseX, 0); // Aplica a rotação
        }
    }
}
