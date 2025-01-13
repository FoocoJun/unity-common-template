using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	public static bool Initialized { get; set; } = false;

	private static Managers s_instance;
	public static Managers Instance { get { Init(); return s_instance; } }

	#region Core
	private SceneManagerEx _scene = new SceneManagerEx();
	
	public static SceneManagerEx Scene { get { return Instance?._scene; } }
	#endregion

	public static void Init()
	{
		if (s_instance == null && Initialized == false)
		{
			Initialized = true;

			GameObject go = GameObject.Find("@Managers");
			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);

			// 초기화
			s_instance = go.GetComponent<Managers>();
		}
	}

}
