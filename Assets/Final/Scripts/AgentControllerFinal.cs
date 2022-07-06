using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentControllerFinal : Agent
{
    public RocketControllerFinal rc;
    public bool episodeFinished = false;

    public override void Initialize()
    {
        rc = GetComponent<RocketControllerFinal>();
    }

    public override void OnEpisodeBegin()
    {
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
        rc.SetLeftEngine(actionBuffers.DiscreteActions[1]);
        rc.SetRightEngine(actionBuffers.DiscreteActions[2]);
        rc.SetForwardEngine(actionBuffers.DiscreteActions[3]);
        rc.SetBackwardEngine(actionBuffers.DiscreteActions[4]);
    }

    public void EndEpisode(float reward)
    {
        SetReward(reward);

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
            actionsOut[1] = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[2] = 1;
        }
        else
        {
            actionsOut[2] = 0;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[3] = 1;
        }
        else
        {
            actionsOut[3] = 0;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[4] = 1;
        }
        else
        {
            actionsOut[4] = 0;
        }
    }
}
