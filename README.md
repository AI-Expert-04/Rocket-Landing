# Rocket-Landing
### 기술
##### PPO ##### Pytorch ##### 후 보상체계 ##### 실시간 보상체계 ##### 커리큘럼 학습 ##### 강화학습 ##### 내적, 벡터


### 파라미터
##### config5.txt
<pre><code>
    behaviors:   동작
    My Behavior:      내 행동
        max_steps: 100000000   1억번 에피소드
        summary_freq: 10000    만번마다 결과값 출력

environment_parameters:
    init_height:
        curriculum: 과정
            - name: init_height1 -> 코드에 있음
              completion_criteria:
                measure: progress 측정: 진행 상황
                behavior: My Behavior 행동 : 내 행동
                threshold: 0.05  임계값
              value: 10
            - name: init_height2
              completion_criteria:  
                measure: progress 측정: 진행 상황
                behavior: My Behavior 행동 : 내 행동
                threshold: 0.15 임계값
              value: 20
</code></pre>


## Unity 파일구조
```
Rocket-Landing
|---- Assets
|     |---- AtomRocket -> 로켓 에셋(무료)
|     |     |---- Demo
|     |     |---- Mesh
|     |     |---- Prefab
|     |---- Demo
|     |     |---- Materials -> 로켓 착륙판 색깔
|     |     |---- Prefab
|     |     |---- Scripts
|     |     |     |---- AgentControllerFinalR.cs -> 에이전트컨트롤러
|     |     |     |---- RocketControllerFinalR.cs -> 로켓컨트롤러
|     |     |     |---- Camera1.cs -> 로켓 추적1
|     |     |     |---- Camera3.cs -> 로켓 추적2
|     |     |---- Final.unity -> Scene
|     |     |---- My Behavior.onnx -> 모델(마지막 모댈)
|     |---- Final
|     |---- Lab
|     |---- ML-Agents
|     |---- Models
|     |---- Quiz
|     |---- Scenes -> 씬
|     |---- configs -> 파라미터
|     |---- results -> 학습된 모델
|---- Packages
|---- ProjectSettings
```
# report [Link](https://docs.google.com/document/d/1wvJgfdiplu9KBd0RmszDFPmp2Y5kYz2s1mIfXryPQIc/edit?usp=sharing)
