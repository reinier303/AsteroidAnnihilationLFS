using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation 
{
    [CreateAssetMenu(menuName = "Shop/Upgrade Player Stat", order = 999)]
    public class ShopItemSO_PlayerStat : ShopItemSO
    {
        public string StatName;

        public float UpgradeAmount;

        private PlayerStats playerStats;

        public void GetPlayerStats()
        {
            playerStats = GameManager.Instance.RPlayer.RPlayerStats;
        }

        public override void ItemBought()
        {
            base.ItemBought();
            if (playerStats == null)
            {
                GetPlayerStats();
            }
            playerStats.Stats[StatName].Value += UpgradeAmount;
        }
    }
}