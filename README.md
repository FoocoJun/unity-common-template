## unity-common-template v2
프로젝트 딸 때 마다 만들기 귀찮아서 만든 탬플릿.

인게임 로직을 제외하고 필요한 것만 넣어놨습니다.

좀 더 로직에 집중하여 생산성이 높아지길 바라는 마음.

## 버전
Used Unity Version : 2022.3.51f1

Used Template : Universal 3D v14.0.0

Other Packages
- Addressables : v1.22.3
- Newtonsoft Json : v3.2.1 (Add Package By Name `com.unity.nuget.newtonsoft-json`)

## 디렉토리 구조
### @Resources
    - Data, Prefabs, Font 등을 관리하는 폴더
    - 필요시 Sprites, Sounds 등 하위 폴더 만들어서 추가 관리
    - 기본 Resources 폴더를 사용하지 않고 필요시 Addressable 적용
### @Scenes
    - 씬 관리 폴더
    - 씬 추가 변경 삭제 시 `File > Build Settings` 업데이트 잊지말기
### @Scripts
    - Managers, Scenes, UI 등을 위한 스크립트 관리하는 폴더

## 구현사항
- 씬 전환 예제 (TitleScene : Addressable Load -> GameScene)
- csv 파싱 및 예시 (`Tools > ParseExcel : @Resources/Data/ExcelData -> @Resources/Data/JsonData`)
- 고질병 UI 바인딩을 해결하기 위한 UI_Base 및 UI_EventHandler
- 각종 Manager
    - DataManager : JSON 데이터 호출 및 Dictionary로 저장 관리
    - PoolManager : 오브젝트 풀링
    - ResourcesManager : Addressable을 활용한 리소스 관리
    - SceneManagerEx : enum 관리 및 씬 전환시 로직 추가를 위한 기존 SceneManager 프록시
    - UIManager : 씬 UI, 팝업UI 그 외 UI 관리
    - GameManager : 게임 메인 로직 및 저장 / 로드 관리
    - Managers : 위 모든 Manager 인스턴스 관리

---

## 패치노트
### 25.1.22. v2 업데이트
- create : GameManager w/ Save and Load
- update : UIManager can manage Popup UIs

### 25.1.13. v1
- 씬 전환 예제 (TitleScene : Addressable Load -> GameScene)
- csv 파싱 및 예시 (`Tools > ParseExcel : @Resources/Data/ExcelData -> @Resources/Data/JsonData`)
- 고질병 UI 바인딩을 해결하기 위한 UI_Base 및 UI_EventHandler
- 각종 Manager
  - DataManager : JSON 데이터 호출 및 Dictionary로 저장 관리
  - PoolManager : 오브젝트 풀링
  - ResourcesManager : Addressable을 활용한 리소스 관리
  - SceneManagerEx : enum 관리 및 씬 전환시 로직 추가를 위한 기존 SceneManager 프록시
  - UIManager : 씬 UI, 그 외 UI 관리 (팝업 관리 미구현)
  - Managers : 위 모든 Manager 인스턴스 관리