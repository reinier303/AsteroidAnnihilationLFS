using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class PlayerAttack : MonoBehaviour
    {
        private InputManager inputManager;
        private ObjectPooler RObjectPooler;
        EquipmentManager equipmentManager;
        private Player rPlayer;
        private CompletionRewardStats completionRewardStats;
        private PlayerShipSettings playerShipSettings;

        private bool canFire;
        public float fireCooldown;

        private List<Vector2> weaponPositions;
        private Dictionary<int, WeaponData> currentWeaponDatas;
        private List<Weapon> currentWeapons;

        [SerializeField] private GameObject muzzleFlash;

        [SerializeField] private float playerVelocityMultiplier;

        private EventSystem eventSystem;

        private void Awake()
        {
            rPlayer = GetComponent<Player>();

            eventSystem = EventSystem.current;

            currentWeapons = new List<Weapon>();

            //TODO::Make this work for multiple ship types when starting work on that
        }

        private void Start()
        {
            playerShipSettings = SettingsManager.Instance.playerShipSettings;
            weaponPositions = playerShipSettings.GetWeaponPositions(EnumCollections.ShipType.Fighter);

            equipmentManager = EquipmentManager.Instance;
            RObjectPooler = ObjectPooler.Instance;
            inputManager = InputManager.Instance;
            completionRewardStats = CompletionRewardStats.Instance;

            Initialize();
        }

        private void Initialize()
        {
            canFire = true;
        }

        public void WeaponChanged()
        {
            currentWeaponDatas = equipmentManager.GetAllEquipedWeapons();
            currentWeapons.Clear();
            int weaponCount = 0;
            foreach (WeaponData wData in currentWeaponDatas.Values)
            {
                if (wData.WeaponType == EnumCollections.Weapons.None) { continue; }
                currentWeapons.Add(equipmentManager.GetWeapon(wData.WeaponType));
                currentWeapons[weaponCount].Initialize(rPlayer.RPlayerStats, equipmentManager);
                weaponCount++;
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
            //TEMP: Get input from inputManager
            if (inputManager.Attacking)
            {
                Fire(0);
            }
        }

        private void Fire(int mouseButton)
        {
            //Fix UI Check
            if (canFire && currentWeapons.Count > 0)
            {
                muzzleFlash.SetActive(false);
                muzzleFlash.SetActive(true);

                if(mouseButton == 0)
                {
                    int weaponIndex = 0;
                    foreach(Weapon weapon in currentWeapons)
                    {
                        currentWeapons[weaponIndex].Fire(RObjectPooler, transform, rPlayer.RPlayerMovement.MovementInput * playerVelocityMultiplier, weaponPositions[weaponIndex], weaponIndex);
                        weaponIndex++;
                    }
                    canFire = false;
                    StartCoroutine(FireCooldownTimer(mouseButton));
                }
            }
        }

        private IEnumerator FireCooldownTimer(int mouseButton)
        {
            if(mouseButton == 0)
            {
                yield return new WaitForSeconds(1 / GetCooldownAverage());
            }
            canFire = true;
        }

        //TODO::Create a way for weapons to fire independently based on their own firerate/alternating. Make this and average both options
        private float GetCooldownAverage()
        {
            float totalFirerate = 0;
            int weapons = 0;
            foreach(Weapon weapon in currentWeapons)
            {
                totalFirerate += currentWeapons[weapons].GetEquipmentStat(EnumCollections.EquipmentStats.FireRate, weapons);
                weapons++;
            }
            return totalFirerate / Mathf.Clamp(weapons, 1, 25);
        }
    }
}