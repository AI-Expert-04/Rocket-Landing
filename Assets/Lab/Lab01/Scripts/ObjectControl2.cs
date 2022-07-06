using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 pos = new Vector3(1, 1, 1);
        //transform.position = pos;

        transform.position = new Vector3(2, transform.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(45, 0, 0);
        //transform.rotation.ToEuler().x
        transform.localScale = new Vector3(2, 1, 1);
    }
}
