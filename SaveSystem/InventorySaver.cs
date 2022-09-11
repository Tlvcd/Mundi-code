using Axis.Abstractions;
using Axis.Items;
using UnityEngine;
[RequireComponent(typeof(Saveable)),DisallowMultipleComponent]
public class InventorySaver : MonoBehaviour, ISaveable
{
    [SerializeField]
    InventoryAsset inventory;

    public void RestoreState(object obj)
    {
        var obiekt = JsonUtility.FromJson<Ekwipunek>(((SaveData)obj).data);
        inventory.LoadInventory(obiekt);
    }

    public object SaveState()
    {
        return new SaveData(JsonUtility.ToJson(inventory.eq));
    }

    [System.Serializable]
    private struct SaveData
    {
        public string data;
        public SaveData(string obj)
        {
            data = obj;
        }
    }
}
