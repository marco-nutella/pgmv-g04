using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; 
using UnityEngine.UIElements;

public class GeradorPlantas : MonoBehaviour
{
    //public string regras = "X->F[+X]F[-X]F";

    // Start is called before the first frame update
    private string plant;
    [SerializeField] private string axiom;
    [SerializeField] private string second_axiom;

    [SerializeField] private int iterations;
    Stack<TransformInfo> stack = new Stack<TransformInfo>();
    Stack<int> splineIndexStack = new Stack<int>();
    private TransformInfo helper;
    [SerializeField] private float length;
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;
    // [SerializeField] private float angleYMin;
    // [SerializeField] private float angleYMax;
    [SerializeField] private Material PlantMaterial;
    [SerializeField] private GameObject folhaPrefab;
    [SerializeField] private float folhaOffset = 0.01f;
    
    private List<List<Vector3>> LineList = new List<List<Vector3>>();


    private GameObject plantObject;
    private float velocidadeDoVento= 0.5f;



    void Start()
    {
        plant = Random.value < 0.5f ? axiom : second_axiom;
        //plant = axiom;
        Debug.Log("Iniciando o Gerador de Plantas" + plant);
        ExpandTreeString();
        CreateMesh();
    }
    private void OnDrawGizmos()
    {
        foreach(List<Vector3> line in LineList){
            Gizmos.DrawLine(line[0], line[1]);
        }
    }

    /*void ExpandTreeString(){
        string expandedTree;
        for(int i=0; i<iterations; i++){
            expandedTree = "";
            foreach(char j in plant){
                switch(j)
                {
                    case 'F':
                        if (Random.Range(0f,100f) < 50f){
                            expandedTree += "F";
                        }
                        else
                        {
                            expandedTree += "FF";
                        }
                        break;
                    case 'B':
                        if(Random.Range(0f, 100f) < 50f){
                            expandedTree += "[llFB][rFB]";
                        }
                        else
                        {
                            expandedTree += "[lFB][rrFB]";
                        }
                        break;
                    default:
                        expandedTree += j.ToString();
                        break;
                }
            }
            plant = expandedTree;
            Debug.Log("Iteração " + i + ": " + plant);
        }
    }*/
    void ExpandTreeString() {
    string expandedTree;
    for (int i = 0; i < iterations; i++) {
        expandedTree = "";
        foreach (char j in plant) {
            switch (j) {
                case 'F':
                    // F: move e desenha, com chance de bifurcar
                    if (Random.Range(0f, 100f) < 50f) {
                        expandedTree += "F";
                    } else {
                        expandedTree += "F[+rF][-lF]";
                    }
                    break;

                case 'f':
                    // f: move sem desenhar, mantém a direção
                    expandedTree += "f";
                    break;

                case '+':
                    // +: rotaciona no sentido horário
                    expandedTree += "+";
                    break;

                case '-':
                    // -: rotaciona no sentido anti-horário
                    expandedTree += "-";
                    break;

                 case 'u':
                    // +: rotaciona no sentido horário
                    expandedTree += "u";
                    break;

                case 'd':
                    // -: rotaciona no sentido anti-horário
                    expandedTree += "d";
                    break;

                 case 'l':
                    // +: rotaciona no sentido horário
                    expandedTree += "l";
                    break;

                case 'r':
                    // -: rotaciona no sentido anti-horário
                    expandedTree += "r";
                    break;

                case 'B':
                    // Exemplo de ramificação com lógica estocástica
                    if (Random.Range(0f, 100f) < 50f) {
                        expandedTree += "[++uFFB][-rFB]";
                    } else {
                        expandedTree += "[+uFFB][--dlFFB]";
                    }
                    break;
                case '[':
                    // [: salva estado - mantém
                    expandedTree += "[";
                    break;

                case ']':
                    // ]: restaura estado - mantém
                    expandedTree += "]";    
                    break;

                default:
                    // Qualquer outro caractere permanece igual
                    expandedTree += j.ToString();
                    break;
            }
        }
        plant = expandedTree;
        Debug.Log("Iteração " + i + ": " + plant);
    }
}


    // Update is called once per frame
    void Update()
    {
        //efeito do vento
        //tempoDecorrido += Time.deltaTime * velocidadeDoVento;
        //float intensidadeVento = Mathf.Sin(tempoDecorrido * velocidadeDoVento);
        //foreach(GameObject ramo in ramos) {
        //     ramo.transform.Rotate(Vector3.up, intensidadeVento * 5.0f); // Aplica rotação ao ramo - ajustar o valor conforme necessário
        //}
        

    }
    void CreateMesh(){
        Vector3 initialPosition; 
        //GameObject plantObject = new GameObject("Plant");
        plantObject = new GameObject("Plant");
        var meshFilter = plantObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        var meshRenderer = plantObject.AddComponent<MeshRenderer>();
        meshRenderer.material = PlantMaterial;

        var container = plantObject.AddComponent<SplineContainer>();
        container.RemoveSplineAt(0);
        var extrude = plantObject.AddComponent<SplineExtrude>(); // Add SplineExtrude component
        extrude.Container = container;
        extrude.Radius = 0.05f;

        var currentSpline = container.AddSpline();
        var splineIndex = container.Splines.FindIndex(currentSpline);

        currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);

        foreach(char j in plant){
            switch(j){
                case 'F':
                    // initialPosition = transform.position;
                    transform.Translate(Vector3.up*length);
                    // LineList.Add(new List<Vector3>(){initialPosition, transform.position});
                    // initialPosition = transform.position;
                    currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);
                    // Instancia folha com aleatoriamente
                    /* if (Random.value < 0.3f) // 30% de chance
                    {
                        Vector3 pos = transform.position - transform.up * folhaOffset;
                        Quaternion rot = Quaternion.LookRotation(transform.up, transform.forward);
                        //Quaternion rot = Quaternion.LookRotation(transform.forward);
                        GameObject folha = Instantiate(folhaPrefab, pos, rot, plantObject.transform);
                        folha.transform.up = transform.up;
                    } */
                    if (Random.value < 0.3f)
                    {
                        float lateralOffset = 0.03f;
                        float verticalOffset = 0.01f;

                        for (int i = -1; i <= 1; i += 2)
                        {
                            Vector3 side = transform.right * i * lateralOffset;
                            Vector3 back = -transform.up * verticalOffset;
                            Vector3 pos = transform.position + side + back;

                            Quaternion rot = Quaternion.LookRotation(transform.forward, transform.up);
                            GameObject folha = Instantiate(folhaPrefab, pos, rot, plantObject.transform);
                            folha.transform.up = transform.up;

                            // Corrige o offset com base no centro do mesh
                            Renderer rend = folha.GetComponentInChildren<Renderer>();
                            if (rend != null)
                            {
                                Vector3 meshCenterOffset = rend.bounds.center - folha.transform.position;
                                folha.transform.position -= meshCenterOffset;
                            }
                        }
                    }


                    break;
                case 'B':
                    break;
                case '[':
                    stack.Push(new TransformInfo(){ // new dá warning, mas está a funcionar
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    splineIndexStack.Push(splineIndex);
                    // Ligar a duas splines
                    int splineCount = currentSpline.Count;
                    int prevSplineIndex = splineIndex;
                    currentSpline = container.AddSpline();
                    splineIndex = container.Splines.FindIndex(currentSpline);

                    currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);
                    container.LinkKnots(new SplineKnotIndex(prevSplineIndex, splineCount - 1), new SplineKnotIndex(splineIndex, 0));

                    break;
                case ']':
                    TransformInfo helper = stack.Pop();
                    transform.position = helper.position;
                    transform.rotation = helper.rotation;
                    splineIndex = splineIndexStack.Pop();
                    currentSpline = container.Splines[splineIndex];
                    break;
                case '+':
                    transform.Rotate(Vector3.back, Random.Range(angleMin, angleMax));
                    // transform.Rotate(Vector3.up, Random.Range(angleYMin, angleYMax));
                    break;
                case '-':
                    transform.Rotate(Vector3.forward, Random.Range(angleMin, angleMax));
                    // transform.Rotate(Vector3.up, Random.Range(angleYMin, angleYMax));
                    break;
                case 'u':
                    transform.Rotate(Vector3.up, Random.Range(angleMin, angleMax));
                    // transform.Rotate(Vector3.up, Random.Range(angleYMin, angleYMax));
                    break;
                case 'd':
                    transform.Rotate(Vector3.down, Random.Range(angleMin, angleMax));
                    // transform.Rotate(Vector3.up, Random.Range(angleYMin, angleYMax));
                    break;
                case 'l':
                    transform.Rotate(Vector3.left, Random.Range(angleMin, angleMax));
                    // transform.Rotate(Vector3.up, Random.Range(angleYMin, angleYMax));
                    break;
                case 'r':
                    transform.Rotate(Vector3.right, Random.Range(angleMin, angleMax));
                    // transform.Rotate(Vector3.up, Random.Range(angleYMin, angleYMax));
                    break;

            }
        }
    }
}
public static class PlantGeradorExtension
{
    public static int FindIndex(this IReadOnlyList<Spline> splines, Spline spline)
    {
        for (int i = 0; i < splines.Count; i++)
        {
            if (splines[i] == spline)
            {
                return i;
            }
        }
        return -1;
    }
}