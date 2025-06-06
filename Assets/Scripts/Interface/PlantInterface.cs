using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantInterface : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField axiomaInput;
    [SerializeField] private TMP_InputField second_axiomaInput;
    [SerializeField] private TMP_InputField iterationsInput;
    [SerializeField] private TMP_InputField lengthInput;
    [SerializeField] private TMP_InputField minAngleInput;
    [SerializeField] private TMP_InputField maxAngleInput;
    [SerializeField] private TMP_InputField speedWindInput;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button restartButton;

    private GeradorPlantas selectedPlant;

    void Start()
    {
        playButton.onClick.AddListener(OnPlay);
        pauseButton.onClick.AddListener(OnPause);
        restartButton.onClick.AddListener(OnRestart);
    }

    public void getDadosPlanta(GameObject holdPoint) {
        if (holdPoint.transform.childCount == 0)
        {
            Debug.LogError("HoldPoint não tem objetos filhos.");
            return;
        }

        GameObject heldPlant = holdPoint.transform.GetChild(0).gameObject;
        selectedPlant = heldPlant.GetComponentInChildren<GeradorPlantas>();


        if (selectedPlant == null)
        {
            Debug.LogError("Script GeradorPlantas não encontrado em heldPlant nem em seus filhos!");
            return;
        }

        axiomaInput.text = selectedPlant.GetAxiom();
        second_axiomaInput.text = selectedPlant.GetSecondAxiom();

        iterationsInput.text = selectedPlant.GetIterations().ToString();
        lengthInput.text = selectedPlant.GetLength().ToString();
        minAngleInput.text = selectedPlant.GetAngleMin().ToString();
        maxAngleInput.text = selectedPlant.GetAngleMax().ToString();
        speedWindInput.text = selectedPlant.GetVelocidadeDoVento().ToString();


    }

    void Update()
    {
        if (selectedPlant != null)
        {
            // Atualizar parâmetros em tempo real usando setters
            selectedPlant.SetAxiom(axiomaInput.text);
            selectedPlant.SetSecondAxiom(second_axiomaInput.text);

            if (int.TryParse(iterationsInput.text, out int iterations))
                selectedPlant.SetIterations(iterations);

            if (float.TryParse(lengthInput.text, out float length))
                selectedPlant.SetLength(length);

            if (float.TryParse(minAngleInput.text, out float minAngle))
                selectedPlant.SetAngleMin(minAngle);

            if (float.TryParse(maxAngleInput.text, out float maxAngle))
                selectedPlant.SetAngleMax(maxAngle);

            if (float.TryParse(speedWindInput.text, out float windSpeed))
                selectedPlant.SetVelocidadeDoVento(windSpeed);

            // Se quiser habilitar vento com um toggle, adicione a variável:
            // selectedPlant.SetWind(windToggle.isOn); 
        }
    }



    void OnPlay() {
        selectedPlant?.DeletePlant();
        selectedPlant?.CreatePlant(1);
    }
    void OnPause() {
        selectedPlant?.DeletePlant();
        selectedPlant?.CreatePlant(2);
    }
    void OnRestart() {
        selectedPlant?.DeletePlant();
        selectedPlant?.CreatePlant();
    }
    
}