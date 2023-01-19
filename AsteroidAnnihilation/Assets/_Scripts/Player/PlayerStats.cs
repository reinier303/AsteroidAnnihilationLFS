using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerStats : MonoBehaviour
    {
        private UIManager uIManager;
        private GameManager gameManager;
        private AudioManager audioManager;
        private CameraManager cameraManager;

        private int PlayerLevel;
        private Dictionary<EnumCollections.PlayerStats, float> Stats;
        private Dictionary<string, float> statMultipliers;
        public PlayerLevelSettings playerLevelSettings;
        private Dictionary<int, SkillNodeData> SkillTreeStats;
        [SerializeField] private ParticleSystem LevelUpEffect;

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
                SkillTreeStats = ES3.Load<Dictionary<int, SkillNodeData>>("skillTreeData");
                PlayerLevel = ES3.Load<int>("playerLevel", defaultValue: 1);
            }

        }

        private void Start()
        {
            playerLevelSettings = SettingsManager.Instance.playerLevelSettings;
            uIManager = UIManager.Instance;
            gameManager = GameManager.Instance;
            cameraManager = gameManager.RCameraManager;
            audioManager = AudioManager.Instance;
            gameManager.onEndGame += SavePlayerStats;
            gameManager.onChangeScene += SavePlayerStats;

        }

        private void NewSave()
        {
            Stats = new Dictionary<EnumCollections.PlayerStats, float>();
            SkillTreeStats = new Dictionary<int, SkillNodeData>();

            //Player Stats
            PlayerLevel = 1;

            Stats.Add(EnumCollections.PlayerStats.BaseHealth, 20); //+30 BaseHull
            Stats.Add(EnumCollections.PlayerStats.BaseHealthRegen, 0.1f);
            Stats.Add(EnumCollections.PlayerStats.BaseMovementSpeed, 1.0f); //+3 BaseEngine
            Stats.Add(EnumCollections.PlayerStats.BaseMagnetRadius, 3); // Magnet Accesories
            Stats.Add(EnumCollections.PlayerStats.CritRate, 5.0f); // Weapons
            Stats.Add(EnumCollections.PlayerStats.CritMultiplier, 2.0f); // Weapons
            Stats.Add(EnumCollections.PlayerStats.CurrentUnits, 0);
            Stats.Add(EnumCollections.PlayerStats.CurrentExperience, 0);
            Stats.Add(EnumCollections.PlayerStats.PowerUpChance, 0);
            Stats.Add(EnumCollections.PlayerStats.BaseEnergyCapacity, 0); //+50 BaseEnergyCore
            Stats.Add(EnumCollections.PlayerStats.BaseEnergyRegen, 0.3f);//+1 BaseEnergyCore
            Stats.Add(EnumCollections.PlayerStats.SkillPointsSpent, 0);
            Stats.Add(EnumCollections.PlayerStats.SkillPointsTotal, 0);

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
            ES3.Save("skillTreeData", SkillTreeStats);
            ES3.Save("playerLevel", PlayerLevel);

        }

        public void AddToUnits(float value)
        {
            Stats[EnumCollections.PlayerStats.CurrentUnits] += Mathf.Clamp(value, 0, Mathf.Infinity);

            uIManager.UpdateUnits();
        }

        public void AddToExperience(float value)
        {
            Debug.Log(value);
            Stats[EnumCollections.PlayerStats.CurrentExperience] += Mathf.Clamp(value, 0, Mathf.Infinity);
            CheckLevelUp(value);
            uIManager.UpdateExperience();
        }

        private void CheckLevelUp(float expGiven)
        {
            float overLevelExp = Mathf.Clamp(Stats[EnumCollections.PlayerStats.CurrentExperience] - GetTotalExpNeeded(), 0, Mathf.Infinity);
            if (Stats[EnumCollections.PlayerStats.CurrentExperience] > GetTotalExpNeeded())
            {
                if (PlayerLevel == 1) {
                    GameManager.Instance.PauseGame(false);
                    uIManager.EnableLevelTutorial();                    
                }
                cameraManager.StartCoroutine(cameraManager.Shake(1, 5));
                LevelUpEffect.Play();
                audioManager.PlayAudio("LevelUp");
                PlayerLevel++;
                Stats[EnumCollections.PlayerStats.SkillPointsTotal] += 2;
                uIManager.UpdateLevel();
                uIManager.OnLevelUp();
            }
            if(overLevelExp > 0) { AddToExperience(overLevelExp); }
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
                return GetTotalExpNeeded();
            }

            return GetTotalExpNeeded() - playerLevelSettings.PlayerLevels[GetPlayerLevel() - 2].TotalExp;
        }
        
        /// <summary>
        /// Returns experience progress towards next level. Used in expBar currentValue
        /// </summary>
        /// <returns></returns>
        public float GetExperienceLevelProgress()
        {
            if(GetPlayerLevel() <= 1) { return Stats[EnumCollections.PlayerStats.CurrentExperience]; }
            else { return Stats[EnumCollections.PlayerStats.CurrentExperience] - playerLevelSettings.PlayerLevels[GetPlayerLevel() - 2].TotalExp; }
            
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
            return playerLevelSettings.PlayerLevels[GetPlayerLevel() - 1].TotalExp;
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

        public bool TryUnlockSkill(float cost)
        {
            if (GetCurrentSkillPoints() >= cost)
            {
                Stats[EnumCollections.PlayerStats.SkillPointsSpent] += cost;
                uIManager.UpdateSkillPoints();
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

        public int GetCurrentSkillPoints()
        {
            return (int)Stats[EnumCollections.PlayerStats.SkillPointsTotal] - (int)Stats[EnumCollections.PlayerStats.SkillPointsSpent];
        }

        public void AddSkillUnlocked(int id, EnumCollections.Stats stat, float value)
        {
            SkillNodeData skillData = new SkillNodeData();
            skillData.Stat = stat;
            skillData.Value = value;
            SkillTreeStats.Add(id, skillData);
        }

        public bool HasSkillNode(int id)
        {
            return SkillTreeStats.ContainsKey(id);
        }

        public float GetSkillsValue(EnumCollections.Stats stat)
        {
            float value = 0;
            foreach(SkillNodeData data in SkillTreeStats.Values)
            {
                if(data.Stat == stat) { value += data.Value;}
            }
            return value;
        }
    }

    public struct SkillNodeData
    {
        public EnumCollections.Stats Stat;
        public float Value;
    }
}
