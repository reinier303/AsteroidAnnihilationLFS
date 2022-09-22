using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        private GameManager gameManager;

        public List<ItemData> InventoryItems;
        public List<EquipmentData> InventoryEquipment;
        public List<WeaponData> InventoryWeapons;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            gameManager.onEndGame += SaveInventory;
            LoadInventory();
        }

        private void SaveInventory()
        {
            ES3.Save("inventoryItems", InventoryItems);
            ES3.Save("inventoryEquipment", InventoryEquipment);
            ES3.Save("inventoryWeapons", InventoryWeapons);
        }

        private void LoadInventory()
        {
            if (!ES3.KeyExists("inventoryItems"))
            {
                InventoryItems = new List<ItemData>();
                InventoryEquipment = new List<EquipmentData>();
                InventoryWeapons = new List<WeaponData>();
                SaveInventory();
            }
            else
            {
                InventoryItems = (List<ItemData>)ES3.Load("inventoryItems");
                InventoryEquipment = (List<EquipmentData>)ES3.Load("inventoryEquipment");
                InventoryWeapons = (List<WeaponData>)ES3.Load("inventoryWeapons");
            }
        }

        public void OpenInventory()
        {

        }
    }

    public struct ItemData
    {
        public string ItemName;
        public int Tier;
        public EnumCollections.Rarities Rarity;
        public EnumCollections.ItemType ItemType;
        public Sprite Icon;
    }
}
