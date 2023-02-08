using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "DropTable", order = 993)]
    public class DropTable : SerializedScriptableObject
    {
        /// <summary>
        /// Random range for amount of drops from table dropped
        /// </summary>
        public Vector2Int DropRange;
        public Dictionary<Drop, float> Drops;

        public Drop GetDrop()
        {
            if(Drops.Count < 1) { return default(Drop); }

            float roll = Random.Range(0, 100f);

            Drop droppedItem = default(Drop);
            foreach(Drop drop in Drops.Keys)
            {
                if (roll <= Drops[drop])
                {
                    droppedItem = drop;
                    continue;
                }            
                return droppedItem;
            }

            return default(Drop);
        }
    }

    [System.Serializable]
    public struct Drop
    {
        /// <summary>
        /// Random range for amount of specified item dropped
        /// </summary>
        public Vector2Int AmountRange;
        public EnumCollections.ItemType ItemType;
        [ShowIf("ItemType", EnumCollections.ItemType.Material)] public Item Item;
        [ShowIf("ItemType", EnumCollections.ItemType.ShipComponent)] public Equipment Equipment;
        [ShowIf("ItemType", EnumCollections.ItemType.Weapon)] public Weapon Weapon;

    }
}
