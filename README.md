<div align="center">

# 🧱 Poly Bash
### A Minimal 3D Physics-based Character Controller Prototype (Unity)

**"단순한 도형(Poly)들 위를, 물리로 부딪히며(Bash) 달린다."**

![Unity](https://img.shields.io/badge/Unity-2022.3.62f3%20LTS-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Scripting-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Render Pipeline](https://img.shields.io/badge/Render%20Pipeline-Built--in-blue?style=for-the-badge)
![Physics](https://img.shields.io/badge/Physics-Rigidbody%20(3D)-orange?style=for-the-badge)
![Platform](https://img.shields.io/badge/Platform-Cross--Platform-lightgrey?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Prototype-yellow?style=for-the-badge)

</div>

---

## 📖 목차

1. [프로젝트 심층 소개 (Overview)](#-프로젝트-심층-소개-overview)
2. [사용 기술 및 라이브러리 (Tech Stack)](#-사용-기술-및-라이브러리-tech-stack--dependencies)
3. [핵심 기능 및 상세 로직 (Key Features & Logic)](#-핵심-기능-및-상세-로직-key-features--logic)
4. [프로젝트 구조 (Directory Structure)](#-프로젝트-구조-및-파일-설명-directory-structure)
5. [Getting Started](#-getting-started-설치-및-실행-가이드)
6. [Troubleshooting & Dev Log](#-troubleshooting--dev-log-트러블슈팅-및-개발-일지)
7. [로드맵 (Roadmap)](#-로드맵-roadmap)

---

## 🔍 프로젝트 심층 소개 (Overview)

### 어떤 목적으로 만들어졌는가?

**Poly Bash**는 Unity의 **3D Rigidbody 물리 엔진**을 기반으로, 가장 기본적인 형태의 **3인칭 캐릭터 이동 + 카메라 추적 시스템**을 검증하기 위한 **미니멀 프로토타입**입니다. 프로젝트명(`PolyBash`, `ProjectSettings.asset`의 `productName`)에서 알 수 있듯, 복잡한 아트 에셋 대신 기본 도형(Poly) 지형 위에서 캐릭터가 이동·점프·충돌(Bash)하는 **물리 기반 코어 루프**를 가장 단순한 형태로 먼저 완성하는 데 초점이 맞춰져 있습니다.

현재 씬(`SampleScene.unity`)에는 최소 구성 요소만 존재합니다:

| GameObject | 역할 |
|---|---|
| `Player` | `Rigidbody` + `PlayerMovement.cs`가 부착된 플레이어 캐릭터 |
| `Main Camera` | `CameraFollow.cs`가 부착되어 Player를 추적하는 3인칭 카메라 |
| `Ground` | 캐릭터가 착지하고 이동할 기본 지면 |

### 전체 작동 흐름 (Architecture Flow) — 시나리오

```
[게임 시작]
    │
    ▼
Start() 1회 실행 ── PlayerMovement: Rigidbody 캐싱, 타겟 FPS 120으로 고정
    │
    ▼
매 프레임(Update) ─┬─ WASD/방향키 입력 → Horizontal/Vertical Axis 읽기
    │              └─ Space 입력 감지 → 점프 여부 플래그
    │
    ▼
고정 프레임(FixedUpdate) ── 입력값 기반으로 Rigidbody.MovePosition() 호출
    │                        (물리 스텝과 동기화된 이동으로 벽 뚫림 방지)
    │
    ▼
모든 오브젝트 이동 완료 후(LateUpdate) ── CameraFollow: 카메라 위치를
                                          "Player 위치 + offset"으로 갱신
    │
    ▼
[화면에 카메라가 플레이어를 따라다니며 렌더링]
```

**시나리오로 풀어보면:**
1. 플레이어가 `W/A/S/D`(또는 방향키)를 누르면 `PlayerMovement.Update()`가 매 프레임 입력을 읽어 **이동 방향 벡터**를 계산합니다.
2. `Space`를 누르면 `Rigidbody.AddForce`로 **즉각적인 위쪽 힘(Impulse)**을 가해 점프가 발생합니다.
3. 실제 위치 이동은 `Update()`가 아닌 **`FixedUpdate()`** 안에서, `Rigidbody.MovePosition()`을 통해 물리 엔진의 고정 타임스텝에 맞춰 수행됩니다 — 이는 프레임레이트에 따라 이동 속도가 들쭉날쭉해지거나 얇은 벽을 뚫고 지나가는 문제를 방지하기 위한 설계입니다.
4. 플레이어가 움직이고 나면, **모든 `Update`/물리 연산이 끝난 뒤 호출되는 `LateUpdate()`** 시점에 `CameraFollow`가 카메라 위치를 `target.position + offset`으로 재계산합니다. 이 순서 덕분에 카메라가 플레이어보다 한 프레임 먼저/늦게 움직이며 떨리는 현상(Jitter)이 방지됩니다.

---

## 🧰 사용 기술 및 라이브러리 (Tech Stack & Dependencies)

`Packages/manifest.json` 및 `ProjectSettings/ProjectVersion.txt`를 기준으로 분석한 스택입니다.

| 구분 | 내용 | 배지 |
|---|---|---|
| 엔진 | Unity **2022.3.62f3 (LTS)** | ![Unity](https://img.shields.io/badge/-Unity%202022.3.62f3%20LTS-000000?style=flat-square&logo=unity&logoColor=white) |
| 언어 | C# (MonoBehaviour 기반) | ![C#](https://img.shields.io/badge/-C%23-239120?style=flat-square&logo=csharp&logoColor=white) |
| 렌더 파이프라인 | Built-in Render Pipeline (Standard Shader 사용, URP/HDRP 패키지 미포함) | ![RP](https://img.shields.io/badge/-Built--in%20RP-blue?style=flat-square) |
| 물리 | `UnityEngine.Rigidbody` (3D 물리, `com.unity.modules.physics`) | ![Physics](https://img.shields.io/badge/-Rigidbody%203D-orange?style=flat-square) |
| UI | `com.unity.ugui` 1.0.0 (uGUI) | ![uGUI](https://img.shields.io/badge/-uGUI-purple?style=flat-square) |
| 텍스트 | `com.unity.textmeshpro` 3.0.7 | ![TMP](https://img.shields.io/badge/-TextMeshPro-informational?style=flat-square) |
| 타임라인 | `com.unity.timeline` 1.7.7 | ![Timeline](https://img.shields.io/badge/-Timeline-lightgrey?style=flat-square) |
| 비주얼 스크립팅 | `com.unity.visualscripting` 1.9.4 | ![VS](https://img.shields.io/badge/-Visual%20Scripting-lightgrey?style=flat-square) |
| 협업 | `com.unity.collab-proxy` 2.11.3 (Unity Version Control 연동) | ![Collab](https://img.shields.io/badge/-Collab%20Proxy-lightgrey?style=flat-square) |
| 입력 | 레거시 `Input Manager` (`Horizontal`, `Vertical`, `Jump` 축 사용) | ![Input](https://img.shields.io/badge/-Legacy%20Input%20Manager-lightgrey?style=flat-square) |
| IDE 연동 | `.vsconfig`에 `Microsoft.VisualStudio.Workload.ManagedGame` 워크로드 명시 | ![VisualStudio](https://img.shields.io/badge/-Visual%20Studio-5C2D91?style=flat-square&logo=visualstudio&logoColor=white) |

> 💡 `com.unity.modules.*` 계열은 Unity 엔진에 기본 내장된 코어 모듈(오디오, 애니메이션, 물리, XR 등)로, 프로젝트 생성 시 자동 포함되는 표준 구성입니다. 이 프로젝트는 **New 3D (Built-in) 템플릿**을 기반으로 시작된 것으로 보입니다.

---

## ⚙️ 핵심 기능 및 상세 로직 (Key Features & Logic)

### 1. `PlayerMovement.cs` — 물리 기반 이동 & 점프

```csharp
public float moveSpeed = 8f;
public float jumpForce = 5f;
```
- **입력 분리 구조**: `Update()`에서는 **입력 감지만** 담당하고(`Input.GetAxisRaw`, `Input.GetButtonDown`), 실제 물리 이동은 `FixedUpdate()`에서 처리하는 **Unity 권장 패턴**을 따릅니다. `GetAxisRaw`를 사용해 Unity의 기본 입력 스무딩(가속/감속) 없이 **즉각적인 반응성**을 확보했습니다.
- **수평 이동만 계산**: `moveDirection = new Vector3(x, 0, z).normalized`로 Y축(수직) 값은 배제한 채 XZ 평면상의 이동 벡터만 정규화하여, 대각선 이동 시 속도가 더 빨라지는 문제(피타고라스 보정)를 방지했습니다.
- **점프는 즉시 힘(Impulse) 적용**: `rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse)`로 질량과 무관하게 순간적인 속도 변화를 주어, 버튼을 누르는 즉시 반응하는 점프감을 구현했습니다.
- **이동은 `MovePosition` 사용**: `rb.velocity`를 직접 조작하는 대신 `rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime)`을 사용하여, **물리 엔진의 충돌 처리(Continuous Collision)와 자연스럽게 통합**되면서도 이동 속도를 코드로 직접 제어할 수 있도록 설계했습니다.
- **디바이스 최적화**: `Application.targetFrameRate = 120`을 `Start()`에서 강제 설정하여, 고주사율(120Hz) 디스플레이를 지원하는 모바일 기기(주석상 "아이폰 프로 모델용")에서도 프레임 제한 없이 부드러운 체감을 제공하도록 했습니다.

### 2. `CameraFollow.cs` — 지터(떨림) 없는 3인칭 추적 카메라

```csharp
public Transform target;
public Vector3 offset;
```
- **`LateUpdate()` 채택 이유**: Unity의 실행 순서상 모든 `Update()`/`FixedUpdate()`가 끝난 뒤 `LateUpdate()`가 호출됩니다. 카메라 추적 로직을 여기 배치함으로써, **플레이어가 이동을 완전히 마친 좌표를 기준으로 카메라가 뒤따라가게 되어** 카메라가 미세하게 떨리거나 한 프레임 밀려 보이는 현상을 근본적으로 방지합니다.
- **Null 가드**: `if (target != null)` 체크를 통해 씬 전환 등으로 타겟이 아직 할당되지 않은 초기 프레임에서 `NullReferenceException`이 발생하지 않도록 방어적으로 코딩했습니다.
- **오프셋 기반 상대 위치 고정**: 절대 좌표가 아닌 `target.position + offset`(인스펙터에서 조절 가능한 `Vector3`)을 사용해, 디자이너가 코드 수정 없이 에디터에서 카메라 거리/높이/각도를 자유롭게 튜닝할 수 있게 했습니다.

### 3. 씬 구성 (`SampleScene.unity`)
- `Player`: `Rigidbody` 컴포넌트 + `PlayerMovement` 스크립트가 부착되어 물리 시뮬레이션의 대상이 됩니다.
- `Main Camera`: `CameraFollow` 스크립트가 부착되어 `target`에 `Player`의 `Transform`이 연결되어 있습니다.
- `Ground`: 충돌체(Collider)를 가진 정적 지면 오브젝트로, `Player`의 착지 기준면 역할을 합니다.
- `PlayerMat.mat`: Unity 기본 **Standard Shader**를 사용하는 머티리얼로, 별도 텍스처 없이 색상만으로 캐릭터를 표현하는 **로우 폴리(Low-Poly) 스타일**에 부합합니다.

---

## 📂 프로젝트 구조 및 파일 설명 (Directory Structure)

```bash
Poly-Bash-main/
├── .gitignore                          # Unity 표준 gitignore (Library/Temp/Obj/Build 등 산출물 제외)
├── .vsconfig                           # Visual Studio 설치 시 필요한 워크로드 명시 (ManagedGame)
│
├── Assets/                             # 게임의 실제 콘텐츠(코드/씬/머티리얼)가 위치하는 핵심 폴더
│   ├── PlayerMovement.cs               # ⭐ 플레이어 이동 + 점프 로직 (Rigidbody 기반)
│   ├── PlayerMovement.cs.meta          # Unity 에디터가 관리하는 GUID/임포트 설정 메타파일
│   ├── CameraFollow.cs                 # ⭐ 3인칭 카메라 추적 로직 (LateUpdate 기반)
│   ├── CameraFollow.cs.meta            # 메타파일
│   ├── PlayerMat.mat                   # 플레이어 캐릭터에 적용되는 Standard Shader 머티리얼
│   ├── PlayerMat.mat.meta              # 메타파일
│   ├── Scenes.meta                     # Scenes 폴더 메타파일
│   └── Scenes/
│       ├── SampleScene.unity           # 메인(유일) 게임 씬 — Player/Main Camera/Ground 배치
│       └── SampleScene.unity.meta      # 메타파일
│
├── Packages/
│   ├── manifest.json                   # 프로젝트가 사용하는 UPM 패키지 의존성 목록
│   └── packages-lock.json              # 패키지 버전 고정(lock) 파일
│
└── ProjectSettings/                    # Unity 프로젝트 전역 설정 (씬 목록, 입력, 물리, 품질 등)
    ├── ProjectSettings.asset           # 제품명(PolyBash), 회사명, 버전(0.1) 등 핵심 설정
    ├── ProjectVersion.txt              # 사용된 Unity 에디터 버전 고정 (2022.3.62f3)
    ├── InputManager.asset              # 레거시 입력 축 정의 (Horizontal/Vertical/Jump 등)
    ├── EditorBuildSettings.asset       # 빌드에 포함될 씬 목록
    ├── GraphicsSettings.asset          # 렌더 파이프라인/셰이더 관련 전역 설정
    ├── QualitySettings.asset           # 품질 등급별 렌더링 옵션
    ├── TagManager.asset                # 태그/레이어 정의
    ├── DynamicsManager.asset           # 3D 물리 엔진 전역 설정(중력 등)
    ├── Physics2DSettings.asset         # 2D 물리 엔진 설정 (현재 미사용)
    ├── AudioManager.asset              # 오디오 전역 설정
    ├── TimeManager.asset               # 픽스드 타임스텝 등 시간 관련 설정
    ├── PackageManagerSettings.asset    # 패키지 매니저 UI 관련 설정
    ├── PresetManager.asset             # 에셋 임포트 프리셋 설정
    ├── SceneTemplateSettings.json      # 씬 템플릿 설정
    ├── VersionControlSettings.asset    # 버전 관리 도구(Collab 등) 연동 설정
    ├── VFXManager.asset                # 비주얼 이펙트 그래프 전역 설정
    ├── XRSettings.asset                # XR(VR/AR) 관련 설정 (현재 미사용)
    ├── NavMeshAreas.asset              # 내비게이션 메시 영역 정의 (현재 미사용)
    ├── ClusterInputManager.asset       # 클러스터 렌더링용 입력 설정 (현재 미사용)
    ├── EditorSettings.asset            # 에디터 자체 동작 설정(직렬화 모드 등)
    ├── UnityConnectSettings.asset      # Unity 서비스(애널리틱스 등) 연동 설정
    └── Packages/
        └── com.unity.testtools.codecoverage/
            └── Settings.json           # 코드 커버리지 테스트 도구 설정
```

> ⚠️ Unity 프로젝트의 특성상 `Library/`, `Temp/`, `Obj/`, `Build/`, `Logs/`, `UserSettings/` 폴더는 `.gitignore`에 의해 저장소에 포함되지 않으며, **Unity 에디터로 처음 열 때 자동 재생성**됩니다.

---

## 🚀 Getting Started (설치 및 실행 가이드)

### 1️⃣ 사전 요구사항

- **Unity Hub** 설치
- **Unity Editor `2022.3.62f3`** (LTS) — `ProjectSettings/ProjectVersion.txt`에 명시된 버전과 **동일하거나 상위 2022.3.x LTS**를 권장합니다. 버전이 다르면 Unity Hub가 자동으로 프로젝트 업그레이드 여부를 묻습니다.
- Visual Studio 2022 (또는 VS Code / Rider) + **".NET 데스크톱 개발" / "Unity를 사용한 게임 개발"** 워크로드 (`.vsconfig`에 `Microsoft.VisualStudio.Workload.ManagedGame` 명시됨)

### 2️⃣ 저장소 클론

```bash
git clone <이 저장소의 URL>
cd Poly-Bash-main
```

### 3️⃣ Unity Hub에서 프로젝트 열기

1. Unity Hub 실행 → **[프로젝트]** 탭 → **[추가(Add)]** → 클론한 `Poly-Bash-main` 폴더 선택
2. 에디터 버전이 설치되어 있지 않다면 Unity Hub가 `2022.3.62f3` 설치를 안내합니다.
3. 프로젝트를 열면 `Packages/manifest.json`에 명시된 의존 패키지들이 **Package Manager에 의해 자동으로 다운로드/복원**됩니다. (별도의 `npm install`, `pip install` 같은 수동 설치 과정이 필요 없습니다.)

### 4️⃣ 씬 열기 & 실행

1. `Project` 창에서 `Assets/Scenes/SampleScene.unity`를 더블클릭하여 엽니다.
2. 상단 툴바의 **▶ Play** 버튼을 눌러 에디터 내에서 바로 테스트합니다.
3. **조작법**: `W`/`A`/`S`/`D`(또는 방향키) 이동, `Space` 점프

### 5️⃣ 빌드하기

1. 메뉴 `File > Build Settings...` 이동
2. `EditorBuildSettings.asset`에 등록된 `SampleScene`이 빌드 목록에 있는지 확인 (없다면 `Add Open Scenes` 클릭)
3. 원하는 플랫폼(PC/Mac/Linux, Android, iOS 등) 선택 후 `Build` 또는 `Build And Run` 클릭

---

## 🛠️ Troubleshooting & Dev Log (트러블슈팅 및 개발 일지)

### 🧩 이슈 1. "Update()에서 바로 이동시켰더니 벽을 뚫고 지나가요"
- **원인**: `Update()`는 프레임레이트에 종속적으로 호출되는 반면, Unity의 물리 시뮬레이션(충돌 판정)은 **고정된 타임스텝(`FixedUpdate`)** 으로 동작합니다. `Update()`에서 `Transform`을 직접 움직이면 물리 충돌 판정과 타이밍이 어긋나 **터널링(벽 관통)** 현상이 발생할 수 있습니다.
- **해결**: 입력 감지는 `Update()`에 두되, 실제 위치 갱신(`rb.MovePosition`)은 반드시 `FixedUpdate()`에서 수행하도록 분리했습니다. 코드 주석에도 `// 4. 물리 프레임으로 이동 (벽 뚫기 방지)`로 이 의도가 명시되어 있습니다.

### 🧩 이슈 2. "카메라가 플레이어를 따라갈 때 미세하게 떨려요(Jitter)"
- **원인**: 카메라 추적 로직을 `Update()`에 두면, 플레이어의 물리 갱신(`FixedUpdate`)과 카메라의 갱신 타이밍이 어긋나 **한 프레임의 위치 차이**가 시각적으로 떨림처럼 보이는 문제가 생깁니다.
- **해결**: `CameraFollow`의 추적 로직을 **모든 이동/물리 계산이 끝난 뒤 실행되는 `LateUpdate()`** 로 이동시켰습니다. 주석 `// 카메라 덜덜거림 방지`에서 이 문제를 명시적으로 인지하고 해결했음을 확인할 수 있습니다.

### 🧩 이슈 3. "대각선(예: W+D)으로 이동할 때 캐릭터가 더 빨리 움직여요"
- **원인**: `x`와 `z` 입력값을 그대로 더해서 이동시키면, 대각선 방향 벡터의 크기(magnitude)가 `√2`배가 되어 **정면/측면 이동보다 대각선 이동이 더 빠른** 고전적인 버그가 발생합니다.
- **해결**: `moveDirection = new Vector3(x, 0, z).normalized`로 **방향 벡터를 항상 단위 벡터(길이 1)로 정규화**한 뒤 `moveSpeed`를 곱하는 방식을 채택하여, 이동 방향과 무관하게 항상 일정한 속도를 보장했습니다.

### 🧩 이슈 4. "특정 모바일 기기에서 프레임이 60으로 제한돼요"
- **원인**: 일부 고주사율 디스플레이(120Hz) 기기는 Unity의 기본 타겟 프레임레이트 설정에 따라 60fps로 제한될 수 있습니다.
- **해결**: `Start()` 시점에 `Application.targetFrameRate = 120`을 명시적으로 호출하여, 지원 기기에서 **최대 120fps까지 프레임 제한을 해제**하고 더 부드러운 조작감을 제공하도록 처리했습니다. (주석: `// 아이폰 프로 모델용 120프레임 제한 해제`)

### 🧩 이슈 5. "카메라 오프셋을 코드에 하드코딩했더니 튜닝이 번거로워요"
- **원인**: 카메라와 캐릭터 사이의 거리/높이는 게임 플레이 테스트를 거치며 수십 번씩 조정되는 값인데, 매번 코드를 수정하고 재컴파일하는 것은 비효율적입니다.
- **해결**: `offset`을 `public Vector3`로 선언하여 **Unity 인스펙터(Inspector) 창에서 직접 드래그/입력으로 실시간 조정**할 수 있게 노출했습니다. 코드 재컴파일 없이 플레이 모드에서도 즉시 값을 바꿔볼 수 있습니다.

### 🧩 이슈 6. "레거시 Input Manager, 새 Input System 중 어떤 걸 쓸까?"
- **현황**: `Packages/manifest.json`에 `com.unity.inputsystem` 패키지가 포함되어 있지 않고, `ProjectSettings/InputManager.asset`에 `Horizontal`/`Vertical`/`Jump` 축이 정의되어 있는 것으로 보아 **레거시 Input Manager(`Input.GetAxisRaw`)** 방식을 그대로 사용 중입니다.
- **판단**: 프로토타입 단계에서는 별도 패키지 설치나 액션 맵 구성 없이 **즉시 사용 가능한 레거시 방식**이 빠른 검증에 더 적합하다고 판단하여 유지된 것으로 보입니다. (추후 멀티 플랫폼 입력 확장이 필요하다면 새 Input System으로의 마이그레이션을 고려할 수 있습니다.)

---

## 🗺️ 로드맵 (Roadmap)

현재 프로젝트는 이동/카메라만 검증된 **최소 기능 프로토타입(MVP)** 단계입니다. 코드 구조상 다음과 같은 확장이 자연스럽게 이어질 수 있습니다.

- [ ] 지면 접지 판정(`IsGrounded`) 추가로 **공중 다중 점프 방지**
- [ ] 애니메이터(Animator) 연동으로 이동/점프 모션 재생
- [ ] 카메라 회전(마우스 룩) 및 충돌 회피(카메라-벽 클리핑 방지) 로직 추가
- [ ] 레거시 Input Manager → **Unity Input System**으로 마이그레이션 검토
- [ ] `Ground` 외 추가 지형/장애물 오브젝트로 레벨 디자인 확장
