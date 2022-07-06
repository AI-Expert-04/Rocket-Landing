using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(3, 0, 0);
            float x = transform.position.x;
            transform.rotation = Quaternion.Euler(45, 0, 0);
            transform.localScale = new Vector3(2, 1, 1);
        }
        else
        {
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
