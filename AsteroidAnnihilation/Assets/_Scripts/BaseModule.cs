using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BaseModule : ScriptableObject
    {
        public int ID;
        public string StatName;
        public Stat stat;
        public int PointsSpent;
        public float PointValue;
        public int PointCost;
        public int MaxPoints;

        public ModuleData GetModuleData()
        {
            ModuleData data = new ModuleData();
            data.ID = ID;
            //data.Weapon = Weapon;
            data.StatName = StatName;
            data.stat = stat;
            data.PointsSpent = PointsSpent;
            data.PointValue = PointValue;
            data.PointCost = PointCost;
            data.MaxPoints = MaxPoints;

            return data;
        }
    }

    [System.Serializable]
    public class ModuleData
    {
        public int ID;
        //public Weapon Weapon;
        public string StatName;
        public Stat stat;
        public int PointsSpent;
        public float PointValue;
        public int PointCost;
        public int MaxPoints;

        public float GetStatValue()
        {
            return stat.GetBaseValue() + (PointsSpent * PointValue);
        }
    }
}