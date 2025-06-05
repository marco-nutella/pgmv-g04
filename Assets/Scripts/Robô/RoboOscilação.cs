using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboOscilação : MonoBehaviour
{
    public float amplitude = 0.2f;     // Altura do sobe e desce
    public float frequency = 1f;       // Velocidade da oscilação

    private bool canMove = true;

    public void blockMoveInteraction(bool value)
    {
        canMove = value;
        Debug.Log("Controlador do robô está "+ canMove );
    }

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if(canMove){
            float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.localPosition = startPos + new Vector3(0, offsetY, 0);
        }
    }
}
