using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CabinetGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextAsset XmlReference;
    private XmlDocument cabinetData;

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
    private List<GameObject> cabinets = new List<GameObject>();
    private GameObject[] doors;
    private GameObject[] modules;
    [SerializeField]
    private Grid placementGrid; //= gameObject.GetComponent<Grid>()

    private XmlNodeList ReadXmlFile(TextAsset textAsset) {
        cabinetData = new XmlDocument();
        cabinetData.LoadXml(textAsset.text);
        
        return cabinetData.SelectNodes("/Cabinet");
    }

    private List<int> GetCabinetHeights(int width, float height) {
        float heightDiv = height/3.0f;
        float stackedCabinets = Mathf.Ceil(heightDiv); // Lógica de fazer a "stack" das cabinetes caso a altura seja maior do que 3

        List<int> cabinetHeights = new List<int>();
        cabinetHeights.Add((int) stackedCabinets);
        // 1 > 1/3 = 0.33 > 1
        // 2 > 2/3 = 0.67 > 1
        // 5 > 5/3 = 1.67 > 2
        if (stackedCabinets > 1) {
            int i = 1;
            int remainingHeight = (int) height;
            while (remainingHeight > 3) { // temos certeza que vai ser um armário de tamanho 3. poderíamos fazer == 3, mas desta forma podemos executar as linhas asseguir do loop sem precisar verificar se k ~= 0
                cabinetHeights.Add(3);
                remainingHeight-=3; // reduzimos o tamanho de uma cabinete de tamanho integral
                i++;
            }
            
            int heightDivRemainder = remainingHeight%3; // A altura desejada se for menor ou maior que 3, 0 se for 3
            cabinetHeights.Add(heightDivRemainder == 0 ? 3 : heightDivRemainder);
        } else {
            int heightDivRemainder = (int) height%3;
            cabinetHeights.Add(heightDivRemainder == 0 ? 3 : heightDivRemainder);
        }

        return cabinetHeights;
    }

    private void PlaceCabinet() {

    }

    private void PlaceComponent() {
        
    }
    public void GenerateCabinet()
    {
        XmlNodeList cabinnetSettingsList = ReadXmlFile(XmlReference);
        XmlNode cabinetSettings = cabinnetSettingsList.Item(0);

        int width = int.Parse(cabinetSettings.Attributes["Width"].Value);
        int height = int.Parse(cabinetSettings.Attributes["Height"].Value);

        if (width <= 0 || height <= 0) { // Verificar se os valores são válidos
            Debug.LogError("Cabinet width or height are invalid or null. Width: " + width + " Height:" + height );
        }

        List<int> cabinetHeights = GetCabinetHeights(width, (float) height);
        int stackedCabinets = cabinetHeights[0];
        int iters = 0;

        Vector3 rowStartingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z); 
        for (int j = 0; j < stackedCabinets; j++) { // O próprio transform irá mudar de posição e sevirá de "base" para ir botando as cabinetes
           if (j > 0) {
                //transform.position = cabinets[-width+(width*j)].transform.position; // 0 quando para a primeira stack (primeiro armário), width para a segunda stack (primeiro armário da segunda fileira), etc
                transform.position = rowStartingPosition;
                transform.position += transform.up * 5.4f * j; // Posicionar de acordo com a altura, hard-coded por agora, mudar
                rowStartingPosition = transform.position;
           }

            for (int i = 0; i < width; i++) { // Indexado por 1 para ignorar o primeiro item do array, que é a quantidade de cabinetes empilhadas
                int currentCabinetHeight = cabinetHeights[j+1];

                GameObject chosenCabinet = cabinetPrefabsFromInspector[currentCabinetHeight-1];

                transform.position += transform.right * 4; // Posicionar de acordo com a largura, hard-coded por agora, mudar
                cabinets.Add(Instantiate(chosenCabinet, transform.position, transform.rotation)); // + new Vector3(transform.position.x, transform.position.y, transform.position.z)
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

    void Start()
    {
        GenerateCabinet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
