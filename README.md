# Unity Common Template v3

Unity 게임 프로젝트를 빠르게 시작하기 위한 프레임워크 템플릿.

매번 프로젝트 생성할 때마다 반복되는 작업을 줄이고, 게임 로직에 집중할 수 있도록 핵심 시스템들을 미리 구현해두었습니다.

## 버전 정보

| 항목 | 버전 |
|------|------|
| Unity | 6000.2.7f2 (Unity 6) |
| URP | 17.2.0 |
| Addressables | 2.7.6 |
| Newtonsoft Json | 3.2.2 |
| Unity IAP | 5.1.1 |
| LevelPlay (Ads) | 9.2.0 |
| Input System | 1.14.2 |

## 디렉토리 구조

```
Assets/
├── @Dev/               # 개발용 씬 스크립트 및 테스트 리소스
├── @ExcelData/         # 원본 Excel/CSV 데이터 파일
├── @Scenes/            # 씬 파일 (DevScene, LoadingScene)
├── @Scripts/           # 메인 스크립트
│   ├── Config/         # ScriptableObject 설정 클래스
│   ├── Controllers/    # 게임 오브젝트 컨트롤러
│   │   └── Creature/   # Player, Monster, Npc
│   ├── Data/           # 데이터 클래스 (ItemData, TextData)
│   ├── Editor/         # 에디터 스크립트
│   ├── Managers/       # 싱글톤 매니저들
│   ├── Scenes/         # 씬 컨트롤러
│   ├── UI/             # UI 컴포넌트
│   │   ├── Popup/      # 팝업 UI
│   │   ├── Scene/      # 씬 UI
│   │   └── SubItem/    # 서브 아이템 UI
│   └── Utils/          # 유틸리티 및 Define
├── Resources/          # 런타임 로드 리소스
│   ├── Config/         # 설정 에셋 (GameConfig, IAPConfig, AdsConfig)
│   ├── Data/JsonData/  # 파싱된 JSON 데이터
│   ├── Prefabs/        # 프리팹
│   └── Sounds/         # 사운드 파일
└── Settings/           # URP 및 프로젝트 설정
```

## 핵심 시스템

### Manager System (Singleton 기반)

| Manager | 역할 |
|---------|------|
| **GameManager** | 게임 상태, 저장/로드, GameData 관리 |
| **SceneManager** | 씬 전환 및 로딩 처리 |
| **UIManager** | UI 생명주기, 팝업/씬 UI 관리 |
| **ResourceManager** | Addressables 기반 리소스 로드 |
| **SoundManager** | BGM, 효과음 재생 (2채널) |
| **EventManager** | 이벤트 기반 통신 |
| **SaveManager** | 직렬화/역직렬화 |
| **DataManager** | JSON 데이터 테이블 관리 |
| **ObjectManager** | GameObject 생성/관리 |
| **PoolManager** | 오브젝트 풀링 |
| **LocalizationManager** | 다국어 지원 (KOR, ENG) |
| **IAPManager** | 인앱 결제 |
| **AdsManager** | 광고 통합 |

### Object Hierarchy

```
MonoBehaviour
├── Singleton<T>        # 매니저 베이스
├── ObjectBase          # 게임 오브젝트 베이스
│   └── Creature        # 생명체 베이스
│       ├── Player
│       ├── Monster
│       └── Npc
├── BaseScene           # 씬 컨트롤러 베이스
└── UI_Base             # UI 컴포넌트 베이스
    ├── UI_Scene
    └── UI_Popup
```

### UI Binding System

enum 기반 자동 바인딩으로 UI 요소 관리:

```csharp
enum GameObjects { Player, Enemy }
enum Texts { ScoreText, TimerText }
enum Buttons { StartButton, ExitButton }

void Awake()
{
    BindObjects(typeof(GameObjects));
    BindTexts(typeof(Texts));
    BindButtons(typeof(Buttons));
}
```

## 사용법

### 데이터 파싱

1. `@ExcelData/` 폴더에 CSV 파일 추가
2. `Tools > Parse Excel` 실행
3. `Resources/Data/JsonData/`에 JSON 생성됨

### 새 Manager 추가

```csharp
public class MyManager : Singleton<MyManager>
{
    // 구현
}
```

### 새 Creature 추가

1. `Define.ECreatureType`에 enum 추가
2. `Creature` 상속 클래스 생성
3. 생성자에서 `_creatureType` 설정

### 새 UI 추가

1. `UI_Scene` 또는 `UI_Popup` 상속
2. enum으로 바인딩할 요소 정의
3. `Awake()`에서 `Bind<T>()` 호출

## 코딩 컨벤션

- **Private 필드**: `_camelCase` (언더스코어 접두사)
- **Public 프로퍼티**: `PascalCase`
- **Enum**: `EPascalCase` ('E' 접두사)
- **Manager**: `PascalCase` + "Manager" 접미사

## 설정 파일

- `Resources/Config/GameConfig.asset`: 게임 기본 설정
- `Resources/Config/IAPConfig.asset`: 인앱 결제 상품 정의
- `Resources/Config/AdsConfig.asset`: 광고 네트워크 설정

---

## 패치노트

### 25.12.22. v3 업데이트
- **Unity 6 업그레이드**: 6000.2.7f2
- **구조 개편**: Managers 폴더 평탄화 (Core/Contents 제거)
- **Singleton 패턴**: 독립적인 Singleton<T> 베이스 클래스
- **신규 Manager**: Event, Localization, IAP, Ads, Object, Sound
- **Controller 시스템**: ObjectBase → Creature → Player/Monster/Npc
- **Config 시스템**: ScriptableObject 기반 설정 (Game, IAP, Ads)
- **URP 설정**: Mobile/PC 분리된 렌더 파이프라인
- **리소스 구조**: @Resources → Resources로 변경
- **Addressables**: 레거시 설정 제거, 필요시 재구성

### 25.1.22. v2 업데이트
- GameManager 추가 (Save/Load)
- UIManager 팝업 관리 기능 추가

### 25.1.13. v1
- 초기 버전
- 씬 전환, CSV 파싱, UI 바인딩, 기본 Manager 시스템
