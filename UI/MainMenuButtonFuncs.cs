using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonFuncs : MonoBehaviour
{
    public void CloseGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
