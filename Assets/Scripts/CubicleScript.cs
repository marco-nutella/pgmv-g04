using UnityEngine;

public class CubicleScript : UtilityScript
{
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    public float moveDistance = 1.75f; // Distância de abertura/fechamento
    [SerializeField]
    public bool abreParaEsquerda = true; // Define se a porta abre para direita ou esquerda
    private bool aberta = false; // Estado atual da porta (fechada = false, aberta = true)
    private bool debounce = false;

    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position; // Guarda a posição inicial
    }

    void Update()
    {
        if (OnClickActivation(transform.gameObject)) {
            MoverPorta();
        }
    }

    void MoverPorta()
    {
        Vector3 direcao = abreParaEsquerda ? -transform.forward : transform.forward;
        Vector3 endPosition = posicaoInicial + direcao * moveDistance;
        
        if (debounce) {
            return;
        }
        debounce = true;

        if (!aberta)
        {
            StartCoroutine(TweenGameObject(transform.gameObject, endPosition, 0.5f, animationCurve, callbackResult => {
                debounce = callbackResult;
            }));
            ActivateDescendantLights(transform.parent.gameObject, true);
        }
        else
        {
            StartCoroutine(TweenGameObject(transform.gameObject, posicaoInicial, 0.5f, animationCurve, callbackResult => {
                debounce = callbackResult;
            }));
            ActivateDescendantLights(transform.parent.gameObject, false);
        }

        aberta = !aberta; // Inverte o estado
    }
}
