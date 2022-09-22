using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/General Item Settings", order = 1002)]
    public class GeneralItemSettings : SerializedScriptableObject
    {
        public Dictionary<EnumCollections.Rarities, Vector2Int> RarityModAmountRange;

        public int GetModAmount(EnumCollections.Rarities rarity)
        {
            return Random.Range(RarityModAmountRange[rarity].x, RarityModAmountRange[rarity].y);
        }
    }
}
