using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PickUp : MonoBehaviour
    {
        private EquipmentManager equipmentManager;
        private InventoryManager inventoryManager;
        private SettingsManager settingsManager;
        private AudioManager audioManager;
        private ObjectPooler objectPooler;

        private ItemData itemData;
        private EquipmentData equipmentData; 
        private WeaponData weaponData;

        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer icon;

        private GeneralItemSettings generalItemSettings;
        private EnumCollections.ItemType itemType;

        public void Initialize(Drop drop)
        {
            settingsManager = SettingsManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            inventoryManager = InventoryManager.Instance;
            audioManager = AudioManager.Instance;
            objectPooler = ObjectPooler.Instance;
            generalItemSettings = settingsManager.generalItemSettings;

            //itemType = GetItemType();
            //GenerateDrop();

            icon.sprite = weaponData.EquipmentData.ItemData.Icon;
            //TODO:: Fix this with ObjectPooler and make it return itself to poolparent after disabling
            //Instantiate(generalItemSettings.GetRarityMaterial(weaponData.EquipmentData.ItemData.Rarity), transform.position, transform.rotation, transform);

            switch (drop.ItemType)
            {
                case EnumCollections.ItemType.Weapon:
                    weaponData = equipmentManager.GenerateWeapon();
                    break;
                case EnumCollections.ItemType.ShipComponent:
                    equipmentData = equipmentManager.GenerateEquipment();
                    break;
                case EnumCollections.ItemType.Material:
                    itemData = equipmentManager.GenerateItemData(drop.Item);
                    itemData.Amount = Random.Range(drop.AmountRange.x, drop.AmountRange.y);
                    break;
            }
        }


        /*
        private EnumCollections.ItemType GetItemType()
        {
            float random = Random.Range(0, 100);
            EnumCollections.ItemType type = EnumCollections.ItemType.Material;
            if(random <= 100)
            {
                type = EnumCollections.ItemType.Weapon;
            }
            if (random <= 65)
            {
                type = EnumCollections.ItemType.HullPlating;//This is meant to be all equipment
            }
            //TODO::Implement this and turn it back to a normal number
            if (random <= 0)
            {
                type = EnumCollections.ItemType.ShipComponent; //Component once implemented
            }
            return type;
        }

        private void GenerateDrop()
        {
            switch(itemType)
            {
                case EnumCollections.ItemType.Weapon:
                    weaponData = equipmentManager.GenerateWeapon();
                    break;
                case EnumCollections.ItemType.HullPlating:
                    equipmentData = equipmentManager.GenerateEquipment();
                    break;
                case EnumCollections.ItemType.ShipComponent:
                    Debug.Log("Ship Component should be generated here");
                    //weaponData = equipmentManager.GenerateWeapon();
                    break;
            }
        }
        */

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                PickupDrop();
            }
        }

        private bool PickupDrop()
        {
            switch (itemType)
            {
                case EnumCollections.ItemType.Weapon:
                    //SPAWN EFFECT objectPooler.SpawnFromPool("CoinPickupEffect", transform.position, Quaternion.identity);
                    (bool success, WeaponData data) = inventoryManager.AddItem(weaponData);
                    if (success)
                    {
                        ItemPickedUp();
                    }
                    else { return false; }
                    break;
                case EnumCollections.ItemType.HullPlating:
                    (bool equipmentSuccess, EquipmentData equipData) = inventoryManager.AddItem(equipmentData);
                    if (equipmentSuccess)
                    {
                        ItemPickedUp();
                    }
                    else { return false; }
                    break;
                case EnumCollections.ItemType.ShipComponent:
                    Debug.Log("Whoop dee doo you picked up something that hasn't been implemented yet!");
                    gameObject.SetActive(false);
                    //weaponData = equipmentManager.GenerateWeapon();
                    break;
            }
            return false;
        }

        private void ItemPickedUp()
        {
            objectPooler.SpawnFromPool("EquipmentPickupEffect", transform.position, transform.rotation);
            audioManager.PlayAudio("PickupEquipment");
            gameObject.SetActive(false);
        }
    }
}
