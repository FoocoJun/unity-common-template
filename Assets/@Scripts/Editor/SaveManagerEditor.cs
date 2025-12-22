using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveManagerEditor : EditorWindow
{
#if UNITY_EDITOR

    [MenuItem("Tools/Save Manager")]
    public static void ShowWindow()
    {
        GetWindow<SaveManagerEditor>("Save Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Save File Management", EditorStyles.boldLabel);
        GUILayout.Space(10);

        string savePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        bool saveFileExists = File.Exists(savePath);

        EditorGUILayout.LabelField("Save File Path:", EditorStyles.label);
        EditorGUILayout.SelectableLabel(savePath, GUILayout.Height(20));
        GUILayout.Space(5);

        EditorGUILayout.LabelField("Save File Status:", saveFileExists ? "Exists" : "Not Found");
        
        if (saveFileExists)
        {
            FileInfo fileInfo = new FileInfo(savePath);
            EditorGUILayout.LabelField("Last Modified:", fileInfo.LastWriteTime.ToString());
            EditorGUILayout.LabelField("File Size:", $"{fileInfo.Length} bytes");
        }

        GUILayout.Space(10);

        GUI.enabled = saveFileExists;
        if (GUILayout.Button("Open Save File Location", GUILayout.Height(30)))
        {
            EditorUtility.RevealInFinder(savePath);
        }

        if (GUILayout.Button("View Save File Content", GUILayout.Height(30)))
        {
            string content = File.ReadAllText(savePath);
            Debug.Log($"<color=cyan>=== Save File Content ===</color>\n{content}");
        }

        GUILayout.Space(10);

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Delete Save File", GUILayout.Height(40)))
        {
            DeleteSaveFile(savePath);
        }
        GUI.backgroundColor = Color.white;
        GUI.enabled = true;

        GUILayout.Space(20);

        if (GUILayout.Button("Refresh", GUILayout.Height(25)))
        {
            Repaint();
        }
    }

    private void DeleteSaveFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("<color=green>Save file deleted successfully!</color>");
                EditorUtility.DisplayDialog("Success", "Save file has been deleted.", "OK");
                Repaint();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to delete save file: {e.Message}");
            EditorUtility.DisplayDialog("Error", $"Failed to delete save file:\n{e.Message}", "OK");
        }
    }

#endif
}
