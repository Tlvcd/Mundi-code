using UnityEngine;
using System.IO;


public class SettingsFileManager : MonoBehaviour{


    [SerializeField]
    private SettingsContainer container;

    private string savePath;

    private Camera cam;

    private void Awake()
    {
        savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "Settings.json";

        if (!File.Exists(savePath))
        {
            Save();
        }

        LoadSavedOptions();
        
    }

    public void Save()
    {
        var save = container.Settings ?? new SettingValues();

        var saveJson = JsonUtility.ToJson(save, true);

        using var stream = new StreamWriter(savePath);

        stream.Write(saveJson);

        stream.Close();
    }

    public void LoadSavedOptions()
    {
        using var streamReader = new StreamReader(savePath);

        //if (streamReader==null) Save();

        var readOptionsFile = streamReader.ReadToEnd();

        streamReader.Close();

        container.ImportValues(JsonUtility.FromJson<SettingValues>(readOptionsFile));

        container.RefreshSettings();
    }

}

