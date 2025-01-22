using System;
using UnityEngine;

public static class Util
{
	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
			component = go.AddComponent<T>();

		return component;
	}

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		Transform transform = FindChild<Transform>(go, name, recursive);
		if (transform == null)
			return null;

		return transform.gameObject;
	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Transform transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					T component = transform.GetComponent<T>();
					if (component != null)
						return component;
				}
			}
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>())
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
					return component;
			}
		}

		return null;
	}

	public static T ParseEnum<T>(string value)
	{
		return (T)Enum.Parse(typeof(T), value, true);
	}

	public static Color HexToColor(string color)
	{
		if (color.Contains("#") == false)
			color = $"#{color}";

		ColorUtility.TryParseHtmlString(color, out Color parsedColor);

		return parsedColor;
	}
	
	public static string Encrypt(string data)
	{
		return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
	}

	public static string Decrypt(string encryptedData)
	{
		try
		{
			return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedData));
		}
		catch (FormatException)
		{
			Debug.LogError("Invalid encrypted data format.");
			return null;
		}
	}
}
