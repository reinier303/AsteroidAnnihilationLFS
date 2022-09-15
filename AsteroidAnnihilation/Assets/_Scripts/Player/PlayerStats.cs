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


        }

        private void Start()
        {
            uIManager = UIManager.Instance;
            GameManager.Instance.onEndGame += SavePlayerStats;
            GameManager.Instance.onChangeScene += SavePlayerStats;

        }

        private void NewSave()
        {
            Stats = new Dictionary<string, UpgradableStat>();
            string health = EnumCollections.PlayerStats.Health.ToString();
            string movementSpeed = EnumCollections.PlayerStats.MovementSpeed.ToString();
            string magnetRadius = EnumCollections.PlayerStats.MagnetRadius.ToString();
            string critRate = EnumCollections.PlayerStats.CritRate.ToString();
            string critMultiplier = EnumCollections.PlayerStats.CritMultiplier.ToString();
            string powerupChance = EnumCollections.PlayerStats.PowerUpChance.ToString();

            //Player Stats
            Stats.Add(health, new UpgradableStat(health, true, true, 100, 1));
            Stats.Add(movementSpeed, new UpgradableStat(movementSpeed, true, true, 3.5f , 1f));
            Stats.Add(magnetRadius, new UpgradableStat(magnetRadius, false, true, 3, 1));
            Stats.Add(critRate, new UpgradableStat(critRate, false, true, 5.0f, 1));
            Stats.Add(critMultiplier, new UpgradableStat(critMultiplier, false, true, 2.0f, 1));
            Stats.Add("Units", new UpgradableStat("Units", false, false, 0));
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
        public List<UpgradableStat> StatList;
    }

}
