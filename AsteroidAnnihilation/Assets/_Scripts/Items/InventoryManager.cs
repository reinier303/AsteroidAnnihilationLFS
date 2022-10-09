using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class InventoryManager : SerializedMonoBehaviour
    {
        public static InventoryManager Instance;

        private GameManager gameManager;
        private EquipmentManager equipmentManager;
        private SettingsManager settingsManager;

        public int InventorySlots;
        [SerializeField] Dictionary<int, ItemSlot> ItemSlots;

        public Dictionary<int, ItemData> InventoryItems;
        public Dictionary<int, EquipmentData> InventoryEquipment;
        public Dictionary<int, WeaponData> InventoryWeapons;

        private Transform inventoryPanel;
        Transform weaponSlotParent;
        Transform gearSlotParent;
        Transform componentSlotParent;
        [SerializeField] private GameObject inventorySlot;

        private GameObject DraggedObject;

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
        }

        private void SaveInventory()
        {
            ES3.Save("inventorySlots", InventorySlots);
            ES3.Save("inventoryItems", InventoryItems);
            ES3.Save("inventoryEquipment", InventoryEquipment);
            ES3.Save("inventoryWeapons", InventoryWeapons);
        }

        public void LoadInventory()
        {
            if (!ES3.KeyExists("inventoryItems"))
            {
                InventorySlots = 24;
                InventoryItems = new Dictionary<int, ItemData>();
                InventoryEquipment = new Dictionary<int, EquipmentData>();
                InventoryWeapons = new Dictionary<int, WeaponData>();
                SaveInventory();
            }
            else
            {
                InventorySlots = (int)ES3.Load("inventorySlots");
                InventoryItems = (Dictionary<int, ItemData>)ES3.Load("inventoryItems");
                InventoryEquipment = (Dictionary<int, EquipmentData>)ES3.Load("inventoryEquipment");
                InventoryWeapons = (Dictionary<int, WeaponData>)ES3.Load("inventoryWeapons");
            }
            ItemSlots = new Dictionary<int, ItemSlot>();
            for (int i = 0; i < InventorySlots; i++)
            {
                ItemSlot slot = inventoryPanel.GetChild(i).GetComponent<ItemSlot>();
                slot.gameObject.SetActive(true);
                ItemSlots.Add(i, slot);
            }
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
            foreach (ItemSlot slot in ItemSlots.Values)
            {
                //slot.ClearSlot();
                slot.InitializeSlot();
            }
        }

        public void InitializeWeapons()
        {
            int weaponSlots = settingsManager.playerShipSettings.WeaponPositions[EnumCollections.ShipType.Fighter].Count;
            Dictionary<int, WeaponData> weapons = equipmentManager.GetAllEquipedWeapons();
            for (int i = 0; i < weaponSlotParent.childCount; i++)
            {
                GameObject weaponSlot = weaponSlotParent.GetChild(i).gameObject;
                if (i < weaponSlots)
                {
                    weaponSlot.SetActive(true);
                    if (weapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                    if (weapons.Count > i) { 
                        weaponSlot.GetComponent<ItemSlot>().AddItem(weapons[i]);
                        weaponSlot.GetComponent<ItemSlot>().InitializeSlot();
                    }
                }
            }
        }

        public void InitializeGear()
        {
            for (int i = 0; i < gearSlotParent.childCount; i++)
            {
                GameObject gearSlot = gearSlotParent.GetChild(i).gameObject;
                ItemSlot slot = gearSlot.GetComponent<ItemSlot>();

                slot.AddItem(equipmentManager.GetGear(slot.slotType));
            }
        }

        public void InitializeShipComponents()
        {
            //Update trinkets/components
        }

        public bool AddItem(ItemData item, int index = -1)
        {
            if (!InventoryFull())
            {
                if (index == -1)
                {
                    index = GetAvailableSlotIndex();
                    if (index == -1) { return false; }
                }
                InventoryItems.Add(index, item);
                ItemSlots[index].AddItem(item);
                InitializeInventoryItems();
                return true;
            } else { return false; }
        }

        public bool AddItem(EquipmentData equipment, int index = -1)
        {
            if (!InventoryFull())
            {
                if (index == -1)
                {
                    index = GetAvailableSlotIndex();
                    if (index == -1) { return false; }
                }
                InventoryEquipment.Add(index, equipment);
                ItemSlots[index].AddItem(equipment);
                InitializeInventoryItems();
                return true;
            }
            else { return false; }
        }

        public bool AddItem(WeaponData weapon, int index = -1)
        {
            if (!InventoryFull())
            {
                if (index == -1)
                {
                    index = GetAvailableSlotIndex();
                    Debug.Log(index);
                    if (index == -1) { return false; }
                }

                InventoryWeapons.Add(index, weapon);
                inventoryPanel.GetChild(index).GetComponent<ItemSlot>().AddItem(weapon);

                InitializeInventoryItems();
                return true;
            }
            else { return false; }
        }

        public void RemoveItem(ItemData item, int index)
        {
            InventoryItems.Remove(index);
            ItemSlots[index] = default;
            InitializeInventoryItems();
        }

        public void RemoveItem(EquipmentData equipment, int index)
        {
            InventoryEquipment.Remove(index);
            ItemSlots[index] = default;
            InitializeInventoryItems();
        }

        public void RemoveItem(WeaponData weapon, int index)
        {
            InventoryWeapons.Remove(index);
            ItemSlots[index] = default;
            InitializeInventoryItems();
        }

        public int GetItemCount()
        {
            return (InventoryItems.Count + InventoryEquipment.Count + InventoryWeapons.Count);
        }

        public bool InventoryFull()
        {
            return GetItemCount() >= InventorySlots;
        }

        private int GetAvailableSlotIndex()
        {
            for(int i = 0; i < ItemSlots.Count; i++)
            {
                Debug.Log(ItemSlots[i].slotDataType.ToString());
                if (!ItemSlots[i].ContainsItem())
                {
                    return i;
                }
            }
            return -1;
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
