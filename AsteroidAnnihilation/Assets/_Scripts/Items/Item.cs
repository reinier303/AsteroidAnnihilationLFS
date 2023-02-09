using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Item", order = 992)]
    public class Item : SerializedScriptableObject
    {
        public int ID;
        public int Tier;
        public string ItemName;
        public List<Sprite> Icons;
        public Dictionary<EnumCollections.Rarities, float> WeightedRarity;
        public EnumCollections.ItemType ItemType;

        public EnumCollections.Rarities GetRarity()
        {
            EnumCollections.Rarities finalRarity = EnumCollections.Rarities.Common;
            foreach (EnumCollections.Rarities rarity in WeightedRarity.Keys)
            {
                if(Random.Range(0f,100f) < WeightedRarity[rarity])
                {
                    finalRarity = rarity;
                }
            }
            return finalRarity;
        }

        public Sprite GetIcon()
        {
            return Icons[Random.Range(0, Icons.Count)];
        }
    }
}