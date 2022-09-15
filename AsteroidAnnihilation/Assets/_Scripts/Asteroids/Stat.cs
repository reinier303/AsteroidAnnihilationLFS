using UnityEngine;

namespace AsteroidAnnihilation
{
    [System.Serializable]
    public class Stat
    {
        public string StatName;

        public float Value = 1;

        public float Multiplier = 1;

        public bool IsWeapon;

        public Stat(string statName, float value = 1, float multiplier = 1)
        {
            StatName = statName;
            Value = value;
            Multiplier = multiplier;
        }

        public float GetBaseValue()
        {
            return Value * Multiplier;
        }
    }
}