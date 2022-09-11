using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

[CreateAssetMenu(fileName = "SaveSystemDataObject" ,menuName ="Axis Mundi/Internal/SSDO")]
public class SaveSystemDataObject : ScriptableObject
#if UNITY_EDITOR
, IPreprocessBuildWithReport
#endif
{

    [field: SerializeField]
    public string CurrentFilePath { get; private set; }

    [field: SerializeField]
    public string[] Files { get; private set; }

    [field: SerializeField]
    public SaveManager.SaveFileData LoadedSave { get; set; }

    

    public void InitFiles()
    {
        Files = Directory.GetFiles(SaveManager.SavePath, "*.dat");
    }

    public void ChangeFilePath(string path) => CurrentFilePath = path;

#if UNITY_EDITOR
    #region onBuild
    public int callbackOrder => 100;

    public void OnPreprocessBuild(BuildReport report)
    {
        Files = null;
        CurrentFilePath = string.Empty;
        LoadedSave = null;
    }
    #endregion
#endif
}
