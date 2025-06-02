using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float flyForce = 1f;
    public float turnRate = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 4f;
        rb.angularDrag = 2f;
        rb.freezeRotation = true; // evita que colisões ou torque rotacionem o objeto
    }

    void FixedUpdate()
    {
        //if (FindObjectOfType<GenerateArm>().isMovingArms) return;

        float moveX = Input.GetAxis("Horizontal"); // esquerda/direita (gira)
        float moveZ = Input.GetAxis("Vertical");   // frente/trás (move)

        // Gira em torno do próprio eixo Y
        transform.Rotate(Vector3.up * moveX * turnRate);

        // Move para frente com base na rotação atual
        Vector3 move = transform.forward * moveZ;
        rb.AddForce(move * moveSpeed, ForceMode.VelocityChange);

        // Voo vertical com Q e Z
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(Vector3.up * flyForce, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            rb.AddForce(Vector3.down * flyForce, ForceMode.VelocityChange);
        }
    }
}
