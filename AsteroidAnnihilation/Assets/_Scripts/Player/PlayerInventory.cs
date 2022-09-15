using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory Instance;
        private float Units;

        private Dictionary<string, PickUpData> inventory;

        private void Awake()
        {
            Instance = this;
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            inventory = new Dictionary<string, PickUpData>();
        }

        public void AddToInventory(string pickUpName, float Value)
        {
            //If inventory does not contain item yet add it tp the dictionary
            if(!inventory.ContainsKey(pickUpName))
            {
                PickUpData pData;
                pData.PickUpName = pickUpName;
                pData.Amount = Value;

                inventory.Add(pickUpName, pData);
            }
            else
            {
                //Create new pData because directly changing pData[pickUpName].Amount is not allowed
                PickUpData pData;
                pData.Amount = inventory[pickUpName].Amount + Value;
                pData.PickUpName = pickUpName;

                inventory[pickUpName] = pData;
            }
            Debug.Log(inventory[pickUpName].PickUpName + ": " + inventory[pickUpName].Amount);
        }

        private List<PickUpData> InventoryDictionaryToList()
        {
            List<PickUpData> inventoryList = new List<PickUpData>();

            foreach (PickUpData pickUp in inventory.Values)
            {
                inventoryList.Add(pickUp);
            }
            return inventoryList;
        }

        public void SaveInventory()
        {
            //Save inventory list to file
            SaveLoad.Save(InventoryDictionaryToList(), "Inventory");
        }
    }
}