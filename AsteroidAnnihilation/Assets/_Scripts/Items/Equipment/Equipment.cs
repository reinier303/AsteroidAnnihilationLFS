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
        [SerializeField] public Dictionary<EnumCollections.EquipmentStats, Vector2> EquipmentStatRanges;
        [SerializeField] protected Dictionary<EnumCollections.EquipmentStats, Vector2> RarityStatRanges;
            
        [System.NonSerialized] private Dictionary<EnumCollections.EquipmentStats, float> EquipmentStats; 
        [System.NonSerialized] private Dictionary<EnumCollections.EquipmentStats, float> RarityStats;

        protected PlayerStats playerStats;

        public virtual void Initialize(PlayerStats pStats, Dictionary<EnumCollections.EquipmentStats, float> weaponStats, Dictionary<EnumCollections.EquipmentStats, float> rarityStats)
        {
            playerStats = pStats;
            EquipmentStats = weaponStats;
            RarityStats = rarityStats;
            //TODO::Save Weapons
            //GameManager.Instance.onEndGame += SaveWeapon;
            //completionRewardStats = CompletionRewardStats.Instance;
        }

        public float GetEquipmentStat(EnumCollections.EquipmentStats stat)
        {
            if (RarityStats.ContainsKey(stat))
            {
                return EquipmentStats[stat] + RarityStats[stat];
            } else { return EquipmentStats[stat];}
        }

        public Dictionary<EnumCollections.EquipmentStats, float> GetEquipmentStats()
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

        public Dictionary<EnumCollections.EquipmentStats, float> GetRarityStats(EnumCollections.Rarities rarity, GeneralItemSettings settings)
        {
            //Common scenario
            Dictionary<EnumCollections.EquipmentStats, float> rarityStats = new Dictionary<EnumCollections.EquipmentStats, float>();
            if (rarity == EnumCollections.Rarities.Common) { return rarityStats; }

            //Has Rarity > Common
            float modAmount = settings.GetModAmount(rarity);

            List<EnumCollections.EquipmentStats> modList = Enumerable.ToList(RarityStatRanges.Keys);

            for (int i = 0; i < modAmount; i++)
            {
                EnumCollections.EquipmentStats mod = modList[Random.Range(0, modList.Count)];
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
