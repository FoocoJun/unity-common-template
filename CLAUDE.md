# Unity Framework Project - AI Assistant Instructions

## Project Overview
This is a Unity game framework project built with C# 9.0 targeting .NET Framework 4.7.1. The project follows a modular architecture with manager-based systems, object pooling, and a robust UI framework.

**Workspace**: 이 파일(`CLAUDE.md`)이 위치한 프로젝트 루트 디렉토리

## Project Structure

### 1. Core Architecture Patterns

#### Singleton Pattern
- **File**: `Assets\@Scripts\Managers\Singleton.cs`
- All manager classes inherit from `Singleton<T>`
- Automatically creates GameObject if instance doesn't exist
- Uses `DontDestroyOnLoad` to persist across scenes
- Thread-safe initialization with `_init` flag

#### Manager System
All managers are singletons and handle specific game systems:
- **GameManager**: Game state, gold, level, save data (`GameData` class)
- **SceneManager**: Scene loading and transitions
- **UIManager**: UI lifecycle, popups, scene UI
- **ResourceManager**: Asset loading and resource management
- **SoundManager**: BGM and sound effects (2 channels: Bgm, Effect)
- **EventManager**: Event-driven communication (EventType enum)
- **SaveManager**: Serialization/deserialization of game data
- **DataManager**: Game data tables and configurations
- **ObjectManager**: GameObject lifecycle and spawning
- **PoolManager**: Object pooling system (uses `Poolable` interface)
- **LocalizationManager**: Multi-language support (KOR, ENG)
- **IAPManager**: In-app purchases (Unity IAP integration)
- **AdsManager**: Advertisement integration

### 2. Object Hierarchy

```
MonoBehaviour
├── Singleton<T> (Managers)
├── ObjectBase (Base for all game objects)
│   └── Creature (EObjectType.Creature)
│       ├── Player (ECreatureType.Player)
│       ├── Monster (ECreatureType.Monster)
│       └── Npc (ECreatureType.Npc)
├── BaseScene (Scene controllers)
└── UI_Base (UI components)
    ├── UI_Scene
    └── UI_Popup
```

**Key Points**:
- `ObjectBase`: Base class for all interactive objects, has `_objectType` field
- `Creature`: Base for all living entities, has `_creatureType` field
- Inheritance chain: `MonoBehaviour → ObjectBase → Creature → [Player/Monster/Npc]`

### 3. Enumerations (Define.cs)

```csharp
EScene: Unknown, LoadingScene, DevScene
EEventType: None, GoldChanged, LanguageChanged
ELanguage: KOR, ENG
EObjectType: None, Creature, Projectile, Env
ECreatureType: None, Player, Monster, Npc
ESound: Bgm, Effect, Max
```

### 4. UI System

#### UI_Base Pattern
- **File**: `Assets\@Scripts\UI\UI_Base.cs`
- Uses enum-based binding for UI elements
- Dictionary caching: `_objects` stores bound components
- Binding methods: `BindObjects()`, `BindImages()`, `BindTexts()`, `BindButtons()`
- Getter methods: `GetObject()`, `GetText()`, `GetButton()`, `GetImage()`
- `RefreshUI()` virtual method for UI updates

#### UI Types
- **UI_Scene**: Full-screen UI (HUD, main menu)
- **UI_Popup**: Modal dialogs and popups

#### Binding Flow
1. Define enum for UI element names
2. Call `Bind<T>(Type enumType)` in Awake/Start
3. Use `Get<T>(int enumIndex)` to access elements
4. Uses `Utils.FindChildComponent` for recursive hierarchy search

### 5. Utility Classes

#### Utils.cs
- `GetOrAddComponent<T>()`: Safe component access/creation
- `FindChildGameObject()`: Find child by name (recursive option)
- `FindChildComponent<T>()`: Find component in children (recursive option)
- `GetRootTransform()`: Get or create root transform

#### Extension.cs
Custom extension methods for Unity types

### 6. Configuration System

- **GameConfig**: General game settings
- **IAPConfig**: In-app purchase product definitions
- **AdsConfig**: Ad network settings

### 7. Data System

- **GameData**: Serializable game state (Gold, Level, UpgradeCount)
- **TextData**: Localization text data
- **ItemData**: Game item definitions

### 8. Scenes

- **LoadingScene**: Initial loading and initialization
- **DevScene**: Development/testing scene
- **BaseScene**: Base class with `SceneType` property

## Coding Conventions

### Style Guidelines
1. **Naming**:
   - Private fields: `_camelCase` with underscore prefix
   - Public properties: `PascalCase`
   - Enums: `EPascalCase` with 'E' prefix
   - Managers: `PascalCase` + "Manager" suffix

2. **Access Modifiers**:
   - Default to `protected` for inherited fields
   - Use `public` properties with backing fields for encapsulation
   - Serialize with `[SerializeField]` for private fields in Inspector

3. **Patterns**:
   - Managers use Singleton pattern
   - UI uses enum-based binding pattern
   - Controllers inherit from ObjectBase hierarchy
   - Use `static Define` enums with `using static Define;`

4. **Comments**:
   - Minimal comments; prefer self-documenting code
   - Use `// TODO` for pending implementations
   - Korean comments acceptable (developer preference)

### Unity-Specific
- Constructor usage for enum initialization (Player, Monster, Npc)
- `Awake()` for initialization, `Start()` for setup with dependencies
- `DontDestroyOnLoad` for persistent managers
- Object pooling preferred over Instantiate/Destroy

## Common Modification Patterns

### Adding a New Manager
1. Inherit from `Singleton<T>`
2. Place in `Assets\@Scripts\Managers\`
3. Initialize via GameManager or on first access

### Adding a New Creature Type
1. Add enum to `Define.ECreatureType`
2. Create class inheriting from `Creature`
3. Set `_creatureType` in constructor

### Adding UI Elements
1. Create enum matching GameObject names
2. Call `Bind<Type>(enum)` in Awake
3. Access via `Get<Type>(enumIndex)`

### Adding Events
1. Add to `Define.EEventType`
2. Trigger via `EventManager.Instance.TriggerEvent()`
3. Subscribe/unsubscribe in component lifecycle

## External Dependencies
- **TextMeshPro**: UI text rendering
- **Unity IAP**: In-app purchases (v5.1.1)
- Samples included for IAP implementation reference

## Project Structure
```
Assets/
├── @Scripts/           # Main code
│   ├── Config/        # Configuration ScriptableObjects
│   ├── Controllers/   # Game object controllers
│   │   └── Creature/  # Player, Monster, Npc
│   ├── Data/          # Data classes
│   ├── Managers/      # Singleton managers
│   ├── Scenes/        # Scene controllers
│   ├── UI/            # UI components
│   │   ├── Popup/
│   │   └── Scene/
│   └── Utils/         # Utilities and definitions
├── @Dev/              # Development scenes
└── Samples/           # Unity package samples
```

## Key Files Reference
- **Define.cs**: All game enumerations
- **Utils.cs**: Helper functions
- **Singleton.cs**: Base manager pattern
- **GameManager.cs**: Core game state
- **UI_Base.cs**: UI binding system
- **ObjectBase.cs**: Game object base class

## Notes for AI Assistant
- Always check `Define.cs` for existing enums before creating new ones
- Follow the established singleton pattern for managers
- Use enum-based binding for all UI implementations
- Respect the object hierarchy: ObjectBase → Creature → specific types
- Maintain constructor-based type initialization pattern
- Prefer protected fields over public for inheritance
- Keep manager responsibilities single-purpose
- Use Utils helpers instead of direct Unity API when available
