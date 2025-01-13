using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager
{
	private int _order = 10;

	private UI_Scene _sceneUI = null;
	public UI_Scene SceneUI
	{
		set { _sceneUI = value; }
		get { return _sceneUI; }
	}

	public GameObject Root
	{
		get
		{
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };
			return root;
		}
	}

	public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
	{
		Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
		if (canvas == null)
		{
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.overrideSorting = true;
		}

		CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
		if (cs != null)
		{
			cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			cs.referenceResolution = new Vector2(1080, 1920);
		}

		go.GetOrAddComponent<GraphicRaycaster>();

		if (sort)
		{
			canvas.sortingOrder = _order;
			_order++;
		}
		else
		{
			canvas.sortingOrder = sortOrder;
		}
	}

	public T GetSceneUI<T>() where T : UI_Base
	{
		return _sceneUI as T;
	}

	public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"{name}");
		if (parent != null)
			go.transform.SetParent(parent);

		Canvas canvas = go.GetOrAddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.worldCamera = Camera.main;

		return Util.GetOrAddComponent<T>(go);
	}

	public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate(name, parent, pooling);
		go.transform.SetParent(parent);

		return Util.GetOrAddComponent<T>(go);
	}

	public T ShowBaseUI<T>(string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate(name);
		T baseUI = go.GetOrAddComponent<T>();

		go.transform.SetParent(Root.transform);

		return baseUI;
	}

	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate(name);
		T sceneUI = go.GetOrAddComponent<T>();
		_sceneUI = sceneUI;

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	public void Clear()
	{
		_sceneUI = null;
	}
}
