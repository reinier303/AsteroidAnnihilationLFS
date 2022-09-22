using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Shop/Player Unlock", order = 999)]
    public class ShopItemSO_PlayerUnlock : ShopItemSO
    {
        public EnumCollections.PlayerStats UnlockedStat;
        public float StartValue;

        private PlayerStats playerStats;

        public void GetPlayerStats()
        {
            playerStats = GameManager.Instance.RPlayer.RPlayerStats;
        }

        public override void ItemBought()
        {
            base.ItemBought();
            if(playerStats == null)
            {
                GetPlayerStats();
            }
            playerStats.AddStat(UnlockedStat, StartValue);
        }
    }

}
