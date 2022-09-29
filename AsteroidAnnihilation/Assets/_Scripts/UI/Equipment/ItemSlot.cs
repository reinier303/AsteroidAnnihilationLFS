using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private UIManager uIManager;
        private EquipmentManager equipmentManager;
        private InventoryManager inventoryManager;

        private ItemData item;
        private EquipmentData equipment;
        private WeaponData weapon;
        [SerializeField] private Image icon;

        public EnumCollections.ItemType itemType;
        private enum SlotDataType {None, Item, Equipment, Weapon}
        SlotDataType slotDataType;

        private void Awake()
        {
            uIManager = UIManager.Instance;
            slotDataType = SlotDataType.None;
        }

        private void Start()
        {
            equipmentManager = EquipmentManager.Instance;
            inventoryManager = InventoryManager.Instance;
        }

        public void InitializeSlot(ItemData item)
        {
            slotDataType = SlotDataType.Item;
            this.item = item;
            SetIcon(item.Icon);
        }

        public void InitializeSlot(EquipmentData equip)
        {
            slotDataType = SlotDataType.Equipment;
            equipment = equip;
            SetIcon(equip.ItemData.Icon);
        }

        public void InitializeSlot(WeaponData weapon)
        {
            slotDataType = SlotDataType.Weapon;
            this.weapon = weapon;
            SetIcon(weapon.EquipmentData.ItemData.Icon);
        }

        private void SetIcon(Sprite Icon)
        {
            icon.sprite = Icon;
            icon.color = Color.white;
        }

        public void ClearSlot()
        {
            switch (slotDataType)
            {
                case SlotDataType.None:
                    break;
                case SlotDataType.Item:
                    item = default;
                    break;
                case SlotDataType.Equipment:
                    equipment = default;
                    break;
                case SlotDataType.Weapon:
                    weapon = default;
                    break;
            }
            Debug.Log("got here");
            slotDataType = SlotDataType.None;
            icon.sprite = null;
            icon.color = new Color(255,255,255,0);

        }

        public bool ContainsItem()
        {
            if(slotDataType == SlotDataType.None)
            {
                return false;
            }
            else { return true; }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (slotDataType)
            {
                case SlotDataType.None:
                    return;
                case SlotDataType.Item:
                    uIManager.ShowItemTooltip(item);
                    break;
                case SlotDataType.Equipment:
                    uIManager.ShowItemTooltip(equipment);
                    break;
                case SlotDataType.Weapon:
                    uIManager.ShowItemTooltip(weapon);
                    break;
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uIManager.HideItemTooltip();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                if (itemType == EnumCollections.ItemType.Inventory)
                {
                    InventoryClick();
                }
                else
                {
                    EquipmentClick();
                }
            }
        }

        private void InventoryClick()
        {
            switch (slotDataType)
            {
                case SlotDataType.None:
                    return;
                case SlotDataType.Item:
                    return;
                case SlotDataType.Equipment:
                    return;
                case SlotDataType.Weapon:
                    (bool succes, WeaponData data) = equipmentManager.ChangeWeapon(weapon);
                    if (!data.Equals(default(WeaponData)))
                    {
                        inventoryManager.AddItem(data);
                    }
                    if (succes)
                    {
                        inventoryManager.RemoveItem(weapon);
                    }
                    break;
            }
        }

        private void EquipmentClick()
        {
            switch (slotDataType)
            {
                case SlotDataType.None:
                    return;
                case SlotDataType.Item:
                    return;
                case SlotDataType.Equipment:
                    return;
                case SlotDataType.Weapon:
                    if (inventoryManager.InventoryFull()) {
                        Debug.Log("Inventory Full");
                        return; }
                    WeaponData data = equipmentManager.RemoveWeapon(weapon);
                    inventoryManager.AddItem(data);
                    ClearSlot();
                    break;
            }
        }
    }
}
