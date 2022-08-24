# Rocket-Landing

### 파라미터
##### config5.txt
<pre><code>
    behaviors:
    My Behavior:
        max_steps: 100000000
        summary_freq: 10000

environment_parameters:
    init_height:
        curriculum:
            - name: init_height1
              completion_criteria:
                measure: progress
                behavior: My Behavior
                threshold: 0.05
              value: 10
            - name: init_height2
              completion_criteria:
                measure: progress
                behavior: My Behavior
                threshold: 0.15
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
