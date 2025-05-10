using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorPlantas : MonoBehaviour
{
  
    //public string regras = "X->F[+X]F[-X]F";

    // Start is called before the first frame update
    private string plant;
    [SerializeField] private string axiom;
    [SerializeField] private int iterations;
    void Start()
    {
        plant = axiom;
        Debug.Log("Iniciando o Gerador de Plantas" + plant);
        ExpandTreeString();
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
}
