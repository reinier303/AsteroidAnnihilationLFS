using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using static AsteroidAnnihilation.EnumCollections;

namespace AsteroidAnnihilation
{
    public class PlayerAttack : SerializedMonoBehaviour
    {
        private StatManager statManager;
        private InputManager inputManager;
        private AudioManager audioManager;
        private ObjectPooler RObjectPooler;
        EquipmentManager equipmentManager;
        private Player rPlayer;
        private PlayerMovement playerMovement;
        private CompletionRewardStats completionRewardStats;
        private PlayerShipSettings playerShipSettings;
        private UIManager uiManager;

        public bool canFire;
        public float fireCooldown;
        public float MaxEnergy;
        private float currentEnergy;
        private float energyRegen;
        private int mouseButton;

        private List<Vector2> weaponPositions;
        [SerializeField] private Dictionary<int, WeaponData> currentWeaponDatas;
        [SerializeField] private Dictionary<int, Weapon> currentWeapons;

        [SerializeField] private float playerVelocityMultiplier;

        private EventSystem eventSystem;
        private Rigidbody2D rb;
        private bool energyInitialized;

        private void Awake()
        {
            rPlayer = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();

            eventSystem = EventSystem.current;

            currentWeapons = new Dictionary<int, Weapon>();
        }

        private void Start()
        {
            playerShipSettings = SettingsManager.Instance.playerShipSettings;
            weaponPositions = playerShipSettings.GetWeaponPositions(EnumCollections.ShipType.Fighter);

            statManager = StatManager.Instance;
            statManager.OnOffensiveStatsChanged += GetEquipmentVariables;
            audioManager = AudioManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            RObjectPooler = ObjectPooler.Instance;
            inputManager = InputManager.Instance;
            uiManager = UIManager.Instance;
            completionRewardStats = CompletionRewardStats.Instance;
            playerMovement = rPlayer.RPlayerMovement;
            Initialize();
        }

        public void ResetEnergy()
        {
            currentEnergy = MaxEnergy;
            StartCoroutine(RegenerateEnergy());
        }

        private void Initialize()
        {
            canFire = true;
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
            MaxEnergy = statManager.GetStat(EnumCollections.Stats.EnergyCapacity);
            energyRegen = statManager.GetStat(EnumCollections.Stats.EnergyRegen);

            if (!energyInitialized) 
            {
                ResetEnergy();
                energyInitialized = true;
            }
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
                if(Input.GetMouseButton(1))
                {
                    mouseButton = 1;
                }
                else if(Input.GetMouseButton(0))
                {
                    mouseButton = 0;
                }
                Fire(mouseButton);
            }
        }

        private void Fire(int buttonDown)
        {
            //Fix UI Check
            if (canFire && HasWeapon() && CanApplyEnergyCost())
            {
                if(buttonDown == 0)
                {
                    Vector2 addedPlayerVelocity = playerMovement.MovementInput * playerVelocityMultiplier * playerMovement.GetMovementSpeed();
                    //TODO::Add this to weapon to make this different on weapon basis.
                    rb.AddForce(-transform.up * 100);
                    audioManager.DampenNonShotMixers();
                    //Temp
                    for (int i = 0; i < currentWeapons.Count; i++)
                    {
                        if (currentWeapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                        //Todo::Move this to weapon script.
                        audioManager.PlayAudio("PlasmaGunShot");
                        currentWeapons[i].Fire(RObjectPooler, transform, addedPlayerVelocity, weaponPositions[i], i);
                    }
                    canFire = false;
                    StartCoroutine(FireCooldownTimer(buttonDown));               
                }
                if (buttonDown == 1 && CanApplyEnergyCost(2.5f))
                {
                    Vector2 addedPlayerVelocity = playerMovement.MovementInput * playerVelocityMultiplier * playerMovement.GetMovementSpeed();
                    //TODO::Add this to weapon to make this different on weapon basis.
                    rb.AddForce(-transform.up * 100 * 2);
                    audioManager.DampenNonShotMixers();
                    //Temp
                    for (int i = 0; i < currentWeapons.Count; i++)
                    {
                        if (currentWeapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                        //Todo::Move this to weapon script.
                        audioManager.PlayAudio("PlasmaGunShot");
                        currentWeapons[i].Fire2nd(RObjectPooler, transform, addedPlayerVelocity, weaponPositions[i], i);
                    }
                    canFire = false;
                    StartCoroutine(FireCooldownTimer(buttonDown));
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
            float cooldown = 1 / GetFireRateAverage();
            if (mouseButton == 1) { cooldown *= 0.5f; }
            yield return new WaitForSeconds(cooldown);         
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
                float coreFireRate = equipmentManager.GetGearStatValue(EnumCollections.ItemType.EnergyCore, EnumCollections.Stats.FireRate);
                totalFirerate += (currentWeapons[i].GetEquipmentStat(EnumCollections.Stats.FireRate, i) + coreFireRate);
                weapons++;
            }
            return totalFirerate / Mathf.Clamp(weapons, 1, 25);
        }

        private bool CanApplyEnergyCost(float multiplier = 1)
        {
            float cost = 0;
            for (int i = 0; i < currentWeapons.Count; i++)
            {
                if (currentWeapons[i].WeaponType == EnumCollections.Weapons.None) { continue; }
                cost += currentWeapons[i].GetEquipmentStat(EnumCollections.Stats.EnergyPerShot, i);
            }
            cost *= (1 - rPlayer.RPlayerStats.GetSkillsValue(EnumCollections.Stats.EnergyEfficiency));
            if (currentEnergy >= cost) 
            {               
                currentEnergy -= cost * multiplier;
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