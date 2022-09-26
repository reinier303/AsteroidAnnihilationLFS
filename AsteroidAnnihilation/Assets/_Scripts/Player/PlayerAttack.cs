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

        private bool canFire;
        public float fireCooldown;

        private List<WeaponData> currentWeaponDatas;
        private List<Weapon> currentWeapons;

        [SerializeField] private GameObject muzzleFlash;

        [SerializeField] private float playerVelocityMultiplier;

        private EventSystem eventSystem;

        private void Awake()
        {
            rPlayer = GetComponent<Player>();

            eventSystem = EventSystem.current;

            currentWeapons = new List<Weapon>();
        }

        private void Start()
        {
            equipmentManager = EquipmentManager.Instance;
            RObjectPooler = ObjectPooler.Instance;
            inputManager = InputManager.Instance;
            completionRewardStats = CompletionRewardStats.Instance;

            Initialize();

        }

        private void Initialize()
        {
            canFire = true;
            //UIManager.Instance.ShowEquipmentTooltip(currentWeaponData);
        }

        public void WeaponChanged()
        {
            currentWeaponDatas = equipmentManager.GetAllEquipedWeapons();
            currentWeapons.Clear();
            int weaponCount = 0;
            foreach (WeaponData wData in currentWeaponDatas)
            {
                currentWeapons.Add(equipmentManager.GetWeapon(wData.WeaponType));
                currentWeapons[weaponCount].Initialize(rPlayer.RPlayerStats, wData.EquipmentData.EquipmentStats, wData.EquipmentData.RarityStats);
                weaponCount++;
            }
            Debug.Log(currentWeapons.Count);

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
            if (canFire)
            {
                muzzleFlash.SetActive(false);
                muzzleFlash.SetActive(true);

                if(mouseButton == 0)
                {
                    //TODO::Make this work for multiple
                    Debug.Log(currentWeapons[0]);
                    currentWeapons[0].Fire(RObjectPooler, transform, rPlayer.RPlayerMovement.MovementInput * playerVelocityMultiplier);
                    canFire = false;
                    StartCoroutine(FireCooldownTimer(mouseButton));
                }
            }
        }


        private IEnumerator FireCooldownTimer(int mouseButton)
        {
            if(mouseButton == 0)
            {
                yield return new WaitForSeconds(1 / currentWeapons[0].GetEquipmentStat(EnumCollections.EquipmentStats.FireRate));
            }
            canFire = true;
        }
    }
}