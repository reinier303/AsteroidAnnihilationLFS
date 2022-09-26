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
        public Dictionary<EnumCollections.Rarities, Material> RarityMaterials;

        public WeaponData startWeapon;
        public Dictionary<EnumCollections.ItemType, EquipmentData> startGear;

        public int GetModAmount(EnumCollections.Rarities rarity)
        {
            return Random.Range(RarityModAmountRange[rarity].x, RarityModAmountRange[rarity].y);
        }

        public Material GetRarityMaterial(EnumCollections.Rarities rarity)
        {
            return RarityMaterials[rarity];
        }
    }
}
