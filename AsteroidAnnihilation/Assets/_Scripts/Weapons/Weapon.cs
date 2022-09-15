using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [System.Serializable]
    public class Weapon : ScriptableObject, IFire
    {
        public int ID;
        public string WeaponName;
        public string ProjectileName;
        public bool Unlocked;

        public List<UpgradableStat> WeaponStartValues;
        [System.NonSerialized] public Dictionary<string, UpgradableStat> WeaponStatDictionary;
        
        public int WeaponIndex;

        private UIManager uIManager;

        private WeaponData weaponData;

        private PlayerStats playerStats;
        private PowerupManager powerupManager;
        private JSONWeaponDatabank weaponDatabank;
        protected CompletionRewardStats completionRewardStats;

        public virtual void Initialize(PlayerStats pStats)
        {
            weaponDatabank = JSONWeaponDatabank.Instance;
            powerupManager = PowerupManager.Instance;

            //TODO:: Remove after JSON integration
            if (WeaponName != "Plasma Gun")
            {
                Unlocked = false;
            }
            if (!SaveLoad.SaveExists("WeaponData_" +  WeaponName))
            {
                NewSave();
            }
            else
            {
                LoadWeaponStats();
            }
            playerStats = pStats;
            GameManager.Instance.onEndGame += SaveWeaponStats;
            completionRewardStats = CompletionRewardStats.Instance;
        }

        private void NewSave()
        {
            WeaponStatDictionary = new Dictionary<string, UpgradableStat>();

            //Add data from list(filled in editor resources/weapon) to dictionary
            foreach (UpgradableStat stat in WeaponStartValues)
            {
                stat.Level = 0;
                stat.Value = weaponDatabank.LookUpStat(WeaponName, stat.StatName.ToString()).Values[stat.Level];
                WeaponStatDictionary.Add(stat.StatName.ToString(), stat);
            }

            //Save to file
            SaveWeaponStats();
        }

        public void SaveStatDictionaryToList()
        {
            weaponData.StatList = new List<UpgradableStat>();

            foreach (UpgradableStat stat in WeaponStatDictionary.Values)
            {
                weaponData.StatList.Add(stat);
            }
        }

        public Dictionary<string, UpgradableStat> GetStatDictionaryFromList(List<UpgradableStat> statList)
        {
            Dictionary<string, UpgradableStat> StatDictionary = new Dictionary<string, UpgradableStat>();

            foreach (UpgradableStat stat in statList)
            {
                StatDictionary.Add(stat.StatName, stat);
            }

            return StatDictionary;
        }

        public virtual void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity)
        {
            //This method is meant to be overridden.
        }

        public void SaveWeaponStats()
        {
            powerupManager.DisableAllPowerups();
            //Add dictionary data to serializable struct for saving
            SaveStatDictionaryToList();

            weaponData.Unlocked = Unlocked;

            //Save file with data on computer
            SaveLoad.Save(weaponData, "WeaponData_" + WeaponName);
        }

        private void LoadWeaponStats()
        {
            weaponData = SaveLoad.Load<WeaponData>("WeaponData_" + WeaponName);
            WeaponStatDictionary = GetStatDictionaryFromList(weaponData.StatList);
            Unlocked = weaponData.Unlocked;
        }

        protected bool IsCrit()
        {
            if (Random.Range(0f, 100) < playerStats.Stats["CritRate"].GetBaseValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public interface IFire
    {
        //Use Generic T for other optional parameters
        void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity);
    }

    [System.Serializable]
    public struct WeaponData
    {
        public bool Unlocked;
        public List<UpgradableStat> StatList;
    }
}
