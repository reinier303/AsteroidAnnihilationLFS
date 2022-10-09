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
        public enum SlotDataType {None, Item, Equipment, Weapon}
        public SlotDataType slotDataType;

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

        public void InitializeSlot()
        {
            switch(slotDataType)
            {
                case SlotDataType.None:
                    return;
                case SlotDataType.Item:
                    SetIcon(item.Icon);
                    break;
                case SlotDataType.Equipment:
                    SetIcon(equipment.ItemData.Icon);
                    break;
                case SlotDataType.Weapon:
                    SetIcon(weapon.EquipmentData.ItemData.Icon);
                    break;
            }
        }

        public void AddItem(ItemData item)
        {
            slotDataType = SlotDataType.Item;
            this.item = item;
        }

        public void AddItem(EquipmentData equip)
        {
            slotDataType = SlotDataType.Equipment;
            equipment = equip;
        }

        public void AddItem(WeaponData weapon)
        {
            slotDataType = SlotDataType.Weapon;
            Debug.Log(transform.name);
            this.weapon = weapon;
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
                    equipmentManager.MoveWeaponToInventory(index);
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

            iconTransform.anchoredPosition = iconStartPos;

            if (hoveredSlot != null)
            {
                //Check what type of slot our current slot is
                switch (slotType)
                {
                    case EnumCollections.ItemType.Weapon:
                        SwapEquipment(hoveredSlot);
                        break;
                    case EnumCollections.ItemType.Inventory:
                        MoveFromInventory(hoveredSlot);
                        break;
                }
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
                    int hoverIndex = hoveredSlot.transform.GetSiblingIndex();

                    //If item moves to inventory
                    if (hoveredSlot.slotType == EnumCollections.ItemType.Inventory)
                    {
                        equipmentManager.RemoveWeapon(transform.GetSiblingIndex());
                        inventoryManager.AddItem(weapon);
                        ClearSlot();
                        //ClearSlot();

                        return;
                    }
                    //Case where we swap with other weaponslot
                    else if (hoveredSlot.slotType == EnumCollections.ItemType.Weapon)
                    {
                        (bool succes, WeaponData data) = equipmentManager.ChangeWeapon(weapon, hoverIndex);
                        if (succes)
                        {
                            equipmentManager.RemoveWeapon(transform.GetSiblingIndex());
                            ClearSlot();
                        }
                        if (!data.Equals(default(WeaponData)))
                        {
                            equipmentManager.ChangeWeapon(data, transform.GetSiblingIndex());
                        }
                        inventoryManager.InitializeWeapons();
                        return;
                    }
                    else { return; }

            }
        }

        private void MoveFromInventory(ItemSlot hoveredSlot)
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
                    int hoverIndex = hoveredSlot.transform.GetSiblingIndex();

                    //If item moves to inventory
                    if (hoveredSlot.slotType == EnumCollections.ItemType.Inventory)
                    {
                        inventoryManager.AddItem(weapon, hoverIndex);
                        ClearSlot();
                        return;
                    }
                    //Case where we swap with other weaponslot
                    else if (hoveredSlot.slotType == EnumCollections.ItemType.Weapon)
                    {
                        (bool succes, WeaponData data) = equipmentManager.ChangeWeapon(weapon, hoverIndex);
                        if (succes)
                        {
                            inventoryManager.RemoveItem(weapon, hoverIndex);
                            equipmentManager.RemoveWeapon(transform.GetSiblingIndex());
                            ClearSlot();
                        }
                        if (!data.Equals(default(WeaponData)))
                        {
                            inventoryManager.AddItem(weapon);
                        }
                        inventoryManager.InitializeWeapons();
                        return;
                    }
                    else { return; }

            }
        }
    }
}
