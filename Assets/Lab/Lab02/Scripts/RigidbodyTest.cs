using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTest : MonoBehaviour
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
        Debug.Log(rb.velocity);
        Debug.Log(rb.angularVelocity);
        Debug.Log(rb.IsSleeping());
        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector3(0, 1, 0);
            rb.angularVelocity = new Vector3(1, 0, 0);
        }
    }
}
