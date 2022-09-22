using UnityEngine;

namespace AsteroidAnnihilation
{
    [System.Serializable]
    public struct UpgradableStat
    {
        public string StatName;
        public float Value;
        public float Multiplier;
        public bool IsWeapon;
        public int Level;
        public bool Unlocked;
        public bool HasNegative;
        public bool Upgradable;

        public EnumCollections.Weapons WeaponType;
    }

    public static class StatHelper
    {
        public static float GetValue(UpgradableStat stat)
        {
            return stat.Value * stat.Multiplier;
        }
    }
}