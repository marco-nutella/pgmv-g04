using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GeradorPlantas : MonoBehaviour
{
    //public string regras = "X->F[+X]F[-X]F";

    // Start is called before the first frame update
    private string plant;
    [SerializeField] private string axiom;

    [SerializeField] private int iterations;
    Stack<TransformInfo> stack = new Stack<TransformInfo>();
    private TransformInfo helper;
    [SerializeField] private float length;
    [SerializeField] private float angle;
    private List<List<Vector3>> LineList = new List<List<Vector3>>();

    void Start()
    {
        plant = axiom;
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

    void ExpandTreeString(){
        string expandedTree;
        for(int i=0; i<iterations; i++){
            expandedTree = "";
            foreach(char j in plant){
                expandedTree += j switch
                {
                    'F' => "FF",
                    'B' => "[lFB][rFB]",
                    _ => j.ToString()
                };
            }
            plant = expandedTree;
            Debug.Log("Iteração " + i + ": " + plant);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // //efeito do vento
        // tempoDecorrido += Time.deltaTime * velocidadeDoVento;
        // float intensidadeVento = Mathf.Sin(tempoDecorrido * velocidadeDoVento);
        // foreach(GameObject ramo in ramos) {
        //     ramo.transform.Rotate(Vector3.up, intensidadeVento * 5.0f); // Aplica rotação ao ramo - ajustar o valor conforme necessário
        // }
    }
    void CreateMesh(){
        Vector3 initialPosition; 
        foreach(char j in plant){
            switch(j){
                case 'F':
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up*length);
                    LineList.Add(new List<Vector3>(){initialPosition, transform.position});
                    initialPosition = transform.position;

                    break;
                case 'B':
                    break;
                case '[':
                    stack.Push(gameObject.AddComponent<TransformInfo>());
                    break;
                case ']':
                    TransformInfo helper = stack.Pop();
                    transform.position = helper.position;
                    transform.rotation = helper.rotation;
                    break;
                case 'l':
                    transform.Rotate(Vector3.back, angle);
                    break;
                case 'r':
                    transform.Rotate(Vector3.forward, angle);
                    break;

            }
        }
    }
}
