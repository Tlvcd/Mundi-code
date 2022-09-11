using UnityEngine;


[CreateAssetMenu(menuName ="Axis Mundi/Damage Type")]
public class DamageType : ScriptableObject
{
    [SerializeField]
    private string damageTypeName;
    public string TypeName => damageTypeName;

    [SerializeField]
    private Color damageTypeColor;
    public Color TypeColor => damageTypeColor;

    [SerializeField]
    private Sprite damageTypeIcon;
    public Sprite TypeIcon => damageTypeIcon;

}
