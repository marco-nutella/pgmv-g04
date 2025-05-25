using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboOscilação : MonoBehaviour
{
    public float amplitude = 0.2f;     // Altura do sobe e desce
    public float frequency = 1f;       // Velocidade da oscilação

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }
}
