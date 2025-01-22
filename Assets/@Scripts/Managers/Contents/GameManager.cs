using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Define;

[Serializable]
public class GameSaveData
{
    public string LastSaveDate = "";
    
	// 그 외 저장 값   
}

public class GameManager
{
    #region GameData
    public GameSaveData SaveData { get; set; } = new GameSaveData();

    public bool IsInitialized = false;
    #endregion
    
    #region Save & Load
    private string Path { get { return Application.persistentDataPath + "/SaveData.json"; } }
    
    public void InitGame()
	{
		if (File.Exists(Path))
			return;
		
		// 저장된 데이터 없을 시 초기 데이터 선언
		{
			
		}
	}

	public void SaveGame()
	{
		// 저장이 필요한 데이터 SaveData에 재할당
		{
			
		}
        
        // common
        {
			SaveData.LastSaveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        // Save TODO: 비화 어떻게 하지? 1.0.0 이후 고민하기
        string jsonStr = JsonUtility.ToJson(SaveData);
        string encryptedStr = Util.Encrypt(jsonStr);
        File.WriteAllText(Path, encryptedStr);
        
		Debug.Log($"Save Game Completed : {Path}");
	}

	public bool LoadGame()
	{
		// Load
		if (File.Exists(Path) == false)
			return false;

		string encryptedStr = File.ReadAllText(Path);
		string jsonStr = Util.Decrypt(encryptedStr);
		GameSaveData data = JsonUtility.FromJson<GameSaveData>(jsonStr);

		if (data != null)
			SaveData = data;

		// TODO: 저장 데이터 형태가 기존과 맞지 않으면 어떡하지? 1.0.0 이후 고민하기. 세이브 데이터에 저장 버전을 담아도 될듯.
		// 불러올 데이터 SaveData에서 추출 및 할당
		{

		}

		Debug.Log($"Save Game Loaded : {Path}");
		return true;
	}

    public void ClearSaveData()
    {
        SaveData = null;
        File.Delete(Path);
    }
    #endregion
}
