using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AsteroidAnnihilation
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance { get; private set; }

        private PlayerAttack playerAttack;
        private InventoryManager inventoryManager;

        private Dictionary<EnumCollections.Weapons, Weapon> weaponTypesT1;

        private Dictionary<int, WeaponData> equipedWeapons;
        private Dictionary<EnumCollections.ItemType, EquipmentData> equipedGear;

        private GeneralItemSettings generalItemSettings;
        private PlayerShipSettings playerShipSettings;

        private int weaponAmount;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.onEndGame += SaveEquipment;
            generalItemSettings = SettingsManager.Instance.generalItemSettings;
            playerShipSettings = SettingsManager.Instance.playerShipSettings;
            playerAttack = Player.Instance.RPlayerAttack;
            inventoryManager = InventoryManager.Instance;
            
            InitializeWeapons();
            LoadEquipment();
            playerAttack.WeaponChanged();
        }

        private void InitializeWeapons()
        {
            weaponTypesT1 = new Dictionary<EnumCollections.Weapons, Weapon>();

            Object[] weapons = Resources.LoadAll("Weapons/WeaponsT1", typeof(Weapon));
            foreach (Weapon weapon in weapons)
            {
                if (weapon.EquipmentStatRanges == null) { Debug.LogWarning("WeaponStatRanges of " + weapon.name + " are not filled in"); continue; }
                weaponTypesT1.Add(weapon.WeaponType, weapon);
            }
            weaponAmount = playerShipSettings.GetWeaponPositions(EnumCollections.ShipType.Fighter).Count;
        }

        private void SaveEquipment()
        {
            ES3.Save("equipedWeapons", equipedWeapons);
            ES3.Save("equipedGear", equipedGear);
        }

        private void LoadEquipment()
        {
            if (!ES3.KeyExists("equipedWeapons"))
            {
                equipedWeapons = new Dictionary<int, WeaponData>();
                equipedWeapons.Add(0, generalItemSettings.startWeapon);
                for (int i = 1; i < weaponAmount; i++)
                {
                    equipedWeapons.Add(i, default);
                }
                equipedGear = generalItemSettings.startGear;
                SaveEquipment();
            }
            else
            {
                equipedWeapons = (Dictionary<int, WeaponData>)ES3.Load("equipedWeapons");
                equipedGear = (Dictionary<EnumCollections.ItemType, EquipmentData>)ES3.Load("equipedGear");
            }
        }

        public WeaponData GenerateWeapon(List<EnumCollections.Weapons> forcedWeaponTypes = null)
        {
            Weapon weapon;
            WeaponData weaponData = new WeaponData();

            if (forcedWeaponTypes == null)
            {
                List<Weapon> keyList = Enumerable.ToList(weaponTypesT1.Values);
                weapon = keyList[Random.Range(0, keyList.Count)];
            }
            else { weapon = weaponTypesT1[forcedWeaponTypes[Random.Range(0, forcedWeaponTypes.Count)]]; }

            //ItemData
            weaponData.EquipmentData.ItemData.Tier = weapon.Tier;
            weaponData.EquipmentData.ItemData.ItemName = weapon.GenerateName();
            weaponData.EquipmentData.ItemData.ItemType = weapon.Type;
            EnumCollections.Rarities rarity = weapon.GetRarity();
            weaponData.EquipmentData.ItemData.Rarity = rarity;
            weaponData.EquipmentData.ItemData.Icon = weapon.GetIcon();

            //EquipmentData
            weaponData.EquipmentData.EquipmentStats = new Dictionary<EnumCollections.EquipmentStats, float>();
            weaponData.EquipmentData.RarityStats = weapon.GetRarityStats(rarity, generalItemSettings);

            //WeaponData
            weaponData.WeaponType = weapon.WeaponType;
            weaponData.ProjectileType = weapon.ProjectileType;

            foreach (EnumCollections.EquipmentStats stat in weapon.EquipmentStatRanges.Keys)
            {
                float value = Random.Range(weapon.EquipmentStatRanges[stat].x, weapon.EquipmentStatRanges[stat].y);
                value = MathHelpers.RoundToDecimal(value, 2);
                weaponData.EquipmentData.EquipmentStats.Add(stat, value);
            }
            return weaponData;
        }

        public Weapon GetWeapon(EnumCollections.Weapons weaponType)
        {
            return weaponTypesT1[weaponType];
        }

        public WeaponData GetEquipedWeapon(int index)
        {
            return equipedWeapons[index];
        }

        public Dictionary<int, WeaponData> GetAllEquipedWeapons()
        {
            return equipedWeapons;
        }

        public EquipmentData GetGear(EnumCollections.ItemType gearType)
        {
            return equipedGear[gearType];
        }


        public (bool, WeaponData) ChangeWeapon(WeaponData weapon, int index = -1)
        {
            bool succes = false;
            WeaponData data = default;
            Debug.Log(equipedWeapons.Count);

            //CHECK REQUIREMENTS HERE and dont swap weapon if requirements not met
            if (index == -1 )
            {
                Debug.Log("index paramater not supplied");
                int emptyWeaponSlot = GetEmptyWeaponSlotIndex();
                if (emptyWeaponSlot != -1)
                {
                    equipedWeapons[GetEmptyWeaponSlotIndex()] = weapon;
                    succes = true;
                }
                else
                {
                    data = equipedWeapons[index];
                    equipedWeapons[index] = weapon;
                    //equipedWeapons[index] = weapon;
                    succes = true;
                }
            }
            //Case index parameter is supplied
            else
            {
                if (!equipedWeapons[index].Equals(default(WeaponData)))
                {
                    equipedWeapons[index] = weapon;
                    succes = true;
                }
                else
                {
                    data = equipedWeapons[index];
                    equipedWeapons[index] = weapon;
                    //equipedWeapons[index] = weapon;
                    succes = true;
                }
            }

            if (succes)
            {
                playerAttack.WeaponChanged();
                inventoryManager.InitializeWeapons();
                return (succes, data);
            } else { return (succes, default); }

        }

        private int GetEmptyWeaponSlotIndex()
        {
            for(int i = 0; i < equipedWeapons.Count; i++)
            {
                if (!equipedWeapons[i].Equals(default(WeaponData)))
                {
                    return i;
                }
            }
            return -1;
        }

        public void RemoveWeapon(int index)
        {
            inventoryManager.AddItem(equipedWeapons[index]);
            equipedWeapons[index] = default;
            playerAttack.WeaponChanged();
        }

        public void ChangeGear(EnumCollections.ItemType equipType, EquipmentData equipment)
        {
            if (equipedGear.ContainsKey(equipType))
            {
                equipedGear[equipType] = equipment;
            }
            inventoryManager.InitializeGear();
        }
    }

    public struct EquipmentData
    {
        public ItemData ItemData;
        public Dictionary<EnumCollections.EquipmentStats, float> EquipmentStats;
        public Dictionary<EnumCollections.EquipmentStats, float> RarityStats;

    }

    public struct WeaponData
    {
        public EquipmentData EquipmentData;
        public EnumCollections.Weapons WeaponType;
        public EnumCollections.PlayerProjectiles ProjectileType;
    }
}
