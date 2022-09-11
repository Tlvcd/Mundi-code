using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


using UnityEngine.SceneManagement;
using System.Linq;

public class SaveManager : MonoBehaviour
{

    public static string SavePath { get; private set; }

    [SerializeField]
    SaveSystemDataObject datObj;

    [SerializeField] private LoadAsset levelLoaderAsset;

    private void Awake()
    {
        //if (!string.IsNullOrEmpty(SavePath)) return;



        SavePath = Application.persistentDataPath + "/Saves/";

        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        CheckFilesInDirectory();
    }
    

    public void CheckFilesInDirectory() => datObj.InitFiles();

  
    public void SelectFile(string name, bool includeExtension =false)
    {
        datObj.ChangeFilePath(string.Empty);
        var dir = SavePath + name;


        if (includeExtension) dir += ".dat";

        foreach (var item in datObj.Files)
        {
            if (item != dir) continue;

            datObj.ChangeFilePath(item);
            break;
        }

        if (datObj.CurrentFilePath != string.Empty) return;

        Debug.LogError("No files found");
    }

    public void DeleteSaveFile(string name)
    {
        name = SavePath + name;
        if (!File.Exists(name)) return;
        File.Delete(name);
    }

    /// <summary>
    /// Checks if file exists, if not creates new and return true.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CreateNewSaveFile(string name)
    {
        name = SavePath + name + ".dat";
        datObj.ChangeFilePath(name);

        if (datObj.Files.Contains(name)) return false;

        var save = CreateNewSave();
        save.saveName = name;

        SaveFile(save);
        datObj.InitFiles();

        return true;
    }

    private void SaveFile(object data)
    {
        if (datObj.CurrentFilePath == string.Empty) return;

        using var file = File.Open(datObj.CurrentFilePath, FileMode.Create);

        var formatter = new BinaryFormatter();
        formatter.Serialize(file, data);
        file.Close();
    }

    public SaveFileData LoadFile()
    {
        if (!File.Exists(datObj.CurrentFilePath))
            return CreateNewSave();

        using var stream = File.Open(datObj.CurrentFilePath, FileMode.Open);
        var formatter = new BinaryFormatter();

        if (stream.Length == 0)
            return CreateNewSave();

        try
        {
            var data = (SaveFileData)
                formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        catch (System.Exception)
        {
            stream.Close();

            return CreateNewSave();
        }
    }

    

    [Button]
    public void SaveGame()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    [Button]
    public void LoadSceneFromFile()
    {
        datObj.LoadedSave = LoadFile();
        levelLoaderAsset.LoadLevel(datObj.LoadedSave.SceneName);
    }

    public string GetSaveSceneName() => datObj.LoadedSave.SceneName;

    private void CaptureState(SaveFileData state)
    {
        var activeSaveables = FindObjectsOfType<Saveable>(true);

        foreach (var saveable in activeSaveables)
        {
            state.sceneObjects[saveable.GUID] = saveable.SaveState();
        }

        state.SceneName = GetSceneName();

        state.Date = DateTime.UtcNow;
    }


    public void RestoreState()
    {
        var activeSaveables = FindObjectsOfType<Saveable>(true);
        var objs = datObj.LoadedSave.sceneObjects;

        foreach (var saveable in activeSaveables)
        {
            if(objs.TryGetValue(saveable.GUID, out object value))   
            {
                saveable.LoadState(value);
            }
        }
    }

    [System.Serializable]
    public class SaveFileData
    {

        public DateTime Date;

        public string saveName;

        public Dictionary<string, object> sceneObjects;

        public string SceneName;

        public SaveFileData(Dictionary<string, object> objs, string scene)
        {
            Date = DateTime.UtcNow;

            sceneObjects = new Dictionary<string, object>(objs);

            SceneName = scene;
        }

    }


    private void SaveFileSceneName() => Debug.Log(LoadFile().SceneName);

    public string[] GetFileNames()
    {
        var length = datObj.Files.Length;
        var fileNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            fileNames[i] = Path.GetFileName(datObj.Files[i]);
        }
        return fileNames;
    }



    private SaveFileData CreateNewSave()
    {
        return new SaveFileData(new Dictionary<string, object>(),
            "Sosnowiec"); //testowanie narazie
    }

    private string GetSceneName() => SceneManager.GetActiveScene().name;


}
