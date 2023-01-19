using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Completion Rewards", order = 1000)]
    public class StatCompletionRewardSettings : SerializedScriptableObject
    {
        /// <summary>
        /// Divide into weapon types to incentivise use of specific weapons by making a selection
        /// </summary>
        public List<List<StatCompletionReward>> WeaponStatRewards;

        public List<StatCompletionReward> PlayerStatRewards;

        public List<EnumCollections.Unlocks> Unlocks;

        public List<StatCompletionReward> GetCompletionRewardPool(int weaponAmount)
        {
            List<StatCompletionReward> preWeightRewards = new List<StatCompletionReward>();

            //We use this to make sure we dont remove from the actual settings
            List<List<StatCompletionReward>> WeaponStatRewardsInstance = new List<List<StatCompletionReward>>();
            WeaponStatRewardsInstance.AddRange(WeaponStatRewards);

            //TODO:: Make this not add if weapon is not unlocked
            for (int i = 0; i < weaponAmount; i ++)
            {
                int weaponIndex = Random.Range(0, WeaponStatRewardsInstance.Count);
                preWeightRewards.AddRange(WeaponStatRewardsInstance[weaponIndex]);
                WeaponStatRewardsInstance.RemoveAt(weaponIndex);
            }
            preWeightRewards.AddRange(PlayerStatRewards);

            List<StatCompletionReward> actualRewards = new List<StatCompletionReward>();
            foreach(StatCompletionReward reward in preWeightRewards)
            {
                for(int i = 0; i < reward.Weight; i++)
                {
                    actualRewards.Add(reward);
                }
            }
            
            return actualRewards;
        }
    }

    [System.Serializable]
    public class StatCompletionReward
    {
        public bool IsWeapon;
        [ShowIf("IsWeapon")] public EnumCollections.Weapons WeaponType;
        [ShowIf("IsWeapon")] public EnumCollections.Stats WeaponStatType;
        [HideIf("IsWeapon")] public EnumCollections.PlayerStats PlayerStatType;
        public float RewardAmount;
        public int Weight = 1;
    }
}
