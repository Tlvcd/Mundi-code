using UnityEngine;

public class AssetManager : MonoBehaviour
{
    #region singleton
    private static AssetManager instance;
    public static AssetManager I => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion


    [SerializeField]
    DamageDisplay damageIndicator;
    public DamageDisplay DamageIndicator => damageIndicator;
}
