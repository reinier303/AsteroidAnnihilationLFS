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

        private ItemData item;
        private EquipmentData equipment;
        private WeaponData weapon;
        [SerializeField] private Image icon;

        public EnumCollections.ItemType ItemType;
        private enum SlotDataType {None, Item, Equipment, Weapon}
        SlotDataType slotDataType;

        private void Awake()
        {
            uIManager = UIManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            slotDataType = SlotDataType.None;
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
                switch (slotDataType)
                {
                    case SlotDataType.None:
                        return;
                    case SlotDataType.Item:
                        return;
                    case SlotDataType.Equipment:
                        return;
                    case SlotDataType.Weapon:
                        equipmentManager.ChangeWeapon(0, weapon);
                        break;
                }
            }
        }
    }
}
