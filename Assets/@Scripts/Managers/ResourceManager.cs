using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface IResourceLoader
{
    void LoadAll(Action<float> onProgress = null, Action onComplete = null);
    T Get<T>(string key) where T : UnityEngine.Object;
    GameObject Instantiate(string key, Transform parent = null);
    void ReleaseAll();
}

public class ResourceManager : Singleton<ResourceManager>
{
    IResourceLoader _loader = new ResourcesLoader();

    public void SetLoader(IResourceLoader loader)
    {
        _loader?.ReleaseAll();
        _loader = loader;
    }

    public void LoadAll(Action<float> onProgress = null, Action onComplete = null)
    {
        _loader.LoadAll(onProgress, onComplete);
	}

    public T Get<T>(string key) where T : UnityEngine.Object
    {
        return _loader.Get<T>(key);
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        return _loader.Instantiate(key, parent);
    }

    public void ReleaseAll()
    {
        _loader.ReleaseAll();
	}
}

public class ResourcesLoader : IResourceLoader
{
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

	public void LoadAll(Action<float> onProgress = null, Action onComplete = null)
	{
        List<string> paths = new List<string> { "Prefabs", "Config", "Data/JsonData", "Sounds" };
        int totalPaths = paths.Count;
        int loadedPaths = 0;

        foreach (string path in paths)
        {
			UnityEngine.Object[] resources = Resources.LoadAll(path);
            foreach (UnityEngine.Object resource in resources)
            {
                string fullKey = $"{resource.name}_{resource.GetType().Name}";
                if (_resources.ContainsKey(fullKey) == false)
                    _resources.Add(fullKey, resource);
            }

			loadedPaths++;
			float progress = (float)loadedPaths / totalPaths;
			onProgress?.Invoke(progress);

            if (loadedPaths >= totalPaths)
                onComplete?.Invoke();
		}
	}

	public T Get<T>(string key) where T : UnityEngine.Object
    {
		string fullKey = $"{key}_{typeof(T).Name}";

        if (_resources.TryGetValue(fullKey, out UnityEngine.Object resource))
            return resource as T;
            
        return null;
	}

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = Get<GameObject>(key);
        if (prefab == null)
            return null;

        GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);
        instance.name = prefab.name;
        return instance;
	}

    public void ReleaseAll()
    {
        foreach (UnityEngine.Object resource in _resources.Values)
            Resources.UnloadAsset(resource);

        _resources.Clear();
        Resources.UnloadUnusedAssets();
    }
}

public class AddressableLoader : IResourceLoader
{
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
    private List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();
    private List<string> _labels = new List<string> { "Prefabs", "Config", "Data", "Sounds" };

    public void LoadAll(Action<float> onProgress = null, Action onComplete = null)
    {
        int totalLabels = _labels.Count;
        int loadedLabels = 0;
        bool isCompleted = false;

        foreach (string label in _labels)
        {
            var handle = Addressables.LoadAssetsAsync<UnityEngine.Object>(label, (resource) =>
            {
                string fullKey = $"{resource.name}_{resource.GetType().Name}";
                if (_resources.ContainsKey(fullKey) == false)
                    _resources.Add(fullKey, resource);
            });

            _handles.Add(handle);

            handle.Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load label: {label}");
                }

                loadedLabels++;
                float progress = (float)loadedLabels / totalLabels;
                onProgress?.Invoke(progress);

                if (loadedLabels >= totalLabels && !isCompleted)
                {
                    isCompleted = true;
                    onComplete?.Invoke();
                }
            };
        }
    }

    public T Get<T>(string key) where T : UnityEngine.Object
    {
        string fullKey = $"{key}_{typeof(T).Name}";

        if (_resources.TryGetValue(fullKey, out UnityEngine.Object resource))
            return resource as T;

        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = Get<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogWarning($"AddressableLoader: Prefab not found for key: {key}");
            return null;
        }

        GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);
        instance.name = prefab.name;
        return instance;
    }

    public void ReleaseAll()
    {
        foreach (var handle in _handles)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }

        _handles.Clear();
        _resources.Clear();
    }
}