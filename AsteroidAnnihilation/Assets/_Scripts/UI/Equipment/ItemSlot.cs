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
        public EquipmentData equipment;
        private WeaponData weapon;
        [SerializeField] private Image icon;

        public EnumCollections.ItemType slotType;
        public enum SlotDataType { None, Item, Equipment, Weapon }
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
            Debug.Log("dataType" + slotDataType);
            switch (slotDataType)
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

        public void SetItem(ItemData item)
        {
            this.item = item;
        }

        public void SetItem(EquipmentData equip)
        {
            if(equip.ItemData.ItemName == null) { return; }
            slotDataType = SlotDataType.Equipment;
            equipment = equip;
        }

        public void SetItem(WeaponData weapon)
        {
            slotDataType = SlotDataType.Weapon;
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
            icon.color = new Color(255, 255, 255, 0);

        }

        public bool ContainsItem()
        {
            if (slotDataType == SlotDataType.None)
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
            if (eventData.button == PointerEventData.InputButton.Right)
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
                    (bool equipSucces, EquipmentData equipData) = equipmentManager.ChangeGear(equipment);

                    if (!equipData.Equals(default(EquipmentData)))
                    {
                        inventoryManager.AddItem(equipData);
                    }
                    break;
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
                        MoveFromEquipment(hoveredSlot);
                        break;
                    case EnumCollections.ItemType.HullPlating:
                        MoveFromEquipment(hoveredSlot);
                        break;
                    case EnumCollections.ItemType.EnergyCore:
                        MoveFromEquipment(hoveredSlot);
                        break;
                    case EnumCollections.ItemType.Engine:
                        MoveFromEquipment(hoveredSlot);
                        break;
                    case EnumCollections.ItemType.Inventory:
                        MoveFromInventory(hoveredSlot);
                        break;
                }
            }
            else if (eventData.pointerCurrentRaycast.gameObject.tag == "Drop")
            {
                //TODO::Make this drop the item
                if (slotType == EnumCollections.ItemType.Inventory)
                {
                    //Warning you sure?
                    switch (slotDataType)
                    {
                        case SlotDataType.None:
                            return;
                        case SlotDataType.Item:
                            inventoryManager.RemoveItem(item, transform.GetSiblingIndex());
                            break;
                        case SlotDataType.Equipment:
                            inventoryManager.RemoveItem(equipment, transform.GetSiblingIndex());
                            break;
                        case SlotDataType.Weapon:
                            inventoryManager.RemoveItem(weapon, transform.GetSiblingIndex());
                            break;
                    }
                }
            }
        }

        private void MoveFromEquipment(ItemSlot hoveredSlot)
        {
            int hoverIndex = hoveredSlot.transform.GetSiblingIndex();

            switch (slotDataType)
            {
                case SlotDataType.None:
                    return;
                case SlotDataType.Item:
                    return;
                case SlotDataType.Equipment:

                    //If item moves to inventory
                    if (hoveredSlot.slotType == EnumCollections.ItemType.Inventory)
                    {
                        //Remove current piece of equipment
                        equipmentManager.RemoveGear(equipment);

                        //Add Equipment piece to and retrieve equipment data already in slot
                        (bool equipmentSuccess, EquipmentData equipmentData) = inventoryManager.AddItem(equipment, hoverIndex);
                        ClearSlot();
                        if (!equipmentData.Equals(default(EquipmentData)))
                        {
                            equipmentManager.ChangeGear(equipmentData);
                        }
                        return;
                    }
                    else { return; }
                case SlotDataType.Weapon:

                    //If item moves to inventory
                    if (hoveredSlot.slotType == EnumCollections.ItemType.Inventory)
                    {
                        equipmentManager.RemoveWeapon(transform.GetSiblingIndex());
                        //inventoryManager
                        (bool success, WeaponData data) = inventoryManager.AddItem(weapon, hoverIndex);
                        ClearSlot();
                        if (!data.Equals(default(WeaponData)))
                        {
                            equipmentManager.ChangeWeapon(data, transform.GetSiblingIndex());
                        }
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
            int hoverIndex = hoveredSlot.transform.GetSiblingIndex();

            switch (slotDataType)
            {
                case SlotDataType.None:
                    return;
                case SlotDataType.Item:
                    return;
                case SlotDataType.Equipment:
                    //If item moves to inventory
                    if (hoveredSlot.slotType == EnumCollections.ItemType.Inventory)
                    {
                        if (hoveredSlot.slotDataType == SlotDataType.Equipment)
                        {
                            //Get item in hoveredSlot and remove it
                            EquipmentData data = inventoryManager.RemoveItem(equipment, hoverIndex);

                            //Add current item to hoveredSlot
                            inventoryManager.AddItem(equipment, hoverIndex);

                            //Remove current item from current slot
                            inventoryManager.RemoveItem(equipment, transform.GetSiblingIndex());

                            //Add item in hoveredSlot to current slot if hovered slot contains item
                            if (!data.Equals(default(EquipmentData)))
                            {
                                inventoryManager.AddItem(data, transform.GetSiblingIndex());
                            }
                            return;
                        }
                        else if (hoveredSlot.slotDataType == SlotDataType.Weapon)
                        {
                            //Get item in hoveredSlot and remove it
                            WeaponData data = inventoryManager.RemoveItem(weapon, hoverIndex);

                            //Add current item to hoveredSlot
                            inventoryManager.AddItem(equipment, hoverIndex);

                            //Remove current item from current slot
                            inventoryManager.RemoveItem(equipment, transform.GetSiblingIndex());

                            //Add item in hoveredSlot to current slot if hovered slot contains item
                            if (!data.Equals(default(WeaponData)))
                            {
                                inventoryManager.AddItem(data, transform.GetSiblingIndex());
                            }
                            return;
                        }
                        else if (hoveredSlot.slotDataType == SlotDataType.None)
                        {
                            //Add current item to hoveredSlot
                            inventoryManager.AddItem(equipment, hoverIndex);

                            //Remove current item from current slot
                            inventoryManager.RemoveItem(equipment, transform.GetSiblingIndex());
                            return;
                        }
                        else { return; }
                    }
                    //Case where we move from inventory to equipment slot
                    else if (hoveredSlot.slotType == EnumCollections.ItemType.HullPlating ||
                             hoveredSlot.slotType == EnumCollections.ItemType.Engine ||
                             hoveredSlot.slotType == EnumCollections.ItemType.EnergyCore)
                    {
                        (bool succes, EquipmentData data) = equipmentManager.ChangeGear(equipment);
                        if (succes)
                        {
                            inventoryManager.RemoveItem(equipment, transform.GetSiblingIndex());

                            if (!data.Equals(default(EquipmentData)))
                            {
                                inventoryManager.AddItem(data, transform.GetSiblingIndex());
                            }
                            //Remove current weapon from inventory
                            //ClearSlot();
                        }

                        inventoryManager.InitializeWeapons();
                        return;
                    }
                    else { return; }
                case SlotDataType.Weapon:
                    //If item moves to inventory
                    if (hoveredSlot.slotType == EnumCollections.ItemType.Inventory)
                    {
                        if (hoveredSlot.slotDataType == SlotDataType.Weapon)
                        {
                            //Get item in hoveredSlot and remove it
                            WeaponData data = inventoryManager.RemoveItem(weapon, hoverIndex);

                            //Add current item to hoveredSlot
                            inventoryManager.AddItem(weapon, hoverIndex);

                            //Remove current item from current slot
                            inventoryManager.RemoveItem(weapon, transform.GetSiblingIndex());

                            //Add item in hoveredSlot to current slot if hovered slot contains item
                            if (!data.Equals(default(WeaponData)))
                            {
                                inventoryManager.AddItem(data, transform.GetSiblingIndex());
                            }
                            return;
                        }
                        else if (hoveredSlot.slotDataType == SlotDataType.Equipment)
                        {
                            //Get item in hoveredSlot and remove it
                            EquipmentData data = inventoryManager.RemoveItem(equipment, hoverIndex);

                            //Add current item to hoveredSlot
                            inventoryManager.AddItem(weapon, hoverIndex);

                            //Remove current item from current slot
                            inventoryManager.RemoveItem(weapon, transform.GetSiblingIndex());

                            //Add item in hoveredSlot to current slot if hovered slot contains item
                            if (!data.Equals(default(EquipmentData)))
                            {
                                inventoryManager.AddItem(data, transform.GetSiblingIndex());
                            }
                            return;
                        }
                        else if (hoveredSlot.slotDataType == SlotDataType.None)
                        {
                            //Add current item to hoveredSlot
                            inventoryManager.AddItem(weapon, hoverIndex);

                            //Remove current item from current slot
                            inventoryManager.RemoveItem(weapon, transform.GetSiblingIndex());

                            return;
                        }
                        else { return; }
                    }
                    //Case where we move from inventory to weaponslot
                    else if (hoveredSlot.slotType == EnumCollections.ItemType.Weapon)
                    {
                        (bool succes, WeaponData data) = equipmentManager.ChangeWeapon(weapon, hoverIndex);
                        if (succes)
                        {
                            inventoryManager.RemoveItem(weapon, transform.GetSiblingIndex());

                            if (!data.Equals(default(WeaponData)))
                            {
                                inventoryManager.AddItem(data, transform.GetSiblingIndex());
                            }
                            //Remove current weapon from inventory
                            //ClearSlot();
                        }

                        inventoryManager.InitializeWeapons();
                        return;
                    }
                    else { return; }

            }
        }


    }
}
