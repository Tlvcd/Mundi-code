using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempSceneSwitcharoo : MonoBehaviour{
    public void GoToMenu()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
