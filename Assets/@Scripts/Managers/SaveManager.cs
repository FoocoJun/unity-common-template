using System.Collections;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager : Singleton<SaveManager>
{
    private string _savePath;
    private const string SAVE_FILE_NAME = "GameData.json";
    
    [SerializeField] private float _autoSaveInterval = 10f;
    private bool _autoSaveEnabled = true;
    private Coroutine _autoSaveCoroutine;

    void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        StartAutoSave();
    }

    public void StartAutoSave()
    {
        if (_autoSaveCoroutine == null && _autoSaveEnabled)
        {
            _autoSaveCoroutine = StartCoroutine(CoAutoSaveRoutine());
            Debug.Log($"SaveManager: Auto-save started (interval: {_autoSaveInterval}s)");
        }
    }

    public void StopAutoSave()
    {
        if (_autoSaveCoroutine != null)
        {
            StopCoroutine(_autoSaveCoroutine);
            _autoSaveCoroutine = null;
            Debug.Log("SaveManager: Auto-save stopped");
        }
    }

    private IEnumerator CoAutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoSaveInterval);
            Save();
        }
    }

    public void Save()
    {
        GameData gameData = GameManager.Instance.GameData;
        if (gameData == null)
        {
            Debug.LogWarning("SaveManager: GameData is null, cannot save.");
            return;
        }

        try
        {
            string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(_savePath, json);
            Debug.Log($"SaveManager: Game saved to {_savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveManager: Failed to save game. Error: {e.Message}");
        }
    }

    public void Load()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("SaveManager: No save file found. Starting with default data.");
            return;
        }

        try
        {
            string json = File.ReadAllText(_savePath);
            GameData gameData = JsonConvert.DeserializeObject<GameData>(json);
            
            if (gameData != null)
            {
                GameManager.Instance.GameData = gameData;
                Debug.Log($"SaveManager: Game loaded from {_savePath}");
            }
            else
            {
                Debug.LogWarning("SaveManager: Failed to deserialize GameData.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveManager: Failed to load game. Error: {e.Message}");
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(_savePath))
        {
            try
            {
                File.Delete(_savePath);
                Debug.Log("SaveManager: Save file deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"SaveManager: Failed to delete save file. Error: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("SaveManager: No save file to delete.");
        }
    }

    public void Reset()
    {
        try
        {
            // TODO: DataManager에서 초기 설정값 가져오기
            GameData defaultGameData = new GameData();
            
            GameManager.Instance.GameData = defaultGameData;
            
            string json = JsonConvert.SerializeObject(defaultGameData, Formatting.Indented);
            File.WriteAllText(_savePath, json);
            
            Debug.Log("SaveManager: Save file reset to default values.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveManager: Failed to reset save file. Error: {e.Message}");
        }
    }

    public bool HasSaveData()
    {
        return File.Exists(_savePath);
    }

    public string GetSavePath()
    {
        return _savePath;
    }
}
