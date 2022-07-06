using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentController2 : Agent
{
    public EnvironmentParameters m_ResetParams;
    public RocketController2 rc;
    public bool episodeFinished = false;

    public override void Initialize()
    {
        rc = GetComponent<RocketController2>();
    }

    public override void OnEpisodeBegin()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        float init_height = m_ResetParams.GetWithDefault("init_height", 20);
        if (init_height != rc.initHeight)
        {
            Debug.Log("높이 변경:" + rc.initHeight + " -> " + init_height);
            rc.initHeight = init_height;
        }
        rc.ResetRocket();
        episodeFinished = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 rocketPosition = rc.transform.localPosition;
        Vector3 rocketVelocity = rc.rb.velocity;

        sensor.AddObservation(rocketPosition.x);
        sensor.AddObservation(rocketPosition.y);
        sensor.AddObservation(rocketPosition.z);

        sensor.AddObservation(rocketVelocity.x);
        sensor.AddObservation(rocketVelocity.y);
        sensor.AddObservation(rocketVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (actionBuffers.DiscreteActions[0] == 1)
        {
            rc.OnEngine();
        }
        else
        {
            rc.OffEngine();
        }
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
    }
}
