using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileSelector : MonoBehaviour
{
    [SerializeField]
    SaveFileButton button;
    private SaveFileButton[] buttons;

    [SerializeField]
    Transform boxTransform;

    [SerializeField] 
    private TMP_Text saveName, saveDate;

    [SerializeField]
    SaveManager manager;

    [SerializeField]
    ModalWindow prompt;

    [SerializeField] 
    private Button loadFileButton;



    private void OnEnable()
    {
        GenerateSaveButtons();
    }

    private void OnDisable()
    {
        ClearSaveButtons();
    }

    private void GenerateSaveButtons()
    {
        var fileNames = manager.GetFileNames();
        buttons = new SaveFileButton[fileNames.Length];


        for (int i = 0; i < fileNames.Length; i++)
        {
            var item = fileNames[i];
            var option = Instantiate(button, boxTransform);

            option.OnInteractFunction = delegate { DisplayLoad(item); };
            option.OnDeleteInteraction = delegate { DisplayDelete(item); };

            option.SetButtonLabel(item.Replace(".dat", string.Empty));

            buttons[i] = option;
        }
    }

    private void DisplayDelete(string item)
    {
        DisplayPrompt("Usuń zapis", $"Usunąć zapis {item}?", delegate { DeleteFile(item); });
    }

    private void DisplayLoad(string item)
    {
        manager.SelectFile(item);
        var save = manager.LoadFile();

        saveDate.text = save.Date.ToString("HH:mm:ss dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
        saveName.text = item;

        loadFileButton.onClick.RemoveAllListeners();

        loadFileButton.onClick.AddListener(
            delegate
            {
                DisplayPrompt("Wczytaj zapis", $"Wczytać zapis {item}?", WczytajZapis);
            }); 
    }

    private void DisplayPrompt(string title, string desc, Action act)
    {
        var window = Instantiate(prompt, transform.parent);
        window.PopulateButtons("Tak","Powrót");
        window.PopulateButtonActions(act);
        window.SetTitle(title);
        window.SetDescription(desc);
    }

    public void CheckForFilesAndPlay()
    {
        if (manager.GetFileNames().Length > 0) return;

        CreateNewSaveAndLoad();

    }


    private void ClearSaveButtons()
    {
        foreach (var item in buttons) { Destroy(item.gameObject); }
    }

    public void RefreshSaveFileButtons()
    {
        ClearSaveButtons();
        GenerateSaveButtons();
        saveDate.text = string.Empty;
        saveName.text = string.Empty;

        loadFileButton.onClick.RemoveAllListeners();
    }

    private void WczytajZapis()
    {
        manager.LoadSceneFromFile();
    }

    public void ConfirmCreation()
    {
        DisplayPrompt("Nowy zapis","stworzyć nowy zapis i zacząć grę?", CreateNewSaveAndLoad );
    }

    public void CreateNewSaveAndLoad()
    {
        int fileAmount = manager.GetFileNames().Length + 1;
        string saveName= $"Save {fileAmount}";

        int i=0;

        while (File.Exists(SaveManager.SavePath + saveName + ".dat"))
        {
            fileAmount += 1;
            saveName = $"Save {fileAmount}";
            i++;
            if (i == 100)
            {
                Debug.Log("Loop executed too many times");
                break;
            }
        }


        manager.CreateNewSaveFile(saveName);
        manager.LoadSceneFromFile();
    }

    //do input field podczas tworzenia save

    private void DeleteFile(string name)
    {
        manager.DeleteSaveFile(name);
        manager.CheckFilesInDirectory();
        RefreshSaveFileButtons();
    }


}
