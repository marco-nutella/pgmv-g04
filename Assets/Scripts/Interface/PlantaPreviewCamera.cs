using UnityEngine;

public class PlantaPreviewCamera : MonoBehaviour
{
    public Transform holdPoint; // Referência ao holdPoint
    public Vector3 offset = new Vector3(0, 0.2f, -0.5f); // Distância da câmera
    public Vector3 lookOffset = new Vector3(0, 0.1f, 0); // Para mirar no centro da planta

    void LateUpdate()
    {
        if (holdPoint.childCount > 0)
        {
            Transform planta = holdPoint.GetChild(0);
            transform.position = planta.position + offset;
            transform.LookAt(planta.position + lookOffset);
        }
        else
        {
            // Ocultar câmera ou apontar para longe se nada estiver no holdPoint
        }
    }
}
