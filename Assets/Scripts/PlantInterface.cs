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

    private PlantGenerator selectedPlant;
    private GeradorPlantas selectedPlant;

    void Start()
    {
        /**
        playButton.onClick.AddListener(OnPlay);
        pauseButton.onClick.AddListener(OnPause);
        restartButton.onClick.AddListener(OnRestart);
        windToggle.onValueChanged.AddListener(OnToggleWind);
        windToggle.onValueChanged.AddListener(OnToggleWind);*/
    }

    public void SetSelectedPlant(PlantGenerator plant)
    public void SetSelectedPlant(GeradorPlantas plant)
    {
        selectedPlant = plant;



@@ -58,8 +59,9 @@ public class PlantInterface : MonoBehaviour
    }

    //Necessario criar funções para play, pause, restart
    /**
    void OnPlay() => selectedPlant?.PlayGrowth();
    void OnPause() => selectedPlant?.PauseGrowth();
    void OnRestart() => selectedPlant?.RestartGrowth();
    void OnToggleWind(bool isOn) => selectedPlant?.SetWind(isOn);
    void OnToggleWind(bool isOn) => selectedPlant?.SetWind(isOn);*/
}