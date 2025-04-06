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
    private Grid placementGrid = gameObject.GetComponent<Grid>();
    void Start()
    {
      
    }


    private int[] GetCabinetHeights(int width, int height) {
        int stackedCabinets = Mathf.Ceil(height/3); // Lógica de fazer a "stack" das cabinetes caso a altura seja maior do que 3
        int[] cabinetHeights = {stackedCabinets};
        // 1 > 1/3 = 0.33 > 1
        // 2 > 2/3 = 0.67 > 1
        // 5 > 5/3 = 1.67 > 2
        if (stackedCabinets > 1) {
            int i = 1;
            int remainingHeight = height;
            while (remainingHeight > 3) { // temos certeza que vai ser um armário de tamanho 3. poderíamos fazer == 3, mas desta forma podemos executar as linhas asseguir do loop sem precisar verificar se k ~= 0
                cabinetHeights[i] = 3;
                remainingHeight-=3; // reduzimos o tamanho de uma cabinete de tamanho integral
                i++;
            }
            
            int heightDivRemainder = remainingHeight%3; // A altura desejada se for menor ou maior que 3, 0 se for 3
            cabinetHeights[i] = heightDivRemainder == 0 ? 3 : heightDivRemainder;
        } else {
            int heightDivRemainder = height%3;
            cabinetHeights[1] = heightDivRemainder == 0 ? 3 : heightDivRemainder;
        }

        return cabinetHeights
    }

    private void PlaceCabinet() {

    }

    private void PlaceComponent() {
        
    }
    public void GenerateCabinet(XmlNode cabinetSettings)
    {
        int width = int.Parse(cabinetSettings.Attributes["Width"].Value);
        int height = int.Parse(cabinetSettings.Attributes["Height"].Value);

        if (width <= 0 || height <= 0) { // Verificar se os valores são válidos
            Debug.LogError("Cabinet width or height are invalid or null. Width: " + width + " Height:" + height );
        }

        int[] cabinetHeights = GetCabinetHeights(width, height);
        int stackedCabinets = cabinetHeights[0];
        int iters = 0;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < stackedCabinets; j++) {
                int currentCabinetHeight = cabinetHeights[j]

                GameObject chosenCabinet = cabinetPrefabsFromInspector[currentCabinetHeight-1];
                cabinets[iters] = new GameObject;
                iters++;
            }
        }
        /* https://discussions.unity.com/t/how-do-you-set-prefabs-into-a-gameobject-array-that-will-activate-deactivate-when-called/180007
         https://gamedev.stackexchange.com/questions/142451/how-to-instantiate-an-array-of-prefabs-in-c-script
        {
             cabinets[i] = Instantiate(cabinetPrefabsFromInspector[i]) as GameObject;
        }*/

        /*
        para posicionar os armários: 
        0 - garantir que todos os componentes do armário atual já tenham sido posicionados
        1 - definir o tipo de armário de deverá ser colocado para a altura atual (cabinetHeights)
        2 - encontrar o "snapping point" da grid na horizontal (em coordenadas globais)
        3 - colocar o armário adjacente nessa posição com (GetCellCenterWorld)
        4 - colocar componentes no armário novo
        5 - repetir para o próximo armário

        para posicionar os componentes:
        1 - definir o tipo de componente que deverá ser encaixado 
        2 - utilizando as coordenadas locais do armário, encontrar o "snapping point" da grid
        3 - colocar o objeto nessa posição com (GetCellCenterWorld)
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
