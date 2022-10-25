using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class PlayerAttack : SerializedMonoBehaviour
    {
        private InputManager inputManager;
        private ObjectPooler RObjectPooler;
        EquipmentManager equipmentManager;
        private Player rPlayer;
        private PlayerMovement playerMovement;
        private CompletionRewardStats completionRewardStats;
        private PlayerShipSettings playerShipSettings;
        private UIManager uiManager;

        private bool canFire;
        public float fireCooldown;
        public float MaxEnergy;
        private float currentEnergy;
        private float energyRegen;

        private List<Vector2> weaponPositions;
        [SerializeField] private Dictionary<int, WeaponData> currentWeaponDatas;
        [SerializeField] private Dictionary<int, Weapon> currentWeapons;

        [SerializeField] private float playerVelocityMultiplier;

        private EventSystem eventSystem;
        private Rigidbody2D rb;

        private void Awake()
        {
            rPlayer = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();

            eventSystem = EventSystem.current;

            currentWeapons = new Dictionary<int, Weapon>();

            //TODO::Make this work for multiple ship types when starting work on that
        }

        private void Start()
        {
            playerShipSettings = SettingsManager.Instance.playerShipSettings;
            weaponPositions = playerShipSettings.GetWeaponPositions(EnumCollections.ShipType.Fighter);

            equipmentManager = EquipmentManager.Instance;
            RObjectPooler = ObjectPooler.Instance;
            inputManager = InputManager.Instance;
            uiManager = UIManager.Instance;
            completionRewardStats = CompletionRewardStats.Instance;
            playerMovement = rPlayer.RPlayerMovement;

            Initialize();
        }

        private void Initialize()
        {
            canFire = true;
            StartCoroutine(RegenerateEnergy());
        }

        public void InitializeWeapons()
        {
            currentWeaponDatas = equipmentManager.GetAllEquipedWeapons();
            foreach (int index in currentWeaponDatas.Keys)
            {
                if (currentWeaponDatas[index].WeaponType == EnumCollections.Weapons.None)
                {
                    currentWeapons.Add(index, (Weapon)ScriptableObject.CreateInstance("Weapon"));
                    continue;
                }
                currentWeapons.Add(index, equipmentManager.GetWeapon(currentWeaponDatas[index].WeaponType));
                currentWeapons[index].Initialize(rPlayer.RPlayerStats, equipmentManager);
            }
        }

        public void GetEquipmentVariables()
        {
            MaxEnergy = equipmentManager.GetGearStatValue(EnumCollections.ItemType.EnergyCore, EnumCollections.EquipmentStats.EnergyCapacity);
            currentEnergy = MaxEnergy;
            energyRegen = equipmentManager.GetGearStatValue(EnumCollections.ItemType.EnergyCore, EnumCollections.EquipmentStats.EnergyRegen);
        }

        public void WeaponChanged()
        {
            currentWeaponDatas = equipmentManager.GetAllEquipedWeapons();
            foreach (int index in currentWeaponDatas.Keys)
            {
                if (currentWeaponDatas[index].WeaponType == EnumCollections.Weapons.None) 
                {
                    currentWeapons[index] = (Weapon)ScriptableObject.CreateInstance("Weapon");
                    continue; 
                }
                currentWeapons[index] = equipmentManager.GetWeapon(currentWeaponDatas[index].WeaponType);
                currentWeapons[index].Initialize(rPlayer.RPlayerStats, equipmentManager);
            }
        }

        //Old SortWeapons() might use/recycle later
        /*
        public void SortWeapons()
        {

            //Sort by ID for weapon swapping
            Weapons = Weapons.OrderBy(weapon => weapon.WeaponIndex).ToList();

            for (int i = 0; i < Weapons.Count; i++)
            {
                if (!Weapons[i].Unlocked)
                {
                    Weapon weapon = Weapons[i];
                    Weapons.RemoveAt(i);
                    Weapons.Insert(Weapons.Count - 1, weapon);
                }
            }
            /*
            foreach (Weapon weapon in Weapons)
            {
                Debug.Log(weapon.WeaponName + Weapons.IndexOf(weapon));
            }
            
        }
        */

        // Update is called once per frame
        private void Update()
        {
            if(Time.timeScale == 0)
            {
                return;
            }
            if (inputManager.Attacking && !uiManager.MouseOverUI)
            {
                Fire(0);

            }
        }

        private void Fire(int mouseButton)
        {
            //Fix UI Check
            if (canFire && HasWeapon() && CanApplyEnergyCost())
            {
                if(mouseButton == 0)
                {
                    Vector2 addedPlayerVelocity = playerMovement.MovementInput * playerVelocityMultiplier * playerMovement.GetCurrentSpeed();
                    for (int i = 0; i < currentWeapons.Count; i++)
                    {
                        if (currentWeapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                        rb.AddForce(-transform.up * 250);
                        currentWeapons[i].Fire(RObjectPooler, transform, addedPlayerVelocity, weaponPositions[i], i);
                    }
                    canFire = false;
                    StartCoroutine(FireCooldownTimer(mouseButton));
                }
            }
        }

        private bool HasWeapon()
        {
            bool hasWeapon = false;
            for(int i = 0; i < currentWeapons.Count; i++)
            {
                if (currentWeapons[i].WeaponType != EnumCollections.Weapons.None) { hasWeapon = true; break; }
            }
            return hasWeapon;
        }

        private IEnumerator FireCooldownTimer(int mouseButton)
        {
            if(mouseButton == 0)
            {
                yield return new WaitForSeconds(1 / GetFireRateAverage());
            }            
            canFire = true;
        }

        //TODO::Create a way for weapons to fire independently based on their own firerate/alternating. Make this and average both options
        private float GetFireRateAverage()
        {
            float totalFirerate = 0;
            int weapons = 0;
            for (int i = 0; i < currentWeapons.Count; i++)
            {
                if (currentWeapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                float coreFireRate = equipmentManager.GetGearStatValue(EnumCollections.ItemType.EnergyCore, EnumCollections.EquipmentStats.FireRate);
                totalFirerate += (currentWeapons[i].GetEquipmentStat(EnumCollections.EquipmentStats.FireRate, i) + coreFireRate);
                weapons++;
            }
            return totalFirerate / Mathf.Clamp(weapons, 1, 25);
        }

        private bool CanApplyEnergyCost()
        {
            float cost = 0;
            for (int i = 0; i < currentWeapons.Count; i++)
            {
                if (currentWeapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                cost += currentWeapons[i].GetEquipmentStat(EnumCollections.EquipmentStats.EnergyPerShot, i);
            }
            if (currentEnergy >= cost) 
            {               
                currentEnergy -= cost;
                return true;
            } else { return false; }
        }

        private IEnumerator RegenerateEnergy()
        {
            if (currentEnergy < MaxEnergy) { currentEnergy += energyRegen / 30f; }
            else { currentEnergy = MaxEnergy; }
            uiManager.UpdateEnergy(currentEnergy, MaxEnergy);
            yield return new WaitForSeconds(1f/ 30f);
            StartCoroutine(RegenerateEnergy());
        }
    }
}