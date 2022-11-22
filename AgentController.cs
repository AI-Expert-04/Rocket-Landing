using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents; // MLAgents 페키지.
using Unity.MLAgents.Sensors; // //
using Unity.MLAgents.Actuators;// //

// 부모 클래스
public class AgentController : Agent
{
    // MLAgents 이벤트 파라미터 불러오기
    public EnvironmentParameters environmentParameters; 
    //로켓 컨트롤 연동하는  코드
    public RocketController rc;

    // 에피소드는 꺼저있게 실행.
    // bool은 True, False로만 가능
    public bool episodeFinished = false;

    // 오버라이딩.  Initialize()에서 메서드 재정
    // Agent Class에서 Initialize를 상속받음
    public override void Initialize()
    {
        // 로켓런처 구성 요소 가져오기.
        rc = GetComponent<RocketController>();
    }

    // Agent Class에서 OnEpisodeBegin 상속받음
    public override void OnEpisodeBegin()
    {
        rc.ResetRocket();
        // 에피소드 완료를 비활성화
        episodeFinished = false;
    }

    // Agent Class에서 CollectObservations 상속받음
    public override void CollectObservations(VectorSensor sensor)
    {
        // 로켓의 로컬 위치 월드 좌표
        // transform은 게임 오브젝트의 포지션, 회전, 스케일 등의 기능
        Vector3 rocketPosition = rc.transform.localPosition;
        // 로켓의 로컬 회전 
        Vector3 rocketRotation = rc.transform.localRotation.eulerAngles;
        Vector3 rocketVelocity = rc.rb.velocity; // 로켓.물리엔진.속도
        // rc.물엔진.각속도
        Vector3 rocketAngularVelocity = rc.rb.angularVelocity;

        // 로켓 로컬 x, y, z 위치
        sensor.AddObservation(rocketPosition.x);
        sensor.AddObservation(rocketPosition.y);
        sensor.AddObservation(rocketPosition.z);
        
        // 로켓 로컬 x, y, z 회전
        sensor.AddObservation(rocketRotation.x);
        sensor.AddObservation(rocketRotation.y);
        sensor.AddObservation(rocketRotation.z);
        // 로켓 로컬 x, y, z 속도
        sensor.AddObservation(rocketVelocity.x);
        sensor.AddObservation(rocketVelocity.y);
        sensor.AddObservation(rocketVelocity.z);

        // 로켓 로컬 x, y, z 각속도
        sensor.AddObservation(rocketAngularVelocity.x); 
        sensor.AddObservation(rocketAngularVelocity.y); 
        sensor.AddObservation(rocketAngularVelocity.z); 
    }

    // Agent Class에서 CollectObservations 상속받음
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 오버라이딩
        //메인엔진(끈다, 킨다)
        rc.SetMainEngine(actionBuffers.DiscreteActions[0]);


        // 왼쪽, 오른쪽 사이드 엔진(끈다, 왼쪽, 오른쪽)
        if (actionBuffers.DiscreteActions[1] == 0)
        {
            rc.SetLeftEngine(0);
            rc.SetRightEngine(0);
        }

        // 왼쪽, 오른쪽 사이드 엔진(왼쪽 킨다, 오른쪽 끈다)
        else if (actionBuffers.DiscreteActions[1] == 1)
        {
            rc.SetLeftEngine(1);
            rc.SetRightEngine(0);
        }

        // 왼쪽, 오른쪽 사이드 엔진(왼쪽 끈다, 오른쪽 킨다)
        else if (actionBuffers.DiscreteActions[1] == 2)
        {
            rc.SetLeftEngine(0);
            rc.SetRightEngine(1);
        }

        // 앞쪽, 뒤쪽 사이드 엔진(끈다, 앞쪽, 뒤쪽)
        if (actionBuffers.DiscreteActions[2] == 0)
        {
            rc.SetForwardEngine(0);
            rc.SetBackwardEngine(0);
        }
        
        // 앞쪽, 뒤쪽 사이드 엔진(앞쪽 킨다, 뒤쪽 끈다)
        else if (actionBuffers.DiscreteActions[2] == 1)
        {
            rc.SetForwardEngine(1);
            rc.SetBackwardEngine(0);
        }

         // 앞쪽, 뒤쪽 사이드 엔진(앞쪽 끈다, 뒤쪽 킨다)
        else if (actionBuffers.DiscreteActions[2] == 2)
        {
            rc.SetForwardEngine(0);
            rc.SetBackwardEngine(1);
        }
    }
    // 끝난 에피소드
    public void EndEpisode(float reward)
    {
        AddReward(reward);
        episodeFinished = true; // 에피소드 활성회
        StartCoroutine(WaitCoroutine());
    }
    
    IEnumerator WaitCoroutine()
    {
        // 학습과정에서 성공 여부를 보려고 1초 딜레이를 줌
        yield return new WaitForSeconds(1f);
        EndEpisode();
    }
}