using Axis.Items;
using UnityEngine;

[CreateAssetMenu(fileName ="new Talent", menuName ="Axis Mundi/Talent")]
public class Talent : ScriptableObject
{
    [field: SerializeField]
    public Statistics Stat;
    [SerializeField]
    private bool givesStats = true;


    [field: SerializeField]
    public Item Reward;
    [SerializeField]
    private bool givesItem = false;

    [field: SerializeField]
    public uint price { get; private set; }

}
