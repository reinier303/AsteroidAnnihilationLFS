using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [System.Serializable]
    public class Equipment : Item
    { 
        [SerializeField] public Dictionary<EnumCollections.WeaponStats, Vector2> EquipmentStatRanges;
        [SerializeField] protected Dictionary<EnumCollections.WeaponStats, Vector2> RarityStatRanges;
            
        [System.NonSerialized] private Dictionary<EnumCollections.WeaponStats, float> EquipmentStats; 
        [System.NonSerialized] private Dictionary<EnumCollections.WeaponStats, float> RarityStats;

        protected PlayerStats playerStats;

        public virtual void Initialize(PlayerStats pStats, Dictionary<EnumCollections.WeaponStats, float> weaponStats, Dictionary<EnumCollections.WeaponStats, float> rarityStats)
        {
            playerStats = pStats;
            EquipmentStats = weaponStats;
            RarityStats = rarityStats;
            //TODO::Save Weapons
            //GameManager.Instance.onEndGame += SaveWeapon;
            //completionRewardStats = CompletionRewardStats.Instance;
        }

        public float GetEquipmentStat(EnumCollections.WeaponStats stat)
        {
            if (RarityStats.ContainsKey(stat))
            {
                return EquipmentStats[stat] + RarityStats[stat];
            } else { return EquipmentStats[stat];}
        }

        public Dictionary<EnumCollections.WeaponStats, float> GetEquipmentStats()
        {
            return EquipmentStats;
        }

        protected virtual void SaveEquipment()
        {
            ES3.Save(BaseEquipmentName + "Data", EquipmentStats);
        }

        public string GenerateName()
        {
            int randomAlphabet = Random.Range(0, 26);
            char randomLetter = (char)('a' + randomAlphabet);
            string randomString = randomLetter.ToString();
            return BaseEquipmentName + " Model-" + randomString;
        }

        public Dictionary<EnumCollections.WeaponStats, float> GetRarityStats(EnumCollections.Rarities rarity, GeneralItemSettings settings)
        {
            //Common scenario
            Dictionary<EnumCollections.WeaponStats, float> rarityStats = new Dictionary<EnumCollections.WeaponStats, float>();
            if (rarity == EnumCollections.Rarities.Common) { return rarityStats; }

            //Has Rarity > Common
            float modAmount = settings.GetModAmount(rarity);

            List<EnumCollections.WeaponStats> modList = Enumerable.ToList(RarityStatRanges.Keys);

            for (int i = 0; i < modAmount; i++)
            {
                EnumCollections.WeaponStats mod = modList[Random.Range(0, modList.Count)];
                float modValue = Random.Range(RarityStatRanges[mod].x, RarityStatRanges[mod].y);
                modValue = MathHelpers.RoundToDecimal(modValue, 2);
                if (rarityStats.ContainsKey(mod))
                {
                    rarityStats[mod] += modValue;
                }
                else { rarityStats.Add(mod, modValue); }            
            }

            return rarityStats;
        }
    }
}
