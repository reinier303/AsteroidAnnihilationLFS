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
        private Player rPlayer;
        private CompletionRewardStats completionRewardStats;

        private bool canFire;
        public float fireCooldown;

        public List<Weapon> Weapons;

        [SerializeField] private Weapon currentWeapon;

        [SerializeField] private GameObject muzzleFlash;

        [SerializeField] private float playerVelocityMultiplier;

        private EventSystem eventSystem;

        private void Awake()
        {
            rPlayer = GetComponent<Player>();

            eventSystem = EventSystem.current;
        }

        private void Start()
        {
            InitializeWeaponsFromResources();

            Initialize();

            RObjectPooler = ObjectPooler.Instance;
            inputManager = InputManager.Instance;
            completionRewardStats = CompletionRewardStats.Instance;
        }

        private void Initialize()
        {
            canFire = true;
            currentWeapon = Weapons[0];
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public void SetCurrentWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
        }

        public void SetCurrentWeaponStats(Dictionary<string, UpgradableStat> weaponStats)
        {
            currentWeapon.WeaponStatDictionary = weaponStats;
        }

        public void UnlockWeapon(string weaponName)
        {
            foreach(Weapon weapon in Weapons)
            {
                if(weapon.WeaponName == weaponName)
                {
                    weapon.Unlocked = true;
                }
            }
            SortWeapons();
        }

        public bool ChangeWeapon(int index)
        {
            if(index + 1 > Weapons.Count)
            {
                Debug.Log("Weapon with index:" + index + " not set");
                return false;
            }
            if(Weapons[index].Unlocked)
            {
                SetCurrentWeapon(Weapons[index]);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void InitializeWeaponsFromResources()
        {
            Object[] ResourceWeapons = Resources.LoadAll("Weapons", typeof(Weapon));
            foreach (Weapon weapon in ResourceWeapons)
            {
                weapon.Initialize(rPlayer.RPlayerStats);
                Weapons.Add(weapon);
            }

            SortWeapons();
        }

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
            */
        }

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
                    currentWeapon.Fire(RObjectPooler, transform, rPlayer.RPlayerMovement.MovementInput * playerVelocityMultiplier);
                    canFire = false;
                    StartCoroutine(FireCooldownTimer(mouseButton));
                }
            }
        }


        private IEnumerator FireCooldownTimer(int mouseButton)
        {
            if(mouseButton == 0)
            {
                yield return new WaitForSeconds(1 / currentWeapon.WeaponStatDictionary["FireRate"].GetValue(completionRewardStats));
            }
            canFire = true;
        }
    }
}