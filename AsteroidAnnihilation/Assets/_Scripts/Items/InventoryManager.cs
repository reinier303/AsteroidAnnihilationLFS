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
        private SettingsManager settingsManager;

        public int InventorySlots;
        List<ItemSlot> ItemSlots;

        public List<ItemData> InventoryItems;
        public List<EquipmentData> InventoryEquipment;
        public List<WeaponData> InventoryWeapons;

        private Transform inventoryPanel;
        Transform weaponSlotParent;
        Transform gearSlotParent;
        Transform componentSlotParent;
        [SerializeField] private GameObject inventorySlot;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            settingsManager = SettingsManager.Instance;
            equipmentManager = EquipmentManager.Instance;
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

        public void OpenInventory()
        {
            InitializeInventoryItems();

            //TODO::Initialize Components/Trinkets
            InitializeWeapons();
            InitializeGear();
            InitializeShipComponents();
        }

        public void SetUIElements(Transform inventoryPanel, Transform weaponSlotParent, Transform gearSlotParent, Transform componentSlotParent)
        {
            if (this.inventoryPanel == null) { this.inventoryPanel = inventoryPanel; }
            if (this.weaponSlotParent == null) { this.weaponSlotParent = weaponSlotParent; }
            if (this.gearSlotParent == null) { this.gearSlotParent = gearSlotParent; }
            if (this.componentSlotParent == null) { this.componentSlotParent = componentSlotParent; }
        }

        private void InitializeInventoryItems()
        {
            if (ItemSlots.Count != InventorySlots)
            {
                for (int i = 0; i < InventorySlots; i++)
                {
                    ItemSlot slot = inventoryPanel.GetChild(i).GetComponent<ItemSlot>();
                    slot.gameObject.SetActive(true);
                    ItemSlots.Add(slot);
                }
            }
            int currentSlot = 0;
            for (int i = 0; i < InventoryWeapons.Count; i++)
            {
                Debug.Log("i:"+ i +",  currentslot:" + currentSlot);
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
            foreach (ItemSlot slot in ItemSlots)
            {
                if (!slot.ContainsItem()) { slot.ResetSlot(); }
            }
        }

        public void InitializeWeapons()
        {
            int weaponSlots = settingsManager.playerShipSettings.WeaponPositions[EnumCollections.ShipType.Fighter].Count;
            for (int i = 0; i < weaponSlotParent.childCount; i++)
            {
                GameObject weaponSlot = weaponSlotParent.GetChild(i).gameObject;
                if (i < weaponSlots)
                {
                    weaponSlot.SetActive(true);
                    List<WeaponData> weapons = equipmentManager.GetAllEquipedWeapons();
                    if (weapons.Count > i) { weaponSlot.GetComponent<ItemSlot>().InitializeSlot(weapons[i]); }
                }
            }
        }

        public void InitializeGear()
        {
            for (int i = 0; i < gearSlotParent.childCount; i++)
            {
                GameObject gearSlot = gearSlotParent.GetChild(i).gameObject;
                ItemSlot slot = gearSlot.GetComponent<ItemSlot>();

                slot.InitializeSlot(equipmentManager.GetGear(slot.ItemType));
            }
        }

        public void InitializeShipComponents()
        {
            //Update trinkets/components
        }

        public bool AddItem(ItemData item)
        {
            if (GetItemCount() < InventorySlots)
            {
                InventoryItems.Add(item);
                InitializeInventoryItems();
                return true;
            } else { return false; }
        }

        public bool AddItem(EquipmentData equipment)
        {
            if (GetItemCount() < InventorySlots)
            {
                InventoryEquipment.Add(equipment);
                InitializeInventoryItems();
                return true;
            }
            else { return false; }
        }

        public bool AddItem(WeaponData weapon)
        {
            if (GetItemCount() < InventorySlots)
            {
                InventoryWeapons.Add(weapon);
                InitializeInventoryItems();
                return true;
            }
            else { return false; }
        }

        public void RemoveItem(ItemData item)
        {
            InventoryItems.Remove(item);
            InitializeInventoryItems();
        }

        public void RemoveItem(EquipmentData equipment)
        {
            InventoryEquipment.Remove(equipment);
            InitializeInventoryItems();
        }

        public void RemoveItem(WeaponData weapon)
        {
            InventoryWeapons.Remove(weapon);
            InitializeInventoryItems();
            Debug.Log(InventoryWeapons.Count);
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
