using UnityEngine;

public class PortaScript : MonoBehaviour
{
    private float moveDistance = 1.3f; // Distância de abertura/fechamento
    public bool abreParaEsquerda = true; // Define se a porta abre para direita ou esquerda
    private bool aberta = false; // Estado atual da porta (fechada = false, aberta = true)

    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.localPosition; // Guarda a posição inicial
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique esquerdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    MoverPorta();
                }
            }
        }
    }

    void MoverPorta()
    {
        Vector3 direcao = abreParaEsquerda ? Vector3.right : Vector3.left;
        
        if (!aberta)
        {
            transform.localPosition = posicaoInicial + direcao * moveDistance;
        }
        else
        {
            transform.localPosition = posicaoInicial;
        }

        aberta = !aberta; // Inverte o estado
    }
}
