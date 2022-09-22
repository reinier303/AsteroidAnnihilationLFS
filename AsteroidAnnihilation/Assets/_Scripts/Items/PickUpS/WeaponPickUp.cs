using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class WeaponPickUp : MonoBehaviour
    {
        private EquipmentManager equipmentManager;
        private InventoryManager inventoryManager;

        private WeaponData weaponData;

        public void Initialize()
        {
            equipmentManager = EquipmentManager.Instance;
            weaponData = equipmentManager.GenerateWeapon();
        }
    }
}
