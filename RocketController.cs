using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public Rigidbody rb; // 물리엔진
    public AgentController ac; // 에이전트 컨트롤 연동
    public GameObject landingZoneNormal; // 기본 목표물 게임오브젝 받기
    public GameObject landingZoneSuccess; // 성공 목표물 게임오브젝 받기
    public GameObject landingZoneFail; // 실패 목표물 게임오브젝 받기
    public float mainEngineForce = 20f; // 메인엔진 힘 받기
    public float sideEngineForce = 0.5f; // 서브엔진 힘 받기

    // bool = False, True만 가능
    public bool mainEngineOn = false; // 메인엔진 비활성화
    public bool leftEngineOn = false; // 왼쪽엔진 비활성화
    public bool rightEngineOn = false; // 오른쪽엔진 비활성화
    public bool forwardEngineOn = false; // 앞쪽엔진 비활성화
    public bool backwardEngineOn = false; // 뒤쪽엔진 비활성화

    public GameObject mainEngineFx; // 메인엔진 게임오브젝트 받기
    public GameObject leftEngineFx; // 왼쪽엔진 게임오브젝트 받기
    public GameObject rightEngineFx; // 오른쪽엔진 게임오브젝트 받기
    public GameObject forwardEngineFx; // 앞쪽엔진 게임오브젝트 받기
    public GameObject backwardEngineFx; // 뒤쪽엔진 게임오브젝트 받기

    // 파티클 시스템 수정 (색상, 효과 수정)
    ParticleSystem.MainModule mainEngineParticleSystem; // 메인엔진 파티클시스템
    ParticleSystem.MainModule leftEngineParticleSystem; // 왼쪽엔진 파티클시스템
    ParticleSystem.MainModule rightEngineParticleSystem; // 오른쪽엔진 파티클시스템
    ParticleSystem.MainModule forwardEngineParticleSystem; // 앞쪽엔진 파티클시스템
    ParticleSystem.MainModule backwardEngineParticleSystem; // 뒤쪽엔진 파티클시스템

    ParticleSystemRenderer mainEngineParticleSystemRenderer; // 메인엔진 파티클시스템 렌더러

    public GameObject LandingLeg1; // 로켓 다리
    public GameObject LandingLeg2; // 로켓 다리
    public GameObject LandingLeg3; // 로켓 다리
    public GameObject LandingLeg4; // 로켓 다리

    public bool reset = false; // 리셋 비활성화
    public bool stop = false; // 멈추기 비활성화

    public float initHeight = 20; // 초기화 높이
    public float rotationRange = 0;  // 회전 범위
    public float xzRange = 0; // xz 범위

    // 보상체게
    public Vector3 previousVelocity = Vector3.positiveInfinity; // 3차원, (무한, 무한, 무한)
    // 거리
    public float previousDistance = float.PositiveInfinity; // 무한소
    // 지면과 수직이되는 벡터와의 각도
    public float previousAngle = float.PositiveInfinity;

    // fx = 비활성화
    public bool fx = false;

    // 한번만 시작함
    void Start()
    {
        ac = GetComponent<AgentController>(); // 에이전트 구성요소 받기
        rb = GetComponent<Rigidbody>(); // 물리엔진 구성요소 받기

        // 메인엔진의 파티클 시스템 옵션값 코드로 제어
        mainEngineParticleSystem = mainEngineFx.GetComponent<ParticleSystem>().main;
        leftEngineParticleSystem = leftEngineFx.GetComponent<ParticleSystem>().main;
        rightEngineParticleSystem = rightEngineFx.GetComponent<ParticleSystem>().main;
        forwardEngineParticleSystem = forwardEngineFx.GetComponent<ParticleSystem>().main;
        backwardEngineParticleSystem = backwardEngineFx.GetComponent<ParticleSystem>().main;

        // 메인엔진 파티클 시스템 옵션값 코드로 제어
        mainEngineParticleSystemRenderer = mainEngineFx.GetComponent<ParticleSystemRenderer>();

        if (!fx)
        {    
            mainEngineFx.SetActive(false); // 메인엔지진 활성 설정 = 비활성화
            leftEngineFx.SetActive(false); // 왼쪽엔지진 활성 설정 = 비활성화
            rightEngineFx.SetActive(false); // 오른쪽엔지진 활성 설정 = 비활성화
            forwardEngineFx.SetActive(false); // 앞엔지진 활성 설정 = 비활성화
            backwardEngineFx.SetActive(false); // 뒤엔지진 활성 설정 = 비활성화
        }
    }

    // 리셋로켓 -> 나중에 에이전트 컨트롤러에서 씀
    public void ResetRocket()
    {
        reset = true;  
    }

    // 메인엔진 설정 -> 나중에 에이전트 컨트롤러에서 씀
    public void SetMainEngine(int value)
    {
        if (value == 1)
        {
            mainEngineOn = true; // 메인엔진 활성화
        }

        else
        {
            mainEngineOn = false; // 메인엔진 비활성화
        }
    }

    // 왼쪽엔진 설정 -> 나중에 에이전트 컨트롤러에서 씀
    public void SetLeftEngine(int value)
    {
        if (value == 1)
        {
            leftEngineOn = true; // 왼쪽엔진 활성화
        }

        else
        {
            leftEngineOn = false; // 메인엔진 비활성화
        }
    }

    // 오른쪽엔진 설정 -> 나중에 에이전트 컨트롤러에서 씀
    public void SetRightEngine(int value)
    {
        if (value == 1)
        {
            rightEngineOn = true; // 오른쪽엔진 활성화
        }

        else
        {
            rightEngineOn = false;  // 오른쪽엔진 비활성화
        }
    }

    // 앞쪽엔진 설정 -> 나중에 에이전트 컨트롤러에서 씀
    public void SetForwardEngine(int value)
    {
        if (value == 1)
        {
            forwardEngineOn = true; // 앞쪽엔진 활성화
        }

        else
        {
            forwardEngineOn = false; // 앞쪽엔진 비활성화
        }
    }

    // 뒤쪽엔진 설정 -> 나중에 에이전트 컨트롤러에서 씀
    public void SetBackwardEngine(int value)
    {
        if (value == 1) 
        {
            backwardEngineOn = true; // 뒤쪽엔진 활성화
        }

        else
        {
            backwardEngineOn = false; // 뒤쪽엔진 비활성화
        }
    }

    // fixed는 모든컴퓨터에서 사양 관계없이 똑같이 흐름.
    private void FixedUpdate()
    {
        if (fx)
        {
            // Quaternion 4개의 수(x, y, z, w) 축, 각도가 아님
            // 하나의 벡터(x, y, x)와 하나의 스칼라(w, roll)의미
            // Euler (x, y, z) 회전
            // 로켓 다리 펼치기 로컬 회전

            // y의 높이가 20 이하가 된다면 모든다리가 평등하게 펴지는 과정
            LandingLeg1.transform.localRotation = 
            Quaternion.Euler(new Vector3(0, 120f * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0));
            LandingLeg2.transform.localRotation = 
            Quaternion.Euler(new Vector3(120f * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0, -90));
            LandingLeg3.transform.localRotation = 
            Quaternion.Euler(new Vector3(0, -120 * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, -180));
            LandingLeg4.transform.localRotation = 
            Quaternion.Euler(new Vector3(-120 * (10f - Mathf.Clamp(transform.position.y - 4f, 0f, 10f)) / 10f, 0, 90));
        }

        // 만약 한 에피소드가 끝나면
        if (ac.episodeFinished)
        {
            // 엔진 파티클시스템의 내장 기능인 시작 색깔을 = 투명색깔로 번경
            mainEngineParticleSystem.startColor = Color.clear;
            leftEngineParticleSystem.startColor = Color.clear;
            rightEngineParticleSystem.startColor = Color.clear;
            forwardEngineParticleSystem.startColor = Color.clear;
            backwardEngineParticleSystem.startColor = Color.clear;
            return;
        }

        // 만약 리셋이 활성화 된다면
        if (reset)
        {
            // 랜덤.범위 -3.14, ~ 3.14
            float positionTheta = Random.Range(-Mathf.PI, Mathf.PI);
            float rotationTheta = Random.Range(-Mathf.PI, Mathf.PI);

            transform.localPosition = new Vector3(Mathf.Sin(positionTheta)
             * xzRange, initHeight, Mathf.Cos(positionTheta) * xzRange); //y높이 150

            transform.localRotation = Quaternion.Euler(Mathf.Sin(rotationTheta)
             * rotationRange, (rotationRange == 0 ? 0 : 1) * Random.Range(0f, 360f),
              Mathf.Cos(rotationTheta) * rotationRange); // 랜덤회전(-45도~45도) 
              //xz랜덤좌표(0,0 을 중심으로하는 반지름 50인 원의 둘레) 에서 낙하

            rb.velocity = new Vector3(0, 0, 0); // 속도
            // Vector3.zero = (0, 0, 0)
            rb.angularVelocity = Vector3.zero;// 각속도.

            reset = false; // 리셋 비활성화
            stop = false; // stop 비활성화

            previousVelocity = Vector3.positiveInfinity; // 3차원 (무한, 무한, 무한)

            previousDistance = float.PositiveInfinity; // 거리값 초기화
            
            previousAngle = float.PositiveInfinity; // 지면과 수직이되는 벡터와의 각도

            landingZoneNormal.SetActive(true);  // 기본 목표물 활성화
            landingZoneSuccess.SetActive(false); // 성공 목표물 비활성화
            landingZoneFail.SetActive(false); // 실패 목표물 비활성화
            return; 
        }

        // 로켓의 고도가 180을 넘어가면 0점 처리
        // 로켓의 고도가 음수가 되면 0점 처리
        // 로켓의 각도가 50도를 넘어가면 0점 처리
        if (transform.localPosition.y > initHeight * 1.2f || transform.localPosition.y < 0 || 
        Vector3.Angle(Vector3.up, transform.up) > 50)
        {
            landingZoneNormal.SetActive(false); // 기본 목표물 비활성
            landingZoneSuccess.SetActive(false); // 성공 목표물 비활성화
            landingZoneFail.SetActive(true); // 실패 목표물 활성화
            ac.EndEpisode(0); // 점수 0점
            return;
        }

        // ##(목표 달성 보상)##
        if (rb.IsSleeping())
        {
           // 착륙 후 로켓이 넘어지지 않는다면 
            if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.right)) < 0.1 && Mathf.Abs(Vector3.Dot(transform.up, Vector3.forward)) 
            < 0.1 && Vector3.Dot(transform.up, Vector3.up) > 0.9)
            {
                float distance = Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z));
                // (0,0,0)을 중심으로 거리 3 미만인 범위에 y속도 10 이하로 바로 서게 착륙시 +1000
                if (distance < 3f)
                {
                    landingZoneNormal.SetActive(false); // 기본 목표물 비활성화
                    landingZoneSuccess.SetActive(true); // 성공 목표물 활성화
                    landingZoneFail.SetActive(false); // 실패 목표물 비활성화
                    ac.EndEpisode(1000);
                }

                // 착륙 지점의 xz거리가 3을 넘어가면 0점 처리
                else
                {
                    landingZoneNormal.SetActive(false); // 기본 목표물 비활성화
                    landingZoneSuccess.SetActive(false); // 성공 목표물 비활성화
                    landingZoneFail.SetActive(true); // 실패 목표물 활성화
                    ac.EndEpisode(0); // 점수 0점
                }
            }
            
            // 착륙 후 로켓이 넘어지면 0점 처리
            else
            {
                landingZoneNormal.SetActive(false);// 기본 목표물 비활성화
                landingZoneSuccess.SetActive(false); // 성공 목표물 비활성화
                landingZoneFail.SetActive(true); //실패 목표물 활성화
                ac.EndEpisode(0); // 점수 0점
            }
        }

        if (stop)
        {
            // 엔진 파티클시스템의 내장 기능인 시작 색깔을 = 투명색깔로 번경
            mainEngineParticleSystem.startColor = Color.clear;
            leftEngineParticleSystem.startColor = Color.clear;
            rightEngineParticleSystem.startColor = Color.clear;
            forwardEngineParticleSystem.startColor = Color.clear;
            backwardEngineParticleSystem.startColor = Color.clear;
            return;
        }

        //##(목표에 도달하도록 유도하는 보상)##
        // y속도를 줄이면 +0.05, y속도를 늘리면 -0.05
        if (previousVelocity != Vector3.positiveInfinity && Mathf.Abs(rb.velocity.y) < Mathf.Abs(previousVelocity.y))
        {

            ac.AddReward(0.05f); 
        }

        else
        {
            ac.AddReward(-0.05f); 
        }
        
        // xz거리를 줄이면 +0.05, xz거리를 늘리면 -0.05
        if (previousDistance != float.PositiveInfinity && 
        Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z)) < previousDistance)
        {
            // 추가 보상
            ac.AddReward(0.05f);
        }

        else
        {
            // 음수 보상
            ac.AddReward(-0.05f);
        }
         
        // 로켓을 지면과 수직에 가까워지게 하면 +0.05, 수직과 멀어지게 하면 -0.05
        if (previousAngle != float.PositiveInfinity && Vector3.Angle(Vector3.up, transform.up) < previousAngle)
        {
            // 추가 보상
            ac.AddReward(0.05f);
        }

        else
        {
            // 음수 보상
            ac.AddReward(-0.05f);
        }

        previousVelocity = rb.velocity;

        previousDistance = Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z));
        previousAngle = Vector3.Angle(Vector3.up, transform.up);

        // 엔진 조작 파트
        if (mainEngineOn)
        {
            // up * 메인엔진
            rb.AddForceAtPosition(transform.up * mainEngineForce, transform.position);// 
            mainEngineParticleSystem.startColor = new Color(255, 119, 0); // 메인엔진 파티션 색깔(엔진 불꽃 색깔) 주황색
        }

        else
        {
            mainEngineParticleSystem.startColor = Color.clear; // 메인엔진 파티션 색깔(엔진 불꽃 색깔) 투명색
        }

        if (leftEngineOn)
        {
            rb.AddForceAtPosition(transform.right * sideEngineForce, leftEngineFx.transform.position);
            leftEngineParticleSystem.startColor = Color.white; // 왼쪽엔진 파티션 색깔(엔진 불꽃 색깔) 흰색
        }

        else
        {
            leftEngineParticleSystem.startColor = Color.clear; // 파티션 색깔(엔진 불꽃 색깔) 투명색
        }

        if (rightEngineOn)
        {
            rb.AddForceAtPosition(-transform.right * sideEngineForce, rightEngineFx.transform.position);
            rightEngineParticleSystem.startColor = Color.white; // 오른쪽엔진 파티션 색깔(엔진 불꽃 색깔) 흰색
        }

        else
        {
            rightEngineParticleSystem.startColor = Color.clear; // 파티션 색깔(엔진 불꽃 색깔) 투명색
        }

        if (forwardEngineOn)
        {
            rb.AddForceAtPosition(-transform.forward * sideEngineForce, forwardEngineFx.transform.position);
            forwardEngineParticleSystem.startColor = Color.white; // 앞엔진 파티션 색깔(엔진 불꽃 색깔) 흰색
        }

        else
        {
            forwardEngineParticleSystem.startColor = Color.clear; // 파티션 색깔(엔진 불꽃 색깔) 투명색
        }

        if (backwardEngineOn)
        {
            rb.AddForceAtPosition(transform.forward * sideEngineForce, backwardEngineFx.transform.position);
            backwardEngineParticleSystem.startColor = Color.white; // 뒤엔진 파티션 색깔(엔진 불꽃 색깔) 흰색
        }

        else
        {
            backwardEngineParticleSystem.startColor = Color.clear; // 파티션 색깔(엔진 불꽃 색깔) 투명색
        }

        mainEngineParticleSystemRenderer.lengthScale = Mathf.Clamp(transform.position.y, 4, 8);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 에피소드가 끝나거나 멈추면 리턴
        if (ac.episodeFinished || stop)
        {
            return;
        }


        // 착륙 y속도가 10을 넘어가면 0점 처리
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