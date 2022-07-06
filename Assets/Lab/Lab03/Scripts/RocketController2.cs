using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController2 : MonoBehaviour
{
    public Rigidbody rb;
    public AgentController2 ac;
    public float force = 20f;
    public bool engineOn = false;

    public bool reset = false;
    public Renderer floorRenderer;
    public GameObject engineFx;

    public float initHeight = 20;

    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AgentController2>();
        rb = GetComponent<Rigidbody>();
    }

    public void ResetRocket()
    {
        reset = true;
    }

    public void OnEngine()
    {
        engineOn = true;
    }

    public void OffEngine()
    {
        engineOn = false;
    }

    private void FixedUpdate()
    {
        if (ac.episodeFinished)
        {
            return;
        }

        if (reset)
        {
            transform.localPosition = new Vector3(0, initHeight, 0);
            rb.velocity = Vector3.zero;

            reset = false;
            floorRenderer.material.color = Color.white;
            return;
        }

        if (engineOn)
        {
            rb.AddForce(Vector3.up * force);
            engineFx.SetActive(true);
        }
        else
        {
            engineFx.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ac.episodeFinished)
        {
            return;
        }

        //Debug.Log(collision.relativeVelocity);
        if (collision.relativeVelocity.y > 10f)
        {
            floorRenderer.material.color = Color.red;
            ac.EndEpisode(0f);
        }
        else
        {
            floorRenderer.material.color = Color.green;
            ac.EndEpisode(1f);
        }
    }
}
