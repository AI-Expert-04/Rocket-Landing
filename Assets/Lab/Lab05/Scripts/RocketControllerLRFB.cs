using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControllerLRFB : MonoBehaviour
{
    public Rigidbody rb;
    public AgentControllerLRFB ac;

    public float mainEngineForce = 20f;
    public float sideEngineForce = 1f;

    public bool mainEngineOn = false;
    public bool leftEngineOn = false;
    public bool rightEngineOn = false;
    public bool forwardEngineOn = false;
    public bool backwardEngineOn = false;

    public GameObject mainEngineFx;
    public GameObject leftEngineFx;
    public GameObject rightEngineFx;
    public GameObject forwardEngineFx;
    public GameObject backwardEngineFx;

    public bool reset = false;
    public bool stop = false;
    public Renderer floorRenderer;

    public float initHeight = 10;
    public float rotaionRange = 0;

    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AgentControllerLRFB>();
        rb = GetComponent<Rigidbody>();
    }

    public void ResetRocket()
    {
        reset = true;
    }

    public void SetMainEngine(int value)
    {
        if (value == 1)
        {
            mainEngineOn = true;
        }
        else
        {
            mainEngineOn = false;
        }
    }

    public void SetLeftEngine(int value)
    {
        if (value == 1)
        {
            leftEngineOn = true;
        }
        else
        {
            leftEngineOn = false;
        }
    }

    public void SetRightEngine(int value)
    {
        if (value == 1)
        {
            rightEngineOn = true;
        }
        else
        {
            rightEngineOn = false;
        }
    }

    public void SetForwardEngine(int value)
    {
        forwardEngineOn = (value == 1);
    }

    public void SetBackwardEngine(int value)
    {
        backwardEngineOn = (value == 1);
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
            transform.localRotation = Quaternion.Euler(
                new Vector3(
                    Random.Range(-rotaionRange, rotaionRange),
                    Random.Range(-rotaionRange, rotaionRange),
                    Random.Range(-rotaionRange, rotaionRange)
                )
            );
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            reset = false;
            stop = false;
            floorRenderer.material.color = Color.white;
            return;
        }

        if (transform.localPosition.y > initHeight * 1.2f || transform.localPosition.y < 0)
        {
            floorRenderer.material.color = Color.red;
            ac.EndEpisode(0);
            return;
        }

        if (rb.IsSleeping())
        {
            if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.up)) > 0.9 &&
                Mathf.Abs(Vector3.Dot(transform.up, Vector3.right)) < 0.1 &&
                Mathf.Abs(Vector3.Dot(transform.up, Vector3.forward)) < 0.1)
            {
                floorRenderer.material.color = Color.green;
                ac.EndEpisode(1);
            }
            else
            {
                floorRenderer.material.color = Color.red;
                ac.EndEpisode(0);
            }
        }

        if (stop)
        {
            return;
        }

        if (mainEngineOn)
        {
            rb.AddForceAtPosition(transform.up * mainEngineForce, transform.position);
            mainEngineFx.SetActive(true);
        }
        else
        {
            mainEngineFx.SetActive(false);
        }

        if (leftEngineOn)
        {
            rb.AddForceAtPosition(transform.right * sideEngineForce, leftEngineFx.transform.position);
        }
        leftEngineFx.SetActive(leftEngineOn);

        if (rightEngineOn)
        {
            rb.AddForceAtPosition(-transform.right * sideEngineForce, rightEngineFx.transform.position);
        }
        rightEngineFx.SetActive(rightEngineOn);

        if (forwardEngineOn)
        {
            rb.AddForceAtPosition(-transform.forward * sideEngineForce, forwardEngineFx.transform.position);
        }
        forwardEngineFx.SetActive(forwardEngineOn);

        if (backwardEngineOn)
        {
            rb.AddForceAtPosition(transform.forward * sideEngineForce, backwardEngineFx.transform.position);
        }
        backwardEngineFx.SetActive(backwardEngineOn);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ac.episodeFinished || stop)
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
            stop = true;
        }
    }
}
