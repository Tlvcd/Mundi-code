using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Axis.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Axis Mundi/Items/Create New Item")]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private string itemName;
        public string ItemName => itemName;


        [SerializeField]
        private Sprite icon;
        public Sprite Sprite => icon;

        [SerializeField,TextArea(5,10),ResizableTextArea]
        private string description;
        public string Description => description;

        [SerializeField]
        private RarityTypes rarity;
        public RarityTypes GetRarity() => rarity;


        public Color32 GetColorByRarity() => Rarity.RarityColorPairs[rarity];
        public string GetNameByRarity() => Rarity.RarityNamePairs[rarity];
    }

    public enum RarityTypes
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Relic
    }
    
    public static class Rarity //bruh moglo byc SO zamiast tego
    {
        public static readonly Dictionary<RarityTypes, Color32> RarityColorPairs =
            new Dictionary<RarityTypes, Color32> { 
                { RarityTypes.Common,  new Color32(128, 128, 128, 255) },
                { RarityTypes.Rare,  new Color32(0, 128, 255, 255) },
                { RarityTypes.Epic,  new Color32(128, 0, 255, 255) },
                { RarityTypes.Legendary,  new Color32(255, 165, 0, 255) },
                { RarityTypes.Relic,  new Color32(255, 0, 0, 255) }
            };
        public static readonly Dictionary<RarityTypes, string> RarityNamePairs =
            new Dictionary<RarityTypes, string>
            {
                {RarityTypes.Common, "Powszechny" },
                {RarityTypes.Rare, "Rzadki" },
                {RarityTypes.Epic, "Epicki" },
                {RarityTypes.Legendary, "Legendarny" },
                {RarityTypes.Relic, "Relikt" },
            };
    }

}
