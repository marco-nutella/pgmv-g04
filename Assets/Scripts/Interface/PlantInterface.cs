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

    //[Header("Toggle")]
    //[SerializeField] private Toggle windToggle;

    //private GeradorPlantasTest selectedPlant;
    private GeradorPlantas selectedPlant;

    void Start()
    {
        /**
        playButton.onClick.AddListener(OnPlay);
        pauseButton.onClick.AddListener(OnPause);
        restartButton.onClick.AddListener(OnRestart);
        windToggle.onValueChanged.AddListener(OnToggleWind);*/
    }
    
    public void getDadosPlanta(GameObject holdPoint){
        if (holdPoint.transform.childCount == 0)
        {
            Debug.LogError("HoldPoint não tem objetos filhos.");
            return;
        }

        GameObject heldPlant = holdPoint.transform.GetChild(0).gameObject;
        //selectedPlant = heldPlant.GetComponentInChildren<GeradorPlantasTest>();
        selectedPlant = heldPlant.GetComponentInChildren<GeradorPlantas>();


        if (selectedPlant == null)
        {
            Debug.LogError("Script GeradorPlantas não encontrado em heldPlant nem em seus filhos!");
            return;
        }
        /**
        axiomaInput.text = selectedPlant.axiom;
        iterationsInput.text = selectedPlant.iterations.ToString();
        lengthInput.text = selectedPlant.length.ToString();
        minAngleInput.text = selectedPlant.minAngle.ToString();
        maxAngleInput.text = selectedPlant.maxAngle.ToString();
        speedWindInput.text = selectedPlant.windSpeed.ToString();
        windToggle.isOn = selectedPlant.windEnabled;*/
        axiomaInput.text = selectedPlant.GetAxiom();
        second_axiomaInput.text = selectedPlant.GetSecondAxiom();

        iterationsInput.text = selectedPlant.GetIterations().ToString();
        lengthInput.text = selectedPlant.GetLength().ToString();
        minAngleInput.text = selectedPlant.GetAngleMin().ToString();
        maxAngleInput.text = selectedPlant.GetAngleMax().ToString();
        speedWindInput.text = selectedPlant.GetVelocidadeDoVento().ToString();
        //windToggle.isOn = selectedPlant.windEnabled;


    }

    void Update()
    {
        /**
        if (selectedPlant != null)
        {
            // Atualizar parâmetros em tempo real
            selectedPlant.axiom = axiomaInput.text;

            int.TryParse(iterationsInput.text, out selectedPlant.iterations);
            float.TryParse(lengthInput.text, out selectedPlant.length);
            float.TryParse(minAngleInput.text, out selectedPlant.minAngle);
            float.TryParse(maxAngleInput.text, out selectedPlant.maxAngle);
            float.TryParse(speedWindInput.text, out selectedPlant.windSpeed);

            selectedPlant.SetWind(windToggle.isOn);
        }*/
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


    /**
    void OnPlay() => selectedPlant?.PlayGrowth();
    void OnPause() => selectedPlant?.PauseGrowth();
    void OnRestart() => selectedPlant?.RestartGrowth();
    void OnToggleWind(bool isOn) => selectedPlant?.SetWind(isOn);*/
    
}