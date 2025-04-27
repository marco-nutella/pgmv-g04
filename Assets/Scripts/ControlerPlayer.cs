using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlerPlayer : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = 0.0f;

        // Verifica se as teclas de subida ou descida estão pressionadas
        if (Input.GetKey(KeyCode.E))
        {
            moveUp = 1.0f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            moveUp = -1.0f;
        }

        // Cria um vetor de movimento baseado no input
        Vector3 movement = new Vector3(moveHorizontal, moveUp, moveVertical);

        // Aplica o movimento ao jogador
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
