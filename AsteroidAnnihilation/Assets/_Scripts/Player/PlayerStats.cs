using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerStats : MonoBehaviour
    {
        private UIManager uIManager;

        private int PlayerLevel;
        private Dictionary<EnumCollections.PlayerStats, float> Stats;
        private Dictionary<string, float> statMultipliers;
        public PlayerLevelSettings playerLevelSettings;

        private void Awake()
        {
            if (!ES3.KeyExists("playerData"))
            {
                Debug.Log("NewSave");
                NewSave();
            }
            else
            {
                // Lookie here for solution
                //
                //Change Upgradable Stat to struct since classes cant be saved with es3
                //
                //
                Stats = ES3.Load<Dictionary<EnumCollections.PlayerStats, float>>("playerData");
                PlayerLevel = ES3.Load<int>("playerLevel");
            }

        }

        private void Start()
        {
            playerLevelSettings = SettingsManager.Instance.playerLevelSettings;
            uIManager = UIManager.Instance;
            GameManager.Instance.onEndGame += SavePlayerStats;
            GameManager.Instance.onChangeScene += SavePlayerStats;

        }

        private void NewSave()
        {
            Stats = new Dictionary<EnumCollections.PlayerStats, float>();

            //Player Stats
            PlayerLevel = 1;

            Stats.Add(EnumCollections.PlayerStats.Health, 100);
            Stats.Add(EnumCollections.PlayerStats.MovementSpeed, 4.0f);
            Stats.Add(EnumCollections.PlayerStats.MagnetRadius, 3);
            Stats.Add(EnumCollections.PlayerStats.CritRate, 5.0f);
            Stats.Add(EnumCollections.PlayerStats.CritMultiplier, 2.0f);
            Stats.Add(EnumCollections.PlayerStats.CurrentUnits, 0);
            Stats.Add(EnumCollections.PlayerStats.CurrentExperience, 0);
            Stats.Add(EnumCollections.PlayerStats.PowerUpChance, 0);

            //Save to file
            SavePlayerStats();
        }

        public Dictionary<string, UpgradableStat> GetStatDictionaryFromList(List<UpgradableStat> statList)
        {
            Dictionary<string, UpgradableStat> StatDictionary = new Dictionary<string, UpgradableStat>();

            foreach(UpgradableStat stat in statList)
            {
                StatDictionary.Add(stat.StatName, stat);
            }

            return StatDictionary;
        }

        public void SavePlayerStats()
        {
            //Add dictionary data to serializable struct for saving
            //SaveStatDictionaryToList();

            //Save file with data on computer
            //SaveLoad.Save(playerData, "PlayerData");
            ES3.Save("playerData", Stats);
            ES3.Save("playerLevel", PlayerLevel);

        }

        public void AddToUnits(float value)
        {
            Stats[EnumCollections.PlayerStats.CurrentUnits] += Mathf.Clamp(value, 0, Mathf.Infinity);

            uIManager.UpdateUnits();
        }

        public void AddToExperience(float value)
        {
            //TODO::Fix overlevel exp
            Stats[EnumCollections.PlayerStats.CurrentExperience] += Mathf.Clamp(value, 0, Mathf.Infinity);
            CheckLevelUp(value);
            uIManager.UpdateExperience();
        }

        private void CheckLevelUp(float restExp)
        {
            if (Stats[EnumCollections.PlayerStats.CurrentExperience] > GetTotalExpNeeded())
            {
                PlayerLevel++;
                uIManager.UpdateLevel();
            }
        }

        public int GetPlayerLevel()
        {
            return PlayerLevel;
        }

        /// <summary>
        /// Returns experience needed from current level to next level.
        /// </summary>
        /// <returns></returns>
        public float GetExperienceDifference()
        {
            if(GetPlayerLevel() <= 1)
            {
                return playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp;
            }
            return playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp - playerLevelSettings.PlayerLevels[GetPlayerLevel() - 1].TotalExp;
        }
        
        /// <summary>
        /// Returns experience progress towards next level. Used in expBar currentValue
        /// </summary>
        /// <returns></returns>
        public float GetExperienceLevelProgress()
        {
            if(GetPlayerLevel() <= 1) { return Stats[EnumCollections.PlayerStats.CurrentExperience]; }
            else { return Stats[EnumCollections.PlayerStats.CurrentExperience] - playerLevelSettings.PlayerLevels[GetPlayerLevel() - 1].TotalExp; }
            
        }

        /// <summary>
        /// Return experience needed for next level.
        /// </summary>
        /// <returns></returns>
        public float GetExperienceToLevel()
        {
            return playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp - Stats[EnumCollections.PlayerStats.CurrentExperience];
        }

        /// <summary>
        /// Returns total experience needed for next level.
        /// </summary>
        /// <returns></returns>
        public float GetTotalExpNeeded()
        {
            return playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp;
        }

        public bool TryPlayerBuy(float cost)
        {
            if(Stats[EnumCollections.PlayerStats.CurrentUnits] >= cost)
            {
                Stats[EnumCollections.PlayerStats.CurrentUnits] -= cost;
                uIManager.UpdateUnits();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasStat(EnumCollections.PlayerStats stat)
        {
            return Stats.ContainsKey(stat);
        }

        public float GetStatValue(EnumCollections.PlayerStats stat)
        {
            if (Stats.ContainsKey(stat)) { return Stats[stat]; }
            else 
            {
                Debug.LogWarning("Stat " + stat.ToString() + " was not found");
                return 0; 
            }           
        }

        /// <summary>
        /// Add value to stat
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="value"></param>
        public void AddToStat(EnumCollections.PlayerStats stat, float value)
        {
            if (Stats.ContainsKey(stat)) { Stats[stat] += value; }
            else { Debug.LogWarning("Stat not found"); }
        }

        /// <summary>
        /// Add stat to stat dictionary
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="startValue"></param>
        public void AddStat(EnumCollections.PlayerStats stat, float startValue)
        {
            if (!Stats.ContainsKey(stat)) { Stats.Add(stat, startValue); }
            else { Debug.LogWarning("Stat not found"); }
        }
    }
}
