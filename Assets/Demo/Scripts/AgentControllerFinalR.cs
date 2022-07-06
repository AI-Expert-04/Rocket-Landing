using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentControllerFinalR : Agent
{
    public EnvironmentParameters environmentParameters;
    
    public RocketControllerFinalR rc;
    public bool episodeFinished = false;

    public override void Initialize()
    {
        rc = GetComponent<RocketControllerFinalR>();
    }

    public override void OnEpisodeBegin()
    {
        //environmentParameters = Academy.Instance.EnvironmentParameters;
        //rc.initHeight = environmentParameters.GetWithDefault("init_height", 10);
        //rc.rotationRange = environmentParameters.GetWithDefault("rotation_range", 0);
        rc.ResetRocket();
        episodeFinished = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 rocketPosition = rc.transform.localPosition;
        Vector3 rocketRotation = rc.transform.localRotation.eulerAngles;
        Vector3 rocketVelocity = rc.rb.velocity;
        Vector3 rocketAngularVelocity = rc.rb.angularVelocity;

        sensor.AddObservation(rocketPosition.x);
        sensor.AddObservation(rocketPosition.y);
        sensor.AddObservation(rocketPosition.z);

        sensor.AddObservation(rocketRotation.x);
        sensor.AddObservation(rocketRotation.y);
        sensor.AddObservation(rocketRotation.z);

        sensor.AddObservation(rocketVelocity.x);
        sensor.AddObservation(rocketVelocity.y);
        sensor.AddObservation(rocketVelocity.z);

        sensor.AddObservation(rocketAngularVelocity.x);
        sensor.AddObservation(rocketAngularVelocity.y);
        sensor.AddObservation(rocketAngularVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        rc.SetMainEngine(actionBuffers.DiscreteActions[0]);
        if (actionBuffers.DiscreteActions[1] == 0)
        {
            rc.SetLeftEngine(0);
            rc.SetRightEngine(0);
        }
        else if (actionBuffers.DiscreteActions[1] == 1)
        {
            rc.SetLeftEngine(1);
            rc.SetRightEngine(0);
        }
        else if (actionBuffers.DiscreteActions[1] == 2)
        {
            rc.SetLeftEngine(0);
            rc.SetRightEngine(1);
        }

        if (actionBuffers.DiscreteActions[2] == 0)
        {
            rc.SetForwardEngine(0);
            rc.SetBackwardEngine(0);
        }
        else if (actionBuffers.DiscreteActions[2] == 1)
        {
            rc.SetForwardEngine(1);
            rc.SetBackwardEngine(0);
        }
        else if (actionBuffers.DiscreteActions[2] == 2)
        {
            rc.SetForwardEngine(0);
            rc.SetBackwardEngine(1);
        }
    }

    public void EndEpisode(float reward)
    {
        AddReward(reward);

        episodeFinished = true;
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(1f);
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsBuffers)
    {
        var actionsOut = actionsBuffers.DiscreteActions;
        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut[0] = 1;
        }
        else
        {
            actionsOut[0] = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[1] = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                actionsOut[1] = 2;
            }
            else
            {
                actionsOut[1] = 0;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[2] = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                actionsOut[2] = 2;
            }
            else
            {
                actionsOut[2] = 0;
            }
        }
    }
}
