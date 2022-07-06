using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move05 : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity = new Vector3(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity = new Vector3(0, 1, 0);
        }
    }
}
