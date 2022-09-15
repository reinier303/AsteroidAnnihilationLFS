using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveEditor : EditorWindow
{



    [MenuItem("Tools/Save Editor")]
    private static void Init()
    {
        var window = (SaveEditor)GetWindow(typeof(SaveEditor));
        window.minSize = new Vector2(400, 300);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Delete Player Save Data"))
        {
            EditorGUILayout.LabelField("Save Editor", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("\n");
            string path = Application.persistentDataPath + "/saveData/PlayerData.sav";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("PlayerData at: " + path + " deleted.");
            }
            else
            {
                Debug.Log("No file found at: " + path);
            }
        }
        if (GUILayout.Button("Delete Shop Save Data"))
        {
            EditorGUILayout.LabelField("Save Editor", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("\n");
            string path = Application.persistentDataPath + "/saveData/ShopItemsUnlocked.sav";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("ShopItemsUnlocked at: " + path + " deleted.");
            }
            else
            {
                Debug.Log("No file found at: " + path);
            }
        }
        if (GUILayout.Button("Delete All Save Data"))
        {
            EditorGUILayout.LabelField("Save Editor", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("\n");
            string path = Application.persistentDataPath + "/saveData";
            DirectoryInfo directory = new DirectoryInfo(path);
            if (Directory.Exists(path))
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                Debug.Log("Files at: " + path + " deleted.");
            }
            else 
            {
                Debug.Log("No file found at: " + path);                
            }
        }
    }
}
