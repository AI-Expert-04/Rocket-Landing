using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    public Rigidbody rb;
    public Transform left;
    public Transform right;
    public Transform engine;

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
            rb.AddForceAtPosition(transform.up * 10, engine.transform.position);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForceAtPosition(-transform.right, right.transform.position);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForceAtPosition(transform.right, left.transform.position);
        }
    }
}
