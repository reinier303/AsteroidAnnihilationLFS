using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ShopItemSO : ScriptableObject
    {
        public string ItemName;
        public int Cost;
        [TextArea(10, 10)]
        public string Description;
        public Sprite sprite;
        public List<ShopItemSO> UnlockAfterBuying;
        public bool UnlockedOnStart;

        public virtual void ItemBought()
        {
            ShopPanel shopPanel = ShopPanel.Instance;
            shopPanel.RemoveAvailableItem(this);
            foreach(ShopItemSO item in UnlockAfterBuying)
            {
                shopPanel.AddAvailableItem(item);
            }
            //This method is meant to be overridden
        }
    }
}
