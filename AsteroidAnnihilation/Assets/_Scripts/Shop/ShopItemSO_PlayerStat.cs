using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation 
{
    [CreateAssetMenu(menuName = "Shop/Upgrade Player Stat", order = 999)]
    public class ShopItemSO_PlayerStat : ShopItemSO
    {
        public EnumCollections.PlayerStats Stat;

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
            playerStats.AddToStat(Stat, UpgradeAmount);
        }
    }
}