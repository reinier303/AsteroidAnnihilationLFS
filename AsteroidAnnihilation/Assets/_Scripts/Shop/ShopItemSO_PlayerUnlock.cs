using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Shop/Player Unlock", order = 999)]
    public class ShopItemSO_PlayerUnlock : ShopItemSO
    {
        public string UnlockName;

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
            playerStats.Stats[UnlockName].Unlocked = true;
        }
    }

}
