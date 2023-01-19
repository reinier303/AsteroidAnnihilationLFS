using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class StatManager : MonoBehaviour
    {
        public static StatManager Instance;

        private EquipmentManager equipmentManager;
        private PlayerStats playerStats;

        private Dictionary<EnumCollections.Stats, float> finalStats;

        public delegate void OffensiveStatsChanged();
        public OffensiveStatsChanged OnOffensiveStatsChanged;
        public delegate void DefensiveStatsChanged();
        public DefensiveStatsChanged OnDefensiveStatsChanged;
        public delegate void MovementStatsChanged();
        public MovementStatsChanged OnMovementStatsChanged;

        private bool initialized;

        private void Awake()
        {
            Instance = this;
            OnOffensiveStatsChanged += CalculateOffensiveStats;
            OnDefensiveStatsChanged += CalculateDefensiveStats;
            OnMovementStatsChanged += CalculateMovementStats;
        }

        private void CalculateOffensiveStats()
        {
            if (!initialized) { Initialize(); }

            float energyCapacity = playerStats.GetStatValue(EnumCollections.PlayerStats.BaseEnergyCapacity);
            energyCapacity += playerStats.GetSkillsValue(EnumCollections.Stats.EnergyCapacity);
            energyCapacity += equipmentManager.GetGearStatValue(EnumCollections.ItemType.EnergyCore, EnumCollections.Stats.EnergyCapacity);
            AddStat(EnumCollections.Stats.EnergyCapacity, energyCapacity);

            float energyRegen = playerStats.GetStatValue(EnumCollections.PlayerStats.BaseEnergyRegen);
            energyRegen += playerStats.GetSkillsValue(EnumCollections.Stats.EnergyRegen);
            energyRegen += equipmentManager.GetGearStatValue(EnumCollections.ItemType.EnergyCore, EnumCollections.Stats.EnergyRegen);
            AddStat(EnumCollections.Stats.EnergyRegen, energyRegen);
        }

        private void CalculateDefensiveStats()
        {
            if (!initialized) { Initialize(); }

            float maxHealth = playerStats.GetStatValue(EnumCollections.PlayerStats.BaseHealth);
            maxHealth += playerStats.GetSkillsValue(EnumCollections.Stats.Health);
            maxHealth += equipmentManager.GetGearStatValue(EnumCollections.ItemType.HullPlating, EnumCollections.Stats.Health);
            AddStat(EnumCollections.Stats.Health, maxHealth);

            float healthRegen = playerStats.GetStatValue(EnumCollections.PlayerStats.BaseHealthRegen);
            healthRegen += playerStats.GetSkillsValue(EnumCollections.Stats.HealthRegen);
            healthRegen += equipmentManager.GetGearStatValue(EnumCollections.ItemType.HullPlating, EnumCollections.Stats.HealthRegen);
            AddStat(EnumCollections.Stats.HealthRegen, healthRegen);

            float resistance = playerStats.GetSkillsValue(EnumCollections.Stats.PhysicalResistance);
            resistance += equipmentManager.GetGearStatValue(EnumCollections.ItemType.HullPlating, EnumCollections.Stats.PhysicalResistance);
            AddStat(EnumCollections.Stats.PhysicalResistance, resistance);
        }

        private void CalculateMovementStats()
        {
            if (!initialized) { Initialize(); }

        }

        private void AddStat(EnumCollections.Stats stat, float value)
        {
            if (!finalStats.ContainsKey(stat)) { finalStats.Add(stat, value); }
            else { finalStats[stat] = value; }
        }

        public float GetStat(EnumCollections.Stats stat)
        {
            if (finalStats.ContainsKey(stat)) { return finalStats[stat]; }
            else { Debug.LogWarning("Stat not found"); return 0f; }
        }

        private void Initialize()
        {
            initialized = true;
            playerStats = Player.Instance.RPlayerStats;
            equipmentManager = EquipmentManager.Instance;
            finalStats = new Dictionary<EnumCollections.Stats, float>();
        }
    }
}

