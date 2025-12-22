# Addressables 설정 가이드

## 1. Addressables 패키지 설치

### Unity Package Manager를 통한 설치:
1. Unity Editor에서 `Window > Package Manager` 열기
2. 왼쪽 상단의 `Packages:` 드롭다운에서 `Unity Registry` 선택
3. `Addressables` 검색
4. `Install` 버튼 클릭

### 수동 설치 (manifest.json):
`Packages/manifest.json` 파일에 다음 추가:
```json
{
  "dependencies": {
    "com.unity.addressables": "1.21.19"
  }
}
```

## 2. 프로젝트 설정

### Scripting Define Symbols 추가:
1. `Edit > Project Settings > Player` 열기
2. `Other Settings > Scripting Defines` 섹션 찾기
3. `UNITY_ADDRESSABLES` 추가
4. `Apply` 클릭

또는 스크립트로 자동 추가:
```csharp
// Editor 폴더에 배치
#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public class AddressablesDefineSymbol
{
    static AddressablesDefineSymbol()
    {
        string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);
        
        if (!currentDefines.Contains("UNITY_ADDRESSABLES"))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                currentDefines + ";UNITY_ADDRESSABLES");
        }
    }
}
#endif
```

## 3. Addressables 그룹 설정

### 초기 설정:
1. `Window > Asset Management > Addressables > Groups` 열기
2. 첫 실행 시 `Create Addressables Settings` 버튼 클릭

### 리소스에 Label 추가:
다음 Label들을 생성하고 리소스에 할당:
- `Prefabs` - 모든 Prefab 에셋
- `Config` - Configuration ScriptableObjects
- `Data` - JSON 데이터 파일
- `Sounds` - 오디오 클립

### Label 생성 방법:
1. Addressables Groups 창에서 상단의 `Labels` 버튼 클릭
2. `+` 버튼으로 새 Label 추가

### 에셋을 Addressable로 만들기:
1. Project 창에서 에셋 선택
2. Inspector에서 `Addressable` 체크박스 활성화
3. `Labels` 드롭다운에서 적절한 Label 선택

## 4. ResourceManager 사용법

### AddressableLoader로 전환:
```csharp
// LoadingScene.cs 등의 초기화 코드에서
#if UNITY_ADDRESSABLES
ResourceManager.Instance.SetLoader(new AddressableLoader());
#endif

ResourceManager.Instance.LoadAll(OnProgress, OnComplete);
```

### 기존 ResourcesLoader 유지:
```csharp
// 아무 설정 없이 기본 ResourcesLoader 사용
ResourceManager.Instance.LoadAll(OnProgress, OnComplete);
```

## 5. 리소스 로딩 방식 비교

### ResourcesLoader (기본):
- **경로**: `Assets/Resources/` 폴더 필수
- **빌드 크기**: Resources 폴더의 모든 에셋이 빌드에 포함
- **로딩**: 동기식, 간단한 문자열 키 사용
- **장점**: 설정 불필요, 간단한 구조
- **단점**: 빌드 크기 증가, 메모리 관리 제한적

### AddressableLoader:
- **경로**: 프로젝트 어디든 가능
- **빌드 크기**: 필요한 에셋만 선택적 포함 가능
- **로딩**: 비동기식, 라벨/주소 기반
- **장점**: 
  - 원격 에셋 다운로드 가능 (CDN)
  - 메모리 관리 효율적
  - 빌드 크기 최적화
  - 에셋 번들 자동 관리
- **단점**: 초기 설정 필요, 약간의 학습 곡선

## 6. 사용 예제

### 프리팹 로드 및 인스턴스화:
```csharp
// 두 방식 모두 동일한 인터페이스 사용
GameObject player = ResourceManager.Instance.Instantiate("Player");
```

### 특정 타입 리소스 가져오기:
```csharp
AudioClip bgm = ResourceManager.Instance.Get<AudioClip>("MainTheme");
GameConfig config = ResourceManager.Instance.Get<GameConfig>("GameConfig");
```

### 리소스 해제:
```csharp
// 씬 전환 시 또는 게임 종료 시
ResourceManager.Instance.ReleaseAll();
```

## 7. 마이그레이션 체크리스트

기존 Resources 폴더에서 Addressables로 마이그레이션:

- [ ] Addressables 패키지 설치
- [ ] UNITY_ADDRESSABLES Define Symbol 추가
- [ ] Addressables Settings 생성
- [ ] Label 생성 (Prefabs, Config, Data, Sounds)
- [ ] 에셋을 Addressable로 마킹 및 Label 할당
- [ ] LoadingScene에서 AddressableLoader 설정
- [ ] 테스트 빌드 및 검증
- [ ] (선택) Resources 폴더 제거

## 8. 주의사항

1. **동시 로딩**: 
   - AddressableLoader는 비동기 로딩이므로 `LoadAll` 완료 전에 `Get` 호출 시 null 반환 가능
   - 반드시 `onComplete` 콜백 이후에 리소스 사용

2. **메모리 관리**:
   - `ReleaseAll()` 호출 시 모든 Addressable 핸들이 해제됨
   - 씬 전환 시 필요한 에셋만 유지하려면 개별 해제 로직 추가 필요

3. **빌드 설정**:
   - Addressables 그룹의 빌드 설정에서 압축 방식 선택 (LZ4, LZMA)
   - Remote 빌드 시 서버 URL 설정 필요

4. **에디터 테스트**:
   - `Play Mode Script` 설정에서 테스트 방식 선택 가능
   - `Use Asset Database (fastest)` - 에디터에서 빠른 테스트
   - `Simulate Groups (advanced)` - 실제 빌드와 유사한 동작

## 9. 트러블슈팅

### "UNITY_ADDRESSABLES가 정의되지 않음" 에러:
- Scripting Define Symbols에 `UNITY_ADDRESSABLES` 추가 확인
- Unity Editor 재시작

### "Label을 찾을 수 없음" 에러:
- Addressables Groups 창에서 Label 생성 확인
- 에셋에 올바른 Label 할당 확인

### 로딩 완료되지 않음:
- 모든 Label이 유효한지 확인 (빈 Label은 실패)
- 콘솔에서 AsyncOperationStatus.Failed 로그 확인

### 빌드 후 리소스 로드 실패:
- Addressables Groups 빌드 실행 확인 (`Build > New Build > Default Build Script`)
- 빌드 출력물에 Addressables 데이터 포함 확인

## 10. 고급 기능 (향후 확장)

### 개별 에셋 로딩:
```csharp
// IResourceLoader 인터페이스에 추가 메서드
void LoadAsync<T>(string key, Action<T> onComplete) where T : UnityEngine.Object;
```

### 원격 에셋 다운로드:
```csharp
// Addressables Profile에서 Remote 경로 설정
// BuildTarget에 따라 자동으로 원격 서버에서 다운로드
```

### 에셋 업데이트:
```csharp
// 런타임에 새 에셋 다운로드
Addressables.UpdateCatalogs();
```
