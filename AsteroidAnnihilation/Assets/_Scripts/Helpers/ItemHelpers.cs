using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public static class ItemHelpers
    {
        public static EnumCollections.Rarities GetItemRarity(Dictionary<EnumCollections.Rarities, float> rarities)
        {
            EnumCollections.Rarities finalRarity = EnumCollections.Rarities.Common;
            foreach (EnumCollections.Rarities rarity in rarities.Keys)
            {
                if (Random.Range(0f, 100f) < rarities[rarity])
                {
                    finalRarity = rarity;
                }
            }
            return finalRarity;
        }
    }
}
