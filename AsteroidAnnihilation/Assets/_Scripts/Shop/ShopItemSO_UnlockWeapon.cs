using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Shop/Unlock Weapon", order = 999)]
    public class ShopItemSO_UnlockWeapon : ShopItemSO
    {
        /*
        public string WeaponName;

        private PlayerAttack playerAttack;

        public void GetPlayerAttack()
        {
            playerAttack = GameManager.Instance.RPlayer.RPlayerAttack;
        }

        public override void ItemBought()
        {
            base.ItemBought();
            if (playerAttack == null)
            {
                GetPlayerAttack();
            }
            playerAttack.UnlockWeapon(WeaponName);
        }
        */
    }

}
