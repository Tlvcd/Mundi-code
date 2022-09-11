using UnityEngine;

public class AwakeProgressLoader : MonoBehaviour
{
    [SerializeField]
    SaveManager manager;

    private void Start()
    {
        manager.RestoreState();
        manager.SaveGame();
    }

}
