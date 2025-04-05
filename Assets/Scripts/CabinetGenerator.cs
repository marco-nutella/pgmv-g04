using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CabinetGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] cabinetPrefabsFromInspector; /*
    {
    [1] = short;
    [2] = medium;
    [3] = tall;
    }
    */
    private GameObject[] doorPrefabsFromInspector;
    private GameObject[] modulesPrefabsFromInspector;
    private GameObject[] cabinets;
    private GameObject[] doors;
    private GameObject[] modules;
    void Start()
    {
      
    }

    public void GenerateCabinet(XmlNode cabinetSettings)
    {
        int width = cabinetSettings.Attributes["Width"].Value;
        int height = cabinetSettings.Attributes["Height"].Value;

        if (!width || !height || width <= 0 || height <= 0) { // Verificar se os valores são válidos
            Debug.LogError("Cabinet width or height are invalid or null. Width: ", width, " Height:", height );
        }

        float stackedCabinets = Mathf.Ceil(height/3); // Lógica de fazer a "stack" das cabinetes caso a altura seja maior do que 3
        int[] cabinetHeights = {};
        // 1 > 1/3 = 0.33 > 1
        // 2 > 2/3 = 0.67 > 1
        // 5 > 5/3 = 1.67 > 2
        if (stackedCabinets > 1) {
            int i, k = 0, height;
            while (k > 3) {
                cabinetHeights[i] == 3;
                k-=3;
                i++;
            }
            
            cabinetHeights[i] == heightDivRemainder == 0 ? 3 : heightDivRemainder;
        } else {
            int heightDivRemainder = height%3; // A altura desejada se for menor ou maior que 3, 0 se for 3
            cabinetHeights[0] == heightDivRemainder == 0 ? 3 : heightDivRemainder;
        }


        GameObject chosenCabinet = cabinetPrefabsFromInspector[height%3];
        cabinets = new GameObject[width];
        /* https://discussions.unity.com/t/how-do-you-set-prefabs-into-a-gameobject-array-that-will-activate-deactivate-when-called/180007
         https://gamedev.stackexchange.com/questions/142451/how-to-instantiate-an-array-of-prefabs-in-c-script
        {
             cabinets[i] = Instantiate(cabinetPrefabsFromInspector[i]) as GameObject;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
