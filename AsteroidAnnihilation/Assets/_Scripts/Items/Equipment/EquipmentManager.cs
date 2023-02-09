using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class EquipmentManager : SerializedMonoBehaviour
    {
        public static EquipmentManager Instance { get; private set; }

        private StatManager statManager;
        private SettingsManager settingsManager;
        private PlayerAttack playerAttack;
        private InventoryManager inventoryManager;
        private PlayerEntity playerEntity;
        private PlayerMovement playerMovement;

        [SerializeField] private Dictionary<EnumCollections.Weapons, Weapon> weaponTypesT1;
        [SerializeField] private Dictionary<EnumCollections.ItemType, Equipment> equipmentTypesT1;

        [SerializeField]private Dictionary<int, WeaponData> equipedWeapons;
        [SerializeField]private Dictionary<EnumCollections.ItemType, EquipmentData> equipedGear;

        private GeneralItemSettings generalItemSettings;
        private PlayerShipSettings playerShipSettings;

        private int weaponAmount;
        public bool TutorialDone;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.onEndGame += SaveEquipment;
            inventoryManager = InventoryManager.Instance;
            playerEntity.SetHealthToMax();
        }

        public void InitializeEquipment()
        {
            Player player = Player.Instance;
            statManager = StatManager.Instance;
            playerAttack = player.RPlayerAttack;
            playerEntity = player.RPlayerEntity;
            settingsManager = SettingsManager.Instance;
            playerMovement = player.RPlayerMovement;
            generalItemSettings = settingsManager.generalItemSettings;
            playerShipSettings = settingsManager.playerShipSettings;
            LoadEquipment();
            InitializeEquipmentGeneration();
            playerAttack.InitializeWeapons();
            SetEquipmentStats();
        }

        public void SetEquipmentStats()
        {
            statManager.OnOffensiveStatsChanged();
            statManager.OnDefensiveStatsChanged();
        }

        private void InitializeEquipmentGeneration()
        {
            weaponTypesT1 = new Dictionary<EnumCollections.Weapons, Weapon>();

            Object[] weapons = Resources.LoadAll("Weapons/WeaponsT1", typeof(Weapon));
            foreach (Weapon weapon in weapons)
            {
                if (weapon.EquipmentStatRanges == null) { Debug.LogWarning("WeaponStatRanges of " + weapon.name + " are not filled in"); continue; }
                weaponTypesT1.Add(weapon.WeaponType, weapon);
            }

            equipmentTypesT1 = new Dictionary<EnumCollections.ItemType, Equipment>();

            Object[] equipment = Resources.LoadAll("Equipment/EquipmentT1", typeof(Equipment));
            foreach (Equipment equip in equipment)
            {
                if (equip.EquipmentStatRanges == null) { Debug.LogWarning("WeaponStatRanges of " + equip.name + " are not filled in"); continue; }
                if (!equipmentTypesT1.ContainsKey(equip.ItemType)) { equipmentTypesT1.Add(equip.ItemType, equip); }//TODO::REMOVE THIS CHECK UPON ADDING EQUIPMENT SUBTYPES
            }
        }

        private void SaveEquipment()
        {
            ES3.Save("equipedWeapons", equipedWeapons);
            ES3.Save("equipedGear", equipedGear);
        }

        private void LoadEquipment()
        {
            weaponAmount = playerShipSettings.GetWeaponPositions(EnumCollections.ShipType.Fighter).Count;
            if (!ES3.KeyExists("equipedWeapons"))
            {
                equipedWeapons = new Dictionary<int, WeaponData>();
                for (int i = 0; i < weaponAmount; i++)
                {
                    equipedWeapons.Add(i, default);
                }
                equipedWeapons[0] = generalItemSettings.startWeapon;

                equipedGear = new Dictionary<EnumCollections.ItemType, EquipmentData>();
                foreach(EquipmentData equipData in generalItemSettings.startGear.Values)
                {
                    equipedGear.Add(equipData.ItemData.ItemType,equipData);
                }

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
            weaponData.EquipmentData.ItemData.ItemType = weapon.ItemType;
            EnumCollections.Rarities rarity = weapon.GetRarity();
            weaponData.EquipmentData.ItemData.Rarity = rarity;
            weaponData.EquipmentData.ItemData.Icon = weapon.GetIcon();

            //EquipmentData
            weaponData.EquipmentData.EquipmentStats = new Dictionary<EnumCollections.Stats, float>();
            weaponData.EquipmentData.RarityStats = weapon.GetRarityStats(rarity, generalItemSettings);

            //WeaponData
            weaponData.WeaponType = weapon.WeaponType;
            weaponData.ProjectileType = weapon.ProjectileType;

            foreach (EnumCollections.Stats stat in weapon.EquipmentStatRanges.Keys)
            {
                float value = Random.Range(weapon.EquipmentStatRanges[stat].x, weapon.EquipmentStatRanges[stat].y);
                value = MathHelpers.RoundToDecimal(value, 2);
                weaponData.EquipmentData.EquipmentStats.Add(stat, value);
            }
            return weaponData;
        }
        
        public EquipmentData GenerateEquipment(List<EnumCollections.ItemType> forcedEquipTypes = null)
        {
            Equipment equipment;
            EquipmentData equipmentData = new EquipmentData();

            if (forcedEquipTypes == null)
            {
                List<Equipment> keyList = Enumerable.ToList(equipmentTypesT1.Values);
                equipment = keyList[Random.Range(0, keyList.Count)];
            }
            else { equipment = equipmentTypesT1[forcedEquipTypes[Random.Range(0, forcedEquipTypes.Count)]]; }

            //ItemData
            equipmentData.ItemData.Tier = equipment.Tier;
            equipmentData.ItemData.ItemName = equipment.GenerateName();
            equipmentData.ItemData.ItemType = equipment.ItemType;
            EnumCollections.Rarities rarity = equipment.GetRarity();
            equipmentData.ItemData.Rarity = rarity;
            equipmentData.ItemData.Icon = equipment.GetIcon();

            //EquipmentData
            equipmentData.EquipmentStats = new Dictionary<EnumCollections.Stats, float>();
            equipmentData.RarityStats = equipment.GetRarityStats(rarity, generalItemSettings);
            equipmentData.ItemData.ItemType = equipment.ItemType;

            foreach (EnumCollections.Stats stat in equipment.EquipmentStatRanges.Keys)
            {
                float value = Random.Range(equipment.EquipmentStatRanges[stat].x, equipment.EquipmentStatRanges[stat].y);
                value = MathHelpers.RoundToDecimal(value, 3);
                equipmentData.EquipmentStats.Add(stat, value);
            }
            return equipmentData;
        }

        public ItemData GenerateItemData(Item item)
        {
            ItemData itemData = new ItemData();
            itemData.ItemName = item.ItemName;
            itemData.Tier = item.Tier;
            itemData.Rarity = item.GetRarity();
            itemData.ItemType = item.ItemType;
            itemData.Icon = item.GetIcon();
            itemData.Amount = 0;
            return itemData;
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
            if(equipedGear[gearType].ItemData.ItemName != null)
            {
                return equipedGear[gearType];
            }
            else { return default(EquipmentData); }
        }

        public float GetGearStatValue(EnumCollections.ItemType gearType, EnumCollections.Stats statType)
        {
            if (equipedGear[gearType].EquipmentStats == null)
            {
                return 0;
            }
            float baseValue = 0;
            float rarityValue = 0;

            if (equipedGear[gearType].EquipmentStats.ContainsKey(statType))
            {
                baseValue = equipedGear[gearType].EquipmentStats[statType];
            }
            if(equipedGear[gearType].RarityStats.ContainsKey(statType))
            {
                rarityValue = equipedGear[gearType].RarityStats[statType];
            }
            return baseValue + rarityValue;
        }


        public (bool, EquipmentData) ChangeGear(EquipmentData equip)
        {
            bool succes = false;
            //EquipmentData data = default;

            /*
            //int emptyWeaponSlot = GetEmptyWeaponSlotIndex();
            if (emptyWeaponSlot != -1)
            {
                equipedGear[equip.ItemData.ItemType] = equip;
                succes = true;
            }
            else
            {*/
                EquipmentData data = equipedGear[equip.ItemData.ItemType];
                equipedGear[equip.ItemData.ItemType] = equip;
                //equipedWeapons[index] = weapon;
                succes = true;
            //}
            /*
            //Case index parameter is supplied
            else
            {
                if (!equipedWeapons[index].Equals(default(EquipmentData)))
                {
                    data = equipedGear[equip.ItemData.ItemType];
                    equipedGear[equip.ItemData.ItemType] = equip;
                    succes = true;
                }
                else
                {
                    equipedGear[equip.ItemData.ItemType] = equip;
                    //equipedWeapons[index] = weapon;
                    succes = true;
                }
            }
            */
            if (succes)
            {
                CheckTutorialCompleted();
                statManager.OnOffensiveStatsChanged();
                statManager.OnDefensiveStatsChanged();
                playerMovement.GetMovementVariables();

                inventoryManager.InitializeGear();
                return (succes, data);
            }
            else { return (succes, default); }
        }

        public (bool, WeaponData) ChangeWeapon(WeaponData weapon, int index = -1)
        {
            bool succes = false;
            WeaponData data = default;

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
                    data = equipedWeapons[index];
                    equipedWeapons[index] = weapon;
                    succes = true;
                }
                else
                {
                    equipedWeapons[index] = weapon;
                    //equipedWeapons[index] = weapon;
                    succes = true;
                }
            }

            if (succes)
            {
                Debug.Log("asd");
                playerAttack.WeaponChanged();
                inventoryManager.InitializeWeapons();
                CheckTutorialCompleted();
                return (succes, data);
            } else { return (succes, default); }
        }

        private int GetEmptyWeaponSlotIndex()
        {
            for(int i = 0; i < equipedWeapons.Count; i++)
            {
                if (!equipedWeapons[i].Equals(default(WeaponData)))
                {
                    Debug.Log(i);
                    return i;
                }
            }
            return -1;
        }

        public void MoveGearToInventory(EquipmentData data)
        {
            inventoryManager.AddItem(data);
            RemoveGear(data);
        }

        public void RemoveGear(EquipmentData data)
        {
            equipedGear[data.ItemData.ItemType] = default;
            statManager.OnOffensiveStatsChanged();
            statManager.OnDefensiveStatsChanged();
            playerMovement.GetMovementVariables();
        }

        public void MoveWeaponToInventory(int index)
        {
            inventoryManager.AddItem(equipedWeapons[index]);
            RemoveWeapon(index);
        }

        public WeaponData RemoveWeapon(int index)
        {
            WeaponData data;
            data = equipedWeapons[index];
            equipedWeapons[index] = default;
            playerAttack.WeaponChanged();
            return data;
        }

        public void ChangeGear(EnumCollections.ItemType equipType, EquipmentData equipment)
        {
            if (equipedGear.ContainsKey(equipType))
            {
                CheckTutorialCompleted();
                equipedGear[equipType] = equipment;
            }
            inventoryManager.InitializeGear();
        }

        private void CheckTutorialCompleted()
        {
            if (ES3.KeyExists("tutorialDone")) { return; }
                if (!TutorialDone) 
            {
                TutorialDone = true;
                TutorialManager.Instance.ShowNextTutorialMessage();
            }
        }
    }

    public struct EquipmentData
    {
        public ItemData ItemData;
        public Dictionary<EnumCollections.Stats, float> EquipmentStats;
        public Dictionary<EnumCollections.Stats, float> RarityStats;

    }

    public struct WeaponData
    {
        public EquipmentData EquipmentData;
        public EnumCollections.Weapons WeaponType;
        public EnumCollections.PlayerProjectiles ProjectileType;
    }
}
