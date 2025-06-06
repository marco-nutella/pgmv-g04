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

    private List<Transform> ramos = new List<Transform>();
    private List<Transform> folhas = new List<Transform>();
    private float tempoDecorrido = 0f;
    private List<Quaternion> folhasRotacoesOriginais = new List<Quaternion>();
    

    private TransformInfo helper;
    [SerializeField] private float length;
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;

    [SerializeField] private float velocidadeDoVento = 1f;

    // [SerializeField] private float angleYMin;
    // [SerializeField] private float angleYMax;
    [SerializeField] private Material PlantMaterial;
    [SerializeField] private GameObject Leaf;
    [SerializeField] private GameObject flower;
    [SerializeField] private float folhaOffset = 0.01f;

    // [SerializeField] private float branchWidth = 0.1f;
    // [SerializeField] private float leafScale = 1.0f;
    // [SerializeField] private float leafProbability = 0.7f;
    private List<List<Vector3>> LineList = new List<List<Vector3>>();


    private Vector3 StartTransformPosition;
    private Quaternion StartTransformRotation;
    private Vector3 StartTransformScale;
    private GameObject SpawnedSplines;
    private List<GameObject> SpawnedObjectList = new List<GameObject>();


    private GameObject plantObject;
    //private float velocidadeDoVento= 0.5f;

    public void DeletePlant()
    {
        if (SpawnedSplines != null)
        {
            Destroy(SpawnedSplines);
        }
        if (SpawnedObjectList.Count > 0)
        {
            foreach (GameObject obj in SpawnedObjectList)
            {
                Destroy(obj);
            }
        }

        transform.localPosition = StartTransformPosition;
        transform.localRotation = StartTransformRotation;
        transform.localScale = StartTransformScale;
    }

    public void CreatePlant(int? axiom_number = null)
    {
        if (axiom_number != null && axiom_number == 1)
        {
            plant = axiom;
        }
        else if (axiom_number != null && axiom_number == 2)
        {
            plant = second_axiom;
        }
        else
        {
            plant = Random.value < 0.5f ? axiom : second_axiom;
        }
        
        //plant = axiom;
        Debug.Log("Iniciando o Gerador de Plantas" + plant);
        ExpandTreeString();
        CreateMesh();
    }

    void Start()
    {
        StartTransformPosition = transform.localPosition;
        StartTransformRotation = transform.localRotation;
        StartTransformScale = transform.localScale;
        CreatePlant();
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
        /*  tempoDecorrido += Time.deltaTime;

         float intensidade = Mathf.Sin(tempoDecorrido * velocidadeDoVento) * 5f; // graus

        foreach (var ramo in ramos)
        {
            if (ramo != null)
                 ramo.localRotation = Quaternion.Euler(0f, intensidade * 0.2f, intensidade);
        }

        foreach (var folha in folhas)
        {
            if (folha != null)
                 folha.localRotation = Quaternion.Euler(intensidade * 0.3f, intensidade * 0.5f, intensidade * 0.2f);
         } */
        tempoDecorrido += Time.deltaTime * velocidadeDoVento;

        for (int i = 0; i < folhas.Count; i++)
        {
            if (folhas[i] != null)
            {
                Quaternion oscilacao = Quaternion.Euler(
                    Mathf.Sin(tempoDecorrido + i) * 10f,
                    Mathf.Cos(tempoDecorrido + i * 0.3f) * 10f,
                    Mathf.Sin(tempoDecorrido + i * 0.6f) * 5f
                );

                folhas[i].localRotation = folhasRotacoesOriginais[i] * oscilacao;
            }
        }

    }
    void CreateMesh(){
        Vector3 initialPosition; 
        //GameObject plantObject = new GameObject("Plant");
        plantObject = new GameObject("Plant");
        plantObject.transform.parent = transform.parent;
        var meshFilter = plantObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        var meshRenderer = plantObject.AddComponent<MeshRenderer>();
        meshRenderer.material = PlantMaterial;

        // var container = plantObject.AddComponent<SplineContainer>();
        // container.RemoveSplineAt(0);
        // var extrude = plantObject.AddComponent<SplineExtrude>(); // Add SplineExtrude component
        // extrude.Container = container;
        // extrude.Radius = 0.05f;

        // var currentSpline = container.AddSpline();
        var container = plantObject.AddComponent<SplineContainer>();
        SpawnedSplines = plantObject;

        var currentSpline = container.AddSpline();
        currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);

        var extrude = plantObject.AddComponent<SplineExtrude>();
        extrude.Container = container;
        extrude.Radius = 0.05f;


        var splineIndex = container.Splines.FindIndex(currentSpline);

        currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);

        foreach(char j in plant){
            switch(j){
                case 'F':
                    // initialPosition = transform.position;
                    transform.Translate(Vector3.up*length);

                    ramos.Add(transform); // Guarda este ramo
                    // LineList.Add(new List<Vector3>(){initialPosition, transform.position});
                    // initialPosition = transform.position;
                    currentSpline.Add(new BezierKnot(transform.position), TangentMode.AutoSmooth);

                    if (Random.value < 0.3f)
                    {

                        float lateralOffset = 0.02f; // Ajuste fino
                        float verticalOffset = 0.005f; // Mais colado ao ramo


                        for (int i = -1; i <= 1; i += 2)
                        {
                            Vector3 side = transform.right * i * lateralOffset;
                            Vector3 back = -transform.up * verticalOffset;
                            Vector3 pos = transform.position + side + back;

                            // Folha aponta para fora do ramo
                            Quaternion rot = Quaternion.LookRotation(i * transform.right, transform.up);

                            GameObject folha = Instantiate(Leaf, pos, rot, plantObject.transform);
                            folha.transform.parent = transform.parent;
                            folhas.Add(folha.transform);


                            folha.transform.up = transform.up;

                            // Ajuste fino de centro do mesh da folha
                            Renderer rend = folha.GetComponentInChildren<Renderer>();
                            if (rend != null)
                            {
                                Vector3 meshCenterOffset = rend.bounds.center - folha.transform.position;
                                folha.transform.position -= meshCenterOffset;
                            }
                            float scale = Random.Range(0.8f, 1.2f); // Escala aleatória para a folha
                            folha.transform.localScale *= scale;

                            // Rotação aleatória
                            folha.transform.Rotate(Vector3.up, Random.Range(-15f, 15f), Space.Self);


                            folhasRotacoesOriginais.Add(folha.transform.localRotation); // Guarda a rotação original da folha
                            SpawnedObjectList.Add(folha);
                        }
                    }


                    // Flor no final do ramo
                    if (flower != null && Random.value < 0.5f) // chance de flor
                    {
                        Vector3 florPos = transform.position; // ligeiramente acima
                        Quaternion florRot = Quaternion.LookRotation(transform.up);  // aponta na direção do ramo
                        GameObject flor = Instantiate(flower, florPos, florRot, plantObject.transform);
                        flor.transform.parent = transform.parent;

                        // flor.transform.Rotate(Vector3.right, 90); // depende da orientação do prefab
                        flor.transform.up = transform.up;

                        Renderer rendFlor = flor.GetComponentInChildren<Renderer>();
                        if (rendFlor != null)
                        {
                            Vector3 meshCenterOffset = rendFlor.bounds.center - flor.transform.position;
                            flor.transform.position -= meshCenterOffset;
                        }

                        // Escala aleatória leve
                        flor.transform.localScale *= Random.Range(0.9f, 1.1f);
                        SpawnedObjectList.Add(flor);
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

    
    public string GetAxiom() => axiom;
    public void SetAxiom(string value) => axiom = value;

    public string GetSecondAxiom() => second_axiom;
    public void SetSecondAxiom(string value) => second_axiom = value;

    public int GetIterations() => iterations;
    public void SetIterations(int value) => iterations = value;

    public float GetLength() => length;
    public void SetLength(float value) => length = value;

    public float GetAngleMin() => angleMin;
    public void SetAngleMin(float value) => angleMin = value;

    public float GetAngleMax() => angleMax;
    public void SetAngleMax(float value) => angleMax = value;

    public float GetVelocidadeDoVento() => velocidadeDoVento;
    public void SetVelocidadeDoVento(float value) => velocidadeDoVento = value;
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
