using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        private GameManager gameManager;
        private EquipmentManager equipmentManager;

        public int InventorySlots;
        List<ItemSlot> ItemSlots;

        public List<ItemData> InventoryItems;
        public List<EquipmentData> InventoryEquipment;
        public List<WeaponData> InventoryWeapons;

        private Transform inventoryPanel;
        [SerializeField] private GameObject inventorySlot;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.onEndGame += SaveInventory;
            LoadInventory();
        }

        private void SaveInventory()
        {
            ES3.Save("inventorySlots", InventorySlots);
            ES3.Save("inventoryItems", InventoryItems);
            ES3.Save("inventoryEquipment", InventoryEquipment);
            ES3.Save("inventoryWeapons", InventoryWeapons);
        }

        private void LoadInventory()
        {
            if (!ES3.KeyExists("inventoryItems"))
            {
                InventorySlots = 24;
                InventoryItems = new List<ItemData>();
                InventoryEquipment = new List<EquipmentData>();
                InventoryWeapons = new List<WeaponData>();
                SaveInventory();
            }
            else
            {
                InventorySlots = (int)ES3.Load("inventorySlots");
                InventoryItems = (List<ItemData>)ES3.Load("inventoryItems");
                InventoryEquipment = (List<EquipmentData>)ES3.Load("inventoryEquipment");
                InventoryWeapons = (List<WeaponData>)ES3.Load("inventoryWeapons");
            }
            ItemSlots = new List<ItemSlot>();
        }

        public void OpenInventory(Transform inventoryPanel)
        {
            if(ItemSlots.Count != InventorySlots)
            {
                for (int i = 0; i < InventorySlots; i++)
                {
                    ItemSlot slot = inventoryPanel.GetChild(i).GetComponent<ItemSlot>();
                    slot.gameObject.SetActive(true);
                    ItemSlots.Add(slot);
                }
            }
            int currentSlot = 0;
            for(int i = 0; i < InventoryWeapons.Count; i++)
            {
                ItemSlot itemSlot = ItemSlots[currentSlot];
                itemSlot.InitializeSlot(InventoryWeapons[i]);
                currentSlot++;
            }
            for (int i = 0; i < InventoryEquipment.Count; i++)
            {
                ItemSlot itemSlot = ItemSlots[currentSlot];
                itemSlot.InitializeSlot(InventoryEquipment[i]);
                currentSlot++;
            }
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                ItemSlot itemSlot = ItemSlots[currentSlot];
                itemSlot.InitializeSlot(InventoryItems[i]);
                currentSlot++;
            }
        }

        public bool AddItem(ItemData item)
        {
            if(GetItemCount() < InventorySlots)
            {
                InventoryItems.Add(item);
                return true;
            } else { return false; }
        }

        public bool AddItem(EquipmentData equipment)
        {
            if (GetItemCount() < InventorySlots)
            {
                InventoryEquipment.Add(equipment);
                return true;
            }
            else { return false; }
        }

        public bool AddItem(WeaponData weapon)
        {
            if (GetItemCount() < InventorySlots)
            {
                InventoryWeapons.Add(weapon);
                return true;
            }
            else { return false; }
        }

        public int GetItemCount()
        {
            return InventoryItems.Count + InventoryEquipment.Count + InventoryWeapons.Count;
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