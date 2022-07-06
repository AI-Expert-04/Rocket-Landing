using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int a = 10;
    char b;
    float c;
    double d;
    string e;
    GameObject f;
    Rigidbody g;
    Renderer h;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Test Start");
    }

    // Update is called once per frame
    void Update()
    {
        a += 10;
        int b = a;

        Debug.Log("Test Update");
    }

    private void FixedUpdate()
    {
        Debug.Log("Test FixedUpdate");
    }
}
