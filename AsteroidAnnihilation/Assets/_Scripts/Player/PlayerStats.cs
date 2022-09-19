using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerStats : MonoBehaviour
    {
        [System.NonSerialized] public Dictionary<string, UpgradableStat> Stats;

        private UIManager uIManager;

        [SerializeField] private PlayerData playerData;

        public PlayerLevelSettings playerLevelSettings;

        private void Awake()
        {
            //Debug.Log("Why are we out of the if statement!!?!??");
            if (!SaveLoad.SaveExists("PlayerData"))
            {
                Debug.Log("NewSave");
                NewSave();
            }
            else
            {
                Stats = GetStatDictionaryFromList(SaveLoad.Load<PlayerData>("PlayerData").StatList);
            }

            playerLevelSettings = (PlayerLevelSettings)Resources.Load("Settings/PlayerLevelSettings");
            
        }

        private void Start()
        {
            uIManager = UIManager.Instance;
            GameManager.Instance.onEndGame += SavePlayerStats;
            GameManager.Instance.onChangeScene += SavePlayerStats;

        }

        private void NewSave()
        {
            Debug.Log("New Save");
            Stats = new Dictionary<string, UpgradableStat>();
            string health = EnumCollections.PlayerStats.Health.ToString();
            string movementSpeed = EnumCollections.PlayerStats.MovementSpeed.ToString();
            string magnetRadius = EnumCollections.PlayerStats.MagnetRadius.ToString();
            string critRate = EnumCollections.PlayerStats.CritRate.ToString();
            string critMultiplier = EnumCollections.PlayerStats.CritMultiplier.ToString();
            string powerupChance = EnumCollections.PlayerStats.PowerUpChance.ToString();

            //Player Stats
            playerData.PlayerLevel = 1;
            Debug.Log(playerData.PlayerLevel = 1);

            Stats.Add(health, new UpgradableStat(health, true, true, 100, 1));
            Stats.Add(movementSpeed, new UpgradableStat(movementSpeed, true, true, 3.5f , 1f));
            Stats.Add(magnetRadius, new UpgradableStat(magnetRadius, false, true, 3, 1));
            Stats.Add(critRate, new UpgradableStat(critRate, false, true, 5.0f, 1));
            Stats.Add(critMultiplier, new UpgradableStat(critMultiplier, false, true, 2.0f, 1));
            Stats.Add("Units", new UpgradableStat("Units", false, false, 0));
            Stats.Add("Experience", new UpgradableStat("Experience", false, false, 0));
            Stats.Add(powerupChance, new UpgradableStat(powerupChance, false, false, 0));

            //Save to file
            SavePlayerStats();
        }

        public void SaveStatDictionaryToList()
        {
            playerData.StatList = new List<UpgradableStat>();

            foreach (UpgradableStat stat in Stats.Values)
            {
                playerData.StatList.Add(stat);
            }
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
            SaveStatDictionaryToList();

            //Save file with data on computer
            SaveLoad.Save(playerData, "PlayerData");
        }

        public void AddToUnits(float value)
        {
            Stats["Units"].Value += Mathf.Clamp(value, 0, Mathf.Infinity);
            uIManager.UpdateUnits();
        }

        public void AddToExperience(float value)
        {
            //TODO::Fix overlevel exp
            Stats["Experience"].Value += Mathf.Clamp(value, 0, Mathf.Infinity);
            //Make UI Experience update
            //uIManager.UpdateExperience();
            CheckLevelUp(value);
            uIManager.UpdateExperience();
        }

        public int GetPlayerLevel()
        {
            return playerData.PlayerLevel;
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
            return Stats["Experience"].Value - playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp;
        }

        /// <summary>
        /// Return experience needed for next level.
        /// </summary>
        /// <returns></returns>
        public float GetExperienceToLevel()
        {
            return playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp - Stats["Experience"].Value;
        }

        /// <summary>
        /// Returns total experience needed for next level.
        /// </summary>
        /// <returns></returns>
        public float GetTotalExpNeeded()
        {
            return playerLevelSettings.PlayerLevels[GetPlayerLevel()].TotalExp;
        }

        private void CheckLevelUp(float restExp)
        {
            if(Stats["Experience"].Value > playerLevelSettings.PlayerLevels[playerData.PlayerLevel - 1].TotalExp)
            {
                playerData.PlayerLevel++;
                uIManager.UpdateLevel();
            }
        }

        public bool TryPlayerBuy(float cost)
        {
            if(Stats["Units"].GetBaseValue() >= cost)
            {
                Stats["Units"].Value -= cost;
                uIManager.UpdateUnits();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [System.Serializable]
    public struct PlayerData
    {
        public int PlayerLevel;
        public List<UpgradableStat> StatList;
    }

}
