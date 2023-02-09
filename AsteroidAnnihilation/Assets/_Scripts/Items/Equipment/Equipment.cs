using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{   
     [CreateAssetMenu(menuName = "Equipment", order = 997)]
    [System.Serializable]public class Equipment : Item
    { 
        [SerializeField] public Dictionary<EnumCollections.Stats, Vector2> EquipmentStatRanges;
        [SerializeField] protected Dictionary<EnumCollections.Stats, Vector2> RarityStatRanges;
        protected PlayerStats playerStats;
        protected EquipmentManager equipmentManager;
        public string BaseEquipmentName;

        public virtual void Initialize(PlayerStats pStats, EquipmentManager equipmentManager)
        {
            playerStats = pStats;
            this.equipmentManager = equipmentManager;
        }

        public float GetEquipmentStat(EnumCollections.Stats stat, int index)
        {
            EquipmentData data = equipmentManager.GetEquipedWeapon(index).EquipmentData;

            if (data.RarityStats.ContainsKey(stat))
            {
                return data.EquipmentStats[stat] + data.RarityStats[stat];
            } else { return data.EquipmentStats[stat];}
        }

        protected virtual void SaveEquipment()
        {
            //ES3.Save(BaseEquipmentName + "Data", EquipmentStats);
        }

        public string GenerateName()
        {
            int randomAlphabet = Random.Range(0, 26);
            char randomLetter = (char)('a' + randomAlphabet);
            string randomString = randomLetter.ToString();
            return BaseEquipmentName + " Model-" + randomString;
        }

        public Dictionary<EnumCollections.Stats, float> GetRarityStats(EnumCollections.Rarities rarity, GeneralItemSettings settings)
        {
            //Common scenario
            Dictionary<EnumCollections.Stats, float> rarityStats = new Dictionary<EnumCollections.Stats, float>();
            if (rarity == EnumCollections.Rarities.Common) { return rarityStats; }

            //Has Rarity > Common
            float modAmount = settings.GetModAmount(rarity);

            List<EnumCollections.Stats> modList = Enumerable.ToList(RarityStatRanges.Keys);

            for (int i = 0; i < modAmount; i++)
            {
                EnumCollections.Stats mod = modList[Random.Range(0, modList.Count)];
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
