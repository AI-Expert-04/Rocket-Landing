using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControllerFinal : MonoBehaviour
{
    public Rigidbody rb;
    public AgentControllerFinal ac;

    public GameObject landingZoneNormal;
    public GameObject landingZoneSuccess;
    public GameObject landingZoneFail;

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

    ParticleSystem.MainModule mainEngineParticleSystem;
    ParticleSystem.MainModule leftEngineParticleSystem;
    ParticleSystem.MainModule rightEngineParticleSystem;
    ParticleSystem.MainModule forwardEngineParticleSystem;
    ParticleSystem.MainModule backwardEngineParticleSystem;

    ParticleSystemRenderer mainEngineParticleSystemRenderer;

    public GameObject LandingLeg1;
    public GameObject LandingLeg2;
    public GameObject LandingLeg3;
    public GameObject LandingLeg4;

    public bool reset = false;
    public bool stop = false;

    public float initHeight = 20;
    public float rotationRange = 0;
    public float xzRange = 0;

    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AgentControllerFinal>();
        rb = GetComponent<Rigidbody>();

        mainEngineParticleSystem = mainEngineFx.GetComponent<ParticleSystem>().main;
        leftEngineParticleSystem = leftEngineFx.GetComponent<ParticleSystem>().main;
        rightEngineParticleSystem = rightEngineFx.GetComponent<ParticleSystem>().main;
        forwardEngineParticleSystem = forwardEngineFx.GetComponent<ParticleSystem>().main;
        backwardEngineParticleSystem = backwardEngineFx.GetComponent<ParticleSystem>().main;

        mainEngineParticleSystemRenderer = mainEngineFx.GetComponent<ParticleSystemRenderer>();
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
        if (value == 1)
        {
            forwardEngineOn = true;
        }
        else
        {
            forwardEngineOn = false;
        }
    }

    public void SetBackwardEngine(int value)
    {
        if (value == 1)
        {
            backwardEngineOn = true;
        }
        else
        {
            backwardEngineOn = false;
        }
    }

    private void FixedUpdate()
    {
        LandingLeg1.transform.localRotation = Quaternion.Euler(new Vector3(0, 120f * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0));
        LandingLeg2.transform.localRotation = Quaternion.Euler(new Vector3(120f * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0, -90));
        LandingLeg3.transform.localRotation = Quaternion.Euler(new Vector3(0, -120 * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, -180));
        LandingLeg4.transform.localRotation = Quaternion.Euler(new Vector3(-120 * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0, 90));

        if (ac.episodeFinished)
        {
            mainEngineParticleSystem.startColor = Color.clear;
            leftEngineParticleSystem.startColor = Color.clear;
            rightEngineParticleSystem.startColor = Color.clear;
            forwardEngineParticleSystem.startColor = Color.clear;
            backwardEngineParticleSystem.startColor = Color.clear;
            return;
        }

        if (reset)
        {
            transform.localPosition = new Vector3(Random.Range(-xzRange, xzRange), initHeight, Random.Range(-xzRange, xzRange));
            transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(-rotationRange, rotationRange), Random.Range(-rotationRange, rotationRange), Random.Range(-rotationRange, rotationRange)));
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            reset = false;
            stop = false;
            landingZoneNormal.SetActive(true);
            landingZoneSuccess.SetActive(false);
            landingZoneFail.SetActive(false);
            return;
        }

        if (transform.localPosition.y > initHeight * 1.2f || transform.localPosition.y < 0)
        {
            landingZoneNormal.SetActive(false);
            landingZoneSuccess.SetActive(false);
            landingZoneFail.SetActive(true);
            ac.EndEpisode(0);
            return;
        }

        if (rb.IsSleeping())
        {
            if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.right)) < 0.1 && Mathf.Abs(Vector3.Dot(transform.up, Vector3.forward)) < 0.1 && Vector3.Dot(transform.up, Vector3.up) > 0.9)
            {
                float distance = Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z));
                if (distance < 3f)
                {
                    landingZoneNormal.SetActive(false);
                    landingZoneSuccess.SetActive(true);
                    landingZoneFail.SetActive(false);
                    ac.EndEpisode(1f - distance / 3f);
                }
                else
                {
                    landingZoneNormal.SetActive(false);
                    landingZoneSuccess.SetActive(false);
                    landingZoneFail.SetActive(true);
                    ac.EndEpisode(0);
                }
            }
            else
            {
                landingZoneNormal.SetActive(false);
                landingZoneSuccess.SetActive(false);
                landingZoneFail.SetActive(true);
                ac.EndEpisode(0);
            }
        }

        if (stop)
        {
            mainEngineParticleSystem.startColor = Color.clear;
            leftEngineParticleSystem.startColor = Color.clear;
            rightEngineParticleSystem.startColor = Color.clear;
            forwardEngineParticleSystem.startColor = Color.clear;
            backwardEngineParticleSystem.startColor = Color.clear;
            return;
        }

        if (mainEngineOn)
        {
            rb.AddForceAtPosition(transform.up * mainEngineForce, transform.position);
            mainEngineParticleSystem.startColor = new Color(255, 119, 0);
        }
        else
        {
            mainEngineParticleSystem.startColor = Color.clear;
        }

        if (leftEngineOn)
        {
            rb.AddForceAtPosition(transform.right * sideEngineForce, leftEngineFx.transform.position);
            leftEngineParticleSystem.startColor = Color.white;
        }
        else
        {
            leftEngineParticleSystem.startColor = Color.clear;
        }

        if (rightEngineOn)
        {
            rb.AddForceAtPosition(-transform.right * sideEngineForce, rightEngineFx.transform.position);
            rightEngineParticleSystem.startColor = Color.white;
        }
        else
        {
            rightEngineParticleSystem.startColor = Color.clear;
        }

        if (forwardEngineOn)
        {
            rb.AddForceAtPosition(-transform.forward * sideEngineForce, forwardEngineFx.transform.position);
            forwardEngineParticleSystem.startColor = Color.white;
        }
        else
        {
            forwardEngineParticleSystem.startColor = Color.clear;
        }

        if (backwardEngineOn)
        {
            rb.AddForceAtPosition(transform.forward * sideEngineForce, backwardEngineFx.transform.position);
            backwardEngineParticleSystem.startColor = Color.white;
        }
        else
        {
            backwardEngineParticleSystem.startColor = Color.clear;
        }

        mainEngineParticleSystemRenderer.lengthScale = Mathf.Clamp(transform.position.y, 4, 8);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ac.episodeFinished || stop)
        {
            return;
        }

        if (collision.relativeVelocity.y > 10f)
        {
            landingZoneNormal.SetActive(false);
            landingZoneSuccess.SetActive(false);
            landingZoneFail.SetActive(true);
            ac.EndEpisode(0f);
        }
        else
        {
            stop = true;
        }
    }
}
