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

        private WeaponData weaponData;

        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer icon;

        private GeneralItemSettings generalItemSettings;


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
            weaponData = equipmentManager.GenerateWeapon();
            icon.sprite = weaponData.EquipmentData.ItemData.Icon;
            background.material = generalItemSettings.GetRarityMaterial(weaponData.EquipmentData.ItemData.Rarity);
            //SpawnEffect based on rarity
            //EmissionEffect based on rarity
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                //SPAWN EFFECT objectPooler.SpawnFromPool("CoinPickupEffect", transform.position, Quaternion.identity);
                if(inventoryManager.AddItem(weaponData))
                {
                    gameObject.SetActive(false);
                }
                else { Debug.Log("Inventory Full"); }
            }
        }
    }
}
