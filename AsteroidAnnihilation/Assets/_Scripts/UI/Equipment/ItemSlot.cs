using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private UIManager uIManager;
        private EquipmentManager equipmentManager;
        private InventoryManager inventoryManager;

        private ItemData item;
        private EquipmentData equipment;
        private WeaponData weapon;
        [SerializeField] private Image icon;

        public EnumCollections.ItemType slotType;
        private enum SlotDataType {None, Item, Equipment, Weapon}
        SlotDataType slotDataType;

        private Vector2 iconStartPos;
        private RectTransform iconTransform;

        private void Awake()
        {
            uIManager = UIManager.Instance;
            slotDataType = SlotDataType.None;
            iconTransform = icon.GetComponent<RectTransform>();
            iconStartPos = iconTransform.anchoredPosition;
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
                if (eventData.pointerCurrentRaycast.gameObject == null) { return; }

                if (slotType == EnumCollections.ItemType.Inventory)
                {
                    InventoryClick();
                }
                else
                {

                    EquipmentClick(eventData.pointerCurrentRaycast.gameObject.transform.GetSiblingIndex());
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

                    break;
            }
        }

        private void EquipmentClick(int index)
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
                    equipmentManager.RemoveWeapon(index);
                    ClearSlot();
                    break;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!ContainsItem()) { return; }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!ContainsItem()) { return; }

            iconTransform.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!ContainsItem()) { return; }

            ItemSlot hoveredSlot = null;
            if (eventData.pointerCurrentRaycast.gameObject != null) { hoveredSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>(); }

            if (hoveredSlot != null)
            {
                SwapEquipment(hoveredSlot);
            }
            else
            {
                iconTransform.anchoredPosition = iconStartPos;
            }
        }

        private void SwapEquipment(ItemSlot hoveredSlot)
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
                    //Debug.Log(hoveredSlot.slotType.ToString());
                    int hoverIndex = hoveredSlot.transform.GetSiblingIndex();
                    Debug.Log(hoveredSlot.slotType.ToString() + ", " + hoverIndex);

                    //If item comes from inventory
                    if (slotType == EnumCollections.ItemType.Inventory)
                    {
                        (bool succes, WeaponData data) = equipmentManager.ChangeWeapon(weapon, hoverIndex);

                        ClearSlot();
                        if (!data.Equals(default(WeaponData)))
                        {
                            inventoryManager.AddItem(data);
                        }
                        return;
                    }
                    //Case where we swap with other weaponslot
                    else if (slotType == EnumCollections.ItemType.Weapon)
                    {
                        (bool succes, WeaponData data) = equipmentManager.ChangeWeapon(weapon, hoverIndex);
                        ClearSlot();
                        if (!data.Equals(default(WeaponData)))
                        {
                            equipmentManager.ChangeWeapon(weapon, transform.GetSiblingIndex());
                        }
                        inventoryManager.InitializeWeapons();
                        return;
                    }
                    else { return; }

            }
        }
    }
}
