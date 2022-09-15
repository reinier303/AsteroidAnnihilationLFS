using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class JSONWeaponDatabank : MonoBehaviour
    {
        public static JSONWeaponDatabank Instance;

        public List<ShopWeaponData> ShopWeaponDataInstance;

        private void Awake()
        {
            Instance = this;

            //Create list
            ShopWeaponDataInstance = new List<ShopWeaponData>();

            //Load JSON files from resources
            Object[] shopWeaponDataFiles = Resources.LoadAll("Weapons/JSONData", typeof(TextAsset));

            foreach (TextAsset jsonFile in shopWeaponDataFiles)
            {
                ShopWeaponData data = JsonUtility.FromJson<ShopWeaponData>(jsonFile.text);
                ShopWeaponDataInstance.Add(data);
            }
        }


        public ShopWeaponData LookUpWeapon(string weaponName)
        {
            foreach (ShopWeaponData data in ShopWeaponDataInstance)
            {
                if (data.WeaponName == weaponName)
                {
                    return data;
                }
            }
            Debug.LogWarning("Weapon not found, returning empty data");
            return new ShopWeaponData();
        }

        public ShopStatData LookUpStat(string weaponName, string statName)
        {
            //Lookup Weapon
            ShopWeaponData weaponData = new ShopWeaponData();
            weaponData.Index = 404;
            foreach (ShopWeaponData wData in ShopWeaponDataInstance)
            {
                if (wData.WeaponName == weaponName)
                {
                    weaponData = wData;
                }
            }

            if(weaponData.Index == 404)
            {
                Debug.LogWarning("Weapon not found, returning empty data");
                return new ShopStatData();
            }

            //Lookup Stat
            foreach (ShopStatData sData in weaponData.Stats)
            {
                if (sData.StatName == statName)
                {
                    return sData;
                }
            }

            Debug.LogWarning("Stat not found, returning empty data");
            return new ShopStatData();
        }
    }

    [System.Serializable]
    public struct ShopWeaponData
    {
        public string WeaponName;
        public List<ShopStatData> Stats;
        public int Index;

    }

    [System.Serializable]
    public struct ShopStatData
    {
        public string StatName;
        public float[] Values;
        public int[] UpgradeCost;
    }
}