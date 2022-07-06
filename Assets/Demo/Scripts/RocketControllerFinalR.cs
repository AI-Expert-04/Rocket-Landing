using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControllerFinalR : MonoBehaviour
{
    public Rigidbody rb;
    public AgentControllerFinalR ac;

    public GameObject landingZoneNormal;
    public GameObject landingZoneSuccess;
    public GameObject landingZoneFail;

    public float mainEngineForce = 20f;
    public float sideEngineForce = 0.5f;

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
    
    //public float goalSpeed = 1f;
    public float initHeight = 20;
    public float rotationRange = 0;
    public float xzRange = 0;

    public Vector3 previousVelocity = Vector3.positiveInfinity;
    public float previousDistance = float.PositiveInfinity;
    public float previousAngle = float.PositiveInfinity;

    public bool fx = false;

    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<AgentControllerFinalR>();
        rb = GetComponent<Rigidbody>();

        mainEngineParticleSystem = mainEngineFx.GetComponent<ParticleSystem>().main;
        leftEngineParticleSystem = leftEngineFx.GetComponent<ParticleSystem>().main;
        rightEngineParticleSystem = rightEngineFx.GetComponent<ParticleSystem>().main;
        forwardEngineParticleSystem = forwardEngineFx.GetComponent<ParticleSystem>().main;
        backwardEngineParticleSystem = backwardEngineFx.GetComponent<ParticleSystem>().main;

        mainEngineParticleSystemRenderer = mainEngineFx.GetComponent<ParticleSystemRenderer>();

        if (!fx)
        {
            mainEngineFx.SetActive(false);
            leftEngineFx.SetActive(false);
            rightEngineFx.SetActive(false);
            forwardEngineFx.SetActive(false);
            backwardEngineFx.SetActive(false);
        }
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
        if (fx)
        {
            LandingLeg1.transform.localRotation = Quaternion.Euler(new Vector3(0, 120f * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0));
            LandingLeg2.transform.localRotation = Quaternion.Euler(new Vector3(120f * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0, -90));
            LandingLeg3.transform.localRotation = Quaternion.Euler(new Vector3(0, -120 * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, -180));
            LandingLeg4.transform.localRotation = Quaternion.Euler(new Vector3(-120 * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0, 90));
        }

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
            float positionTheta = Random.Range(-Mathf.PI, Mathf.PI);
            float rotationTheta = Random.Range(-Mathf.PI, Mathf.PI);

            transform.localPosition = new Vector3(Mathf.Sin(positionTheta) * xzRange, initHeight, Mathf.Cos(positionTheta) * xzRange);
            transform.localRotation = Quaternion.Euler(Mathf.Sin(rotationTheta) * rotationRange, (rotationRange == 0 ? 0 : 1) * Random.Range(0f, 360f), Mathf.Cos(rotationTheta) * rotationRange);
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = Vector3.zero;

            //goalSpeed = Random.Range(1f, 10f);

            reset = false;
            stop = false;

            previousVelocity = Vector3.positiveInfinity;
            previousDistance = float.PositiveInfinity;
            previousAngle = float.PositiveInfinity;

            landingZoneNormal.SetActive(true);
            landingZoneSuccess.SetActive(false);
            landingZoneFail.SetActive(false);
            return;
        }

        if (transform.localPosition.y > initHeight * 1.2f || transform.localPosition.y < 0 || Vector3.Angle(Vector3.up, transform.up) > 50)
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
                    ac.EndEpisode(1000);
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

        //var velDeltaMagnitude = Mathf.Clamp(Vector3.Distance(rb.velocity, transform.position.normalized * goalSpeed), 0, goalSpeed);

        //var matchSpeedReward = Mathf.Pow(1 - Mathf.Pow(velDeltaMagnitude / goalSpeed, goalSpeed), 2);

        //var lookAtTargetReward = (Vector3.Dot(transform.up, transform.position.normalized) + 1) * .5F;

        //ac.AddReward(matchSpeedReward * lookAtTargetReward);
        //ac.AddReward(lookAtTargetReward);

        if (previousVelocity != Vector3.positiveInfinity && Mathf.Abs(rb.velocity.y) < Mathf.Abs(previousVelocity.y))
        {
            ac.AddReward(0.05f);
        }
        else
        {
            ac.AddReward(-0.05f);
        }

        if (previousDistance != float.PositiveInfinity && Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z)) < previousDistance)
        {
            ac.AddReward(0.05f);
        }
        else
        {
            ac.AddReward(-0.05f);
        }

        if (previousAngle != float.PositiveInfinity && Vector3.Angle(Vector3.up, transform.up) < previousAngle)
        {
            ac.AddReward(0.05f);
        }
        else
        {
            ac.AddReward(-0.05f);
        }

        previousVelocity = rb.velocity;
        previousDistance = Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z));
        previousAngle = Vector3.Angle(Vector3.up, transform.up);

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
            ac.EndEpisode(0);
        }
        else
        {
            stop = true;
        }
    }
}
