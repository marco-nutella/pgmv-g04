using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControler : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float turn_rate = 300;
    [SerializeField] bool use_physics = true;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.isKinematic = !use_physics;
        rb.useGravity = use_physics;

        // Verifica se as teclas de subida ou descida estão pressionadas
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0, 1 * speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(0, -1 * speed * Time.deltaTime, 0);
        }

        if (!use_physics)
        {
            //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), moveUp, Input.GetAxis("Vertical"));

            // Aplica o movimento ao jogador
            //transform.Translate(movement * speed * Time.deltaTime);

            transform.Translate(Vector3.forward  * Input.GetAxis("Vertical") * speed * Time.deltaTime, Space.Self);
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * turn_rate * Time.deltaTime, Space.Self);

        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * speed, ForceMode.Force);
            rb.AddRelativeTorque(Vector3.up * Input.GetAxis("Horizontal") * turn_rate, ForceMode.Force);
        }

        if (transform.position.y < -5)
        {
            transform.position = Vector3.zero;
        }
     
    }
}
