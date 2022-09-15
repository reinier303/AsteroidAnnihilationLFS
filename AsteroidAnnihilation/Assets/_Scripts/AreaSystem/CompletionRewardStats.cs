using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class CompletionRewardStats : MonoBehaviour
    {
        public static CompletionRewardStats Instance;

        private Dictionary<string, Dictionary<string, float>> rewardedWeaponStats;
        private Dictionary<string, float> rewardedPlayerStats;

        private void Awake()
        {
            Instance = this;
            LoadRewardedStats();
        }

        private void Start()
        {
            GameManager.Instance.onEndGame += SaveCompletionRewards;
            GameManager.Instance.onChangeScene += SaveCompletionRewards;
        }

        private void LoadRewardedStats()
        {
            if (ES3.KeyExists("RewardedWeaponStats"))
            {
                rewardedWeaponStats = (Dictionary<string, Dictionary<string, float>>)ES3.Load("RewardedWeaponStats");
                rewardedPlayerStats = (Dictionary<string, float>)ES3.Load("RewardedPlayerStats");
            }
            else
            {
                rewardedWeaponStats = new Dictionary<string, Dictionary<string, float>>();
                rewardedPlayerStats = new Dictionary<string, float>();
                SaveCompletionRewards();
            }
        }

        private void SaveCompletionRewards()
        {
            ES3.Save("RewardedWeaponStats", rewardedWeaponStats);
            ES3.Save("RewardedPlayerStats", rewardedPlayerStats);
        }

        public float GetRewardedStat(string id, string weapon)
        {
            if (weapon != "" && rewardedWeaponStats.ContainsKey(weapon) && rewardedWeaponStats[weapon].ContainsKey(id))
            {
                return rewardedWeaponStats[weapon][id];
            }
            else if (rewardedPlayerStats.ContainsKey(id)) 
            { 
                return rewardedPlayerStats[id]; 
            } else { 
                return 0;}
        }

        public void AddRewardedStat(List<StatCompletionReward> completionRewards)
        {
            foreach(StatCompletionReward reward in completionRewards)
            {
                float rewardValue = reward.RewardAmount;

                if (reward.IsWeapon)
                {
                    string weapon = reward.WeaponType.ToString();
                    string weaponStat = reward.WeaponStatType.ToString();
              
                    if (rewardedWeaponStats.ContainsKey(weapon))
                    {
                        if(rewardedWeaponStats[weapon].ContainsKey(weaponStat))
                        {
                            rewardedWeaponStats[weapon][weaponStat] += rewardValue;
                        }
                        else { rewardedWeaponStats[weapon].Add(weaponStat, rewardValue); }
                    }
                    else
                    {
                        rewardedWeaponStats.Add(weapon, new Dictionary<string, float>());
                        rewardedWeaponStats[weapon].Add(weaponStat, rewardValue);
                    }
                }
                else
                {
                    string playerStat = reward.PlayerStatType.ToString();
                    if (rewardedPlayerStats.ContainsKey(playerStat))
                    {
                        rewardedPlayerStats[playerStat] += rewardValue;
                    }
                    else { rewardedPlayerStats.Add(playerStat, rewardValue); }
                }
            }

        }
    }
}
