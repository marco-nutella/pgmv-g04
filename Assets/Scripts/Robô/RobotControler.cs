using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControler : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float turn_rate = 300;
    [SerializeField] bool use_physics = true;
    [SerializeField] float oscillationAmplitude = 0.5f;
    [SerializeField] float oscillationFrequency = 1.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float oscillation = Mathf.Sin(Time.time * oscillationFrequency) * oscillationAmplitude/1000;
        Vector3 oscillationOffset = new Vector3(0, oscillation, 0);        
        transform.position += oscillationOffset;
    }

    private void FixedUpdate()
    {
        rb.isKinematic = !use_physics;
        rb.useGravity = use_physics;

        if (Input.GetKey(KeyCode.Q)){
            transform.Translate(0, 1 * speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.Z)){
            transform.Translate(0, -1 * speed * Time.deltaTime, 0);
        }

        if (!use_physics)
        {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime, Space.Self);
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