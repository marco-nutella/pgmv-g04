using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantInterface : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField axiomaInput;
    public InputField iterationsInput;
    public InputField lengthInput;
    public InputField minAngleInput;
    public InputField maxAngleInput;
    public InputField velocityWind;

    public Button playButton;
    public Button pauseButton;
    public Button restartButton;
    public Toggle windToggle;

    private GeradorPlantas selectedPlant;

    void Start()
    {
        /**
        playButton.onClick.AddListener(OnPlay);
        pauseButton.onClick.AddListener(OnPause);
        restartButton.onClick.AddListener(OnRestart);
        windToggle.onValueChanged.AddListener(OnToggleWind);*/
    }

    public void SetSelectedPlant(GeradorPlantas plant)
    {
        selectedPlant = plant;

        // Preencher os campos com os valores atuais da planta
        axiomaInput.text = plant.axiom;
        iterationsInput.text = plant.iterations.ToString();
        lengthInput.text = plant.length.ToString();
        minAngleInput.text = plant.minAngle.ToString();
        maxAngleInput.text = plant.maxAngle.ToString();
        velocityWind.text = plant.maxAngle.ToString();

    }

    void Update()
    {
        if (selectedPlant != null)
        {
            // Atualizar parâmetros em tempo real
            selectedPlant.axiom = axiomaInput.text;
            int.TryParse(iterationsInput.text, out selectedPlant.iterations);
            float.TryParse(lengthInput.text, out selectedPlant.length);
            float.TryParse(minAngleInput.text, out selectedPlant.minAngle);
            float.TryParse(maxAngleInput.text, out selectedPlant.maxAngle);
            float.TryParse(velocityWind.text, out selectedPlant.maxAngle);

        }
    }

    //Necessario criar funções para play, pause, restart
    /**
    void OnPlay() => selectedPlant?.PlayGrowth();
    void OnPause() => selectedPlant?.PauseGrowth();
    void OnRestart() => selectedPlant?.RestartGrowth();
    void OnToggleWind(bool isOn) => selectedPlant?.SetWind(isOn);*/
}
