using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class WeaponPickUp : MonoBehaviour
    {
        private EquipmentManager equipmentManager;
        private InventoryManager inventoryManager;
        private SettingsManager settingsManager;

        private EquipmentData equipmentData; 
        private WeaponData weaponData;

        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer icon;

        private GeneralItemSettings generalItemSettings;
        private EnumCollections.ItemType itemType;

        //TEMP FOR TESTING
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            settingsManager = SettingsManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            inventoryManager = InventoryManager.Instance;
            generalItemSettings = settingsManager.generalItemSettings;

            itemType = GetItemType();
            Debug.Log(itemType.ToString());
            GenerateDrop();

            icon.sprite = weaponData.EquipmentData.ItemData.Icon;
            background.material = generalItemSettings.GetRarityMaterial(weaponData.EquipmentData.ItemData.Rarity);
            //SpawnEffect based on rarity
            //EmissionEffect based on rarity

        }

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
            if (random <= 5)
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
                        gameObject.SetActive(false);
                    }
                    else { return false; }
                    break;
                case EnumCollections.ItemType.HullPlating:
                    (bool equipmentSuccess, EquipmentData equipData) = inventoryManager.AddItem(equipmentData);
                    if (equipmentSuccess)
                    {
                        gameObject.SetActive(false);
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
    }
}
