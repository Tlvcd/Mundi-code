using System;
using System.Collections;
using System.Collections.Generic;
using Axis.Items;
using UnityEngine;


namespace Axis.Items
{
    [Serializable]
    public class Equipable : Item
    {
        [field: SerializeField]
        public Statistics statTable { get; protected set; }

        public Statistics GetItemStats() => statTable;
    }

}

