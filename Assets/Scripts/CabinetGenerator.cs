using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;

public class OutOfBoundsSizeException : Exception
{
    public OutOfBoundsSizeException()
    {
    }

    public OutOfBoundsSizeException(string message)
        : base(message)
    {
    }

    public OutOfBoundsSizeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

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

    private Dictionary<string, dynamic> ReadCabinetSettingsFromXml(TextAsset xmlAsset) {
        XmlDocument cabinetXmlDocument = new XmlDocument();
        cabinetXmlDocument.LoadXml(xmlAsset.text);

        XmlNode cabinetSettings = cabinetXmlDocument.DocumentElement;

        int width = int.Parse(cabinetSettings.Attributes["Width"].Value);
        int height = int.Parse(cabinetSettings.Attributes["Height"].Value);

        if (width <= 0 || height <= 0) { // Verificar se os valores são válidos
            //Debug.LogError("Cabinet width or height are invalid or null. Width: " + width + " Height:" + height);
            throw new OutOfBoundsSizeException("Cabinet width or height are invalid or null. Width: " + width + " Height:" + height);
        }

        try {
            XmlNode firstLine = cabinetXmlDocument.SelectNodes("Cabinet/line")[0];
            List<int> componentsList = SpacesIdentedXmlStringToIntList(firstLine.InnerText);

            if (componentsList.Count != height) {
                throw new OutOfBoundsSizeException("Components per cabinet line does not match height. Components per cabinet line: " + componentsList.Count + " Height:" + height);
            }
             
        } catch (Exception e) { // https://learn.microsoft.com/pt-br/dotnet/csharp/language-reference/statements/exception-handling-statements
            Debug.LogException(e, this);
            throw;
        }

        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
        dict.Add("width", width);
        dict.Add("height", height);
        dict.Add("xmlDocument", cabinetXmlDocument);
        return dict;
    }

    private void PlaceCabinets(int width, int height) { // Metodo overloaded que nao requer o xmlDict, para uso por qualquer motivo
        List<int> cabinetHeights = GetCabinetHeights(width, (float) height);
        int stackedCabinets = cabinetHeights[0];

        transform.position += -transform.right * (4*2);
        Vector3 rowStartingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z); 
        for (int j = 0; j < stackedCabinets; j++) { 
            int currentCabinetHeight = cabinetHeights[j+1];
            GameObject chosenCabinet = cabinetPrefabsFromInspector[currentCabinetHeight-1];

           if (j > 0) {
                transform.position = rowStartingPosition;
                transform.position += transform.up * (5.4f*2); 
                rowStartingPosition = transform.position; 
           }

            for (int i = 0; i < width; i++) { 
                transform.position += transform.right * (4*2); 
                cabinets.Add(Instantiate(chosenCabinet, transform.position, transform.rotation)); 
            }
        }
    }

    private void PlaceCabinets(Dictionary<string, dynamic> xmlDict) {
        int width = xmlDict["width"];
        int height = xmlDict["height"];

        List<int> cabinetHeights = GetCabinetHeights(width, (float) height);
        int stackedCabinets = cabinetHeights[0];

        transform.position += -transform.right * (4*2); // Para fazer o off-set do "ir à direita" inicial (de novo, tenho de mudar isso para nao ser hard-coded...)
        Vector3 rowStartingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z); 
        for (int j = 0; j < stackedCabinets; j++) { // O próprio transform irá mudar de posição e sevirá de "base" para ir botando as cabinetes
            int currentCabinetHeight = cabinetHeights[j+1];
            GameObject chosenCabinet = cabinetPrefabsFromInspector[currentCabinetHeight-1];

           if (j > 0) {
                //transform.position = cabinets[-width+(width*j)].transform.position; // 0 quando para a primeira stack (primeiro armário), width para a segunda stack (primeiro armário da segunda fileira), etc
                transform.position = rowStartingPosition;
                transform.position += transform.up * (5.4f*2); // * j Posicionar de acordo com a altura, hard-coded por agora, mudar
                rowStartingPosition = transform.position; 
           }

            for (int i = 0; i < width; i++) { // Indexado por 1 para ignorar o primeiro item do array, que é a quantidade de cabinetes empilhadas
                transform.position += transform.right * (4*2); // Posicionar de acordo com a largura, hard-coded por agora, mudar
                cabinets.Add(Instantiate(chosenCabinet, transform.position, transform.rotation)); // + new Vector3(transform.position.x, transform.position.y, transform.position.z)
                PlaceComponent(xmlDict["xmlDocument"]);
            }
        }
    }

    private List<int> SpacesIdentedXmlStringToIntList(string text) {
        string[] componentsStringArray = text.Split(' ');
        List<int> componentsIntList = new List<int>();

        foreach(string component in componentsStringArray) {
            componentsIntList.Add(int.Parse(component));
        }

        return componentsIntList;
    }

    private void PlaceComponent(XmlDocument xmlDoc) {
        /*
            1 - Ler todas as linhas dentro do elemento "Cabinet"
            2 - Para cada linha, identificar todos os objetos, separados por espaços 
            3 - Para cada objeto, encontrar a cabinete/armario a qual pertence
            3.5 - Esse armario vai ser igual a 0 + o numero da linha na lista "cabinets", e + 3 a cada 3 objetos lidos na linha atual (objNº%3)
            4 - Colocar o objeto na posiçao adequada para seu objNº, no espaço local do armario, como child. Vai ser algum numero representando Y vezes objNº%3
            5 - Repetir para todos os objetos
        */

        XmlNodeList linesList = xmlDoc.SelectNodes("Cabinet/line"); // 1
        for (int i = 0; i < linesList.Count; i++) {
            XmlNode node = linesList[i];
            List<int> componentsList = SpacesIdentedXmlStringToIntList(node.InnerText); // 2
            
            for (int compIdx = 0; compIdx < componentsList.Count; compIdx++) {
                int heightModifier = compIdx/3; // 3.5
                GameObject parentCabinet = cabinets[i+(3*heightModifier)]; // 3
                
                
                
            }
            
        }
    }

    public void GenerateCabinet()
    {
        Dictionary<string, dynamic> cabinetSettingsDict = ReadCabinetSettingsFromXml(XmlReference);
        PlaceCabinets(cabinetSettingsDict);



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
