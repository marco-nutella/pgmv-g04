using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorPlantasTest : MonoBehaviour
{
    [Header("Parâmetros da planta")]
    public string axiom = "F";
    public int iterations = 3;
    public float length = 5f;
    public float minAngle = 25f;
    public float maxAngle = 35f;

    [Header("Vento")]
    public float windSpeed = 1f;
    public bool windEnabled = false;

    private bool isGrowing = false;

    void Start()
    {
        // Inicialização se necessário
    }

    void Update()
    {
        if (isGrowing)
        {
            // Aqui você poderia simular o crescimento da planta
            // ou apenas deixar o estado ativo
        }

        if (windEnabled)
        {
            // Simulação de vento, se necessário
        }
    }

    public void PlayGrowth()
    {
        Debug.Log("Crescimento iniciado.");
        isGrowing = true;
        // Aqui você colocaria o código de animação ou lógica de crescimento
    }

    public void PauseGrowth()
    {
        Debug.Log("Crescimento pausado.");
        isGrowing = false;
        // Aqui você pausaria qualquer lógica de crescimento/tempo
    }

    public void RestartGrowth()
    {
        Debug.Log("Crescimento reiniciado.");
        isGrowing = false;
        // Resetar estado da planta para valores iniciais
        // (ou destruir e recriar, dependendo da lógica)
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        PlayGrowth();
    }

    public void SetWind(bool isOn)
    {
        windEnabled = isOn;
        //Debug.Log($"Vento {(isOn ? "ativado" : "desativado")}.");
    }

    // Método para debug opcional: mostrar valores atuais
    public void PrintCurrentSettings()
    {
        Debug.Log($"Axioma: {axiom}, Iterações: {iterations}, Comprimento: {length}, Ângulos: {minAngle}-{maxAngle}, Vento: {windEnabled} ({windSpeed})");
    }
}
