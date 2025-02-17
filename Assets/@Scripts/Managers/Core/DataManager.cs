using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> MakeDict();
	//bool Validate();
}

public class DataManager
{
	public Dictionary<string, Data.TextData> TextDic { get; private set; } = new Dictionary<string, Data.TextData>();

	public void Init()
	{
		TextDic = LoadJson<Data.TextDataLoader, string, Data.TextData>("TextData").MakeDict();
	}

	private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
		return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
