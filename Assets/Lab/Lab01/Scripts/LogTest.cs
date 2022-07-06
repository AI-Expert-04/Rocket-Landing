using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int b;

    private static GameManager me;

    public static GameManager getInstance()
    {
        return me;
    }

    public void QuitGame()
    {

    }

    public void ResetGame()
    {

    }
}

public class LogTest : MonoBehaviour
{
    public int a = 10;

    //classA c;

    // Start is called before the first frame update
    void Start()
    {
        //c = new classA();
        //GameManager.getInstance().ResetGame();
        
        Debug.Log("LogTest Start");
    }

    // Update is called once per frame
    void Update()
    {
        int b = a * a;
        Debug.Log("LogTest Update" + b.ToString());
    }

    private void FixedUpdate()
    {

    }
}
