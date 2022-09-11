using UnityEngine;

public class RegionSelectionScript : MonoBehaviour
{
    [SerializeField] string regionName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        MusicManager.instance.SwitchRegion(regionName);
        Debug.Log("new region");
    }
}
