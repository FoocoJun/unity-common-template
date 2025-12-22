using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface IValidate
{
    bool Validate();
}

public interface IDataLoader<Key, Value> : IValidate
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : Singleton<DataManager>
{
    private HashSet<IValidate> _loaders = new HashSet<IValidate>();

    public Dictionary<string, TextData> TextDict { get; private set; } = new Dictionary<string, TextData>();
    public Dictionary<int, ItemData> ItemDict { get; private set;  } = new Dictionary<int, ItemData>();

    public GameConfig Config { get; private set; }
    public AdsConfig AdsConfig { get; private set; }
    public IAPConfig IAPConfig { get; private set; }

    public void LoadData()
    {
        // Load ScriptableObject
        Config = LoadScriptableObject<GameConfig>("GameConfig");
        AdsConfig = LoadScriptableObject<AdsConfig>("AdsConfig");
        IAPConfig = LoadScriptableObject<IAPConfig>("IAPConfig");

        // Load JSON Data
        TextDict = LoadJson<TextDataLoader, string, TextData>("TextData").MakeDict();
        ItemDict = LoadJson<ItemDataLoader, int, ItemData>("ItemData").MakeDict();
        // TODO

        Validate();
    }

    private T LoadScriptableObject<T>(string path) where T : ScriptableObject
    {
        T config = ResourceManager.Instance.Get<T>(path);
        
        if (config == null)
        {
            Debug.LogError($"Failed to load ScriptableObject: {path}");
        }
        
        return config;
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : IDataLoader<Key, Value>
    {
        TextAsset textAsset = ResourceManager.Instance.Get<TextAsset>(path);

        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        _loaders.Add(loader);
        Debug.Log(path);

        return loader;
    }

    private bool Validate()
    {
        bool success = true;

        foreach (IValidate loader in _loaders)
        {
            if (loader.Validate() == false)
                success = false;
        }

        _loaders.Clear();

        return success;
    }
}
