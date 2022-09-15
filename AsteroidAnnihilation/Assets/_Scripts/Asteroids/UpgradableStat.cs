using UnityEngine;

namespace AsteroidAnnihilation
{
    [System.Serializable]
    public class UpgradableStat: Stat
    {
        public int Level;

        public bool Unlocked = true;

        public bool HasNegative;

        public bool Upgradable = true;

        public EnumCollections.Weapons WeaponType;

        public UpgradableStat(string statName, bool unlocked, bool upgradable, float value = 1, float multiplier = 1, EnumCollections.Weapons weaponType = EnumCollections.Weapons.None) : base(statName, value, multiplier)
        {
            Level = 0;
            StatName = statName;
            Value = value;
            Multiplier = multiplier;
            Unlocked = unlocked;
            Upgradable = upgradable;
            WeaponType = weaponType;
        }

        public float GetValue(CompletionRewardStats completionStats)
        {
            float value = GetBaseValue() + completionStats.GetRewardedStat(StatName, WeaponType.ToString());
            Debug.Log(GetBaseValue() + StatName + value);
            return value;
        }
    }
}