using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ShopPanel : MonoBehaviour
    {
        public static ShopPanel Instance;

        public List<ShopItemSO> CurrentShopItems;

        [SerializeField] private GameObject ShopItemPrefab;

        [SerializeField] private Transform ShopItemPanel;

        private ShopData shopData;

        private void Awake()
        {
            Instance = this;
            InitializeShopItems();
        }

        private void Start()
        {
            GameManager.Instance.onEndGame += SaveShopItems;
        }

        private void InitializeShopItems()
        {
            if(!SaveLoad.SaveExists("ShopData"))
            {
                Debug.Log("New Shop Save");
                NewSave();
            }
            else
            {
                LoadShopItems();
            }
        }

        private void NewSave()
        {
            shopData.ShopItemsAvailabe = new List<string>();
            Object[] ResourceShopItems = Resources.LoadAll("ShopItems", typeof(ShopItemSO));
            foreach (ShopItemSO item in ResourceShopItems)
            {
                if (item.UnlockedOnStart)
                {
                    CurrentShopItems.Add(item);
                    shopData.ShopItemsAvailabe.Add(item.ItemName);
                    GameObject shopItem = Instantiate(ShopItemPrefab, ShopItemPanel);
                    shopItem.GetComponent<ShopItemUIElement>().Initialize(item);
                }
            }
        }

        private void LoadShopItems()
        {
            shopData = SaveLoad.Load<ShopData>("ShopData");
            Object[] ResourceShopItems = Resources.LoadAll("ShopItems", typeof(ShopItemSO));
            foreach (ShopItemSO item in ResourceShopItems)
            {
                if (shopData.ShopItemsAvailabe.Contains(item.ItemName))
                {
                    CurrentShopItems.Add(item);
                    GameObject shopItem = Instantiate(ShopItemPrefab, ShopItemPanel);
                    shopItem.GetComponent<ShopItemUIElement>().Initialize(item);
                }
            }
        }

        public void RemoveAvailableItem(ShopItemSO item)
        {
            shopData.ShopItemsAvailabe.Remove(item.ItemName);
        }

        public void AddAvailableItem(ShopItemSO item)
        {
            CurrentShopItems.Add(item);
            shopData.ShopItemsAvailabe.Add(item.ItemName);
            GameObject shopItem = Instantiate(ShopItemPrefab, ShopItemPanel);
            shopItem.GetComponent<ShopItemUIElement>().Initialize(item);
        }

        public void SaveShopItems()
        {
            SaveLoad.Save(shopData, "ShopData");
        }
    }

    [System.Serializable]
    public struct ShopData
    {
        public List<string> ShopItemsAvailabe;
    }
}
