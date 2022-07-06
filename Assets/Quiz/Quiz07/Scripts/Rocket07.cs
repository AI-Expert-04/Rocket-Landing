using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket07 : MonoBehaviour
{
    public Rigidbody rb;

    public float mainEngineForce = 10f;
    public float leftEngineForce = 1f;
    public float rightEngineForce = 1f;

    public bool mainEngineOn = false;
    public bool leftEngineOn = false;
    public bool rightEngineOn = false;

    public Transform leftEnginePosition;
    public Transform rightEnginePosition;

    public Renderer floorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            mainEngineOn = true;
        }
        else
        {
            mainEngineOn = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            leftEngineOn = true;
        }
        else
        {
            leftEngineOn = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rightEngineOn = true;
        }
        else
        {
            rightEngineOn = false;
        }
    }

    private void FixedUpdate()
    {
        if (mainEngineOn)
        {
            rb.AddForceAtPosition(transform.up * mainEngineForce, transform.position);
        }

        if (leftEngineOn)
        {
            rb.AddForceAtPosition(-transform.right * leftEngineForce, leftEnginePosition.position);
        }

        if (rightEngineOn)
        {
            rb.AddForceAtPosition(transform.right * rightEngineForce, rightEnginePosition.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.relativeVelocity.y > 10f) {
            floorRenderer.material.color = Color.red;
        }
        else
        {
            floorRenderer.material.color = Color.green;
        }
    }
}
