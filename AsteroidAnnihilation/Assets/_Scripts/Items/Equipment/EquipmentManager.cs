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

        private Dictionary<EnumCollections.Weapons, Weapon> weaponTypesT1;

        private List<WeaponData> equipedWeapons;
        private Dictionary<EnumCollections.ItemType, EquipmentData> equipedGear;

        public GeneralItemSettings generalItemSettings;

        private void Awake()
        {
            Instance = this;

            weaponTypesT1 = new Dictionary<EnumCollections.Weapons, Weapon>();

            equipedWeapons = new List<WeaponData>();

            Object[] weapons = Resources.LoadAll("Weapons/WeaponsT1", typeof(Weapon));
            foreach (Weapon weapon in weapons)
            {
                if(weapon.EquipmentStatRanges == null) {Debug.LogWarning("WeaponStatRanges of " + weapon.name + " are not filled in"); return; }
                weaponTypesT1.Add(weapon.WeaponType, weapon);
            }
        }

        private void Start()
        {
            generalItemSettings = SettingsManager.Instance.generalItemSettings;
            playerAttack = Player.Instance.RPlayerAttack;
            LoadEquipment();
            playerAttack.WeaponChanged();
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
                equipedWeapons = new List<WeaponData>();
                equipedWeapons.Add(generalItemSettings.startWeapon);
                equipedGear = generalItemSettings.startGear;
                SaveEquipment();
            }
            else
            {
                equipedWeapons = (List<WeaponData>)ES3.Load("equipedWeapons");
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

        public List<WeaponData> GetAllEquipedWeapons()
        {
            return equipedWeapons;
        }

        public void ChangeWeapon(int index, WeaponData weapon)
        {
            if (equipedWeapons.Count != 0 && equipedWeapons.Count >= index) { equipedWeapons[index] = weapon; }
            else { equipedWeapons.Add(weapon); }
            playerAttack.WeaponChanged();
        }

        public void ChangeEquipment(EnumCollections.ItemType equipType, EquipmentData equipment)
        {
            if (equipedGear.ContainsKey(equipType))
            {
                equipedGear[equipType] = equipment;
            }
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
