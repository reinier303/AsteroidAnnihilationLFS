using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class PowerupManager : SerializedMonoBehaviour
    {
        /*
        public static PowerupManager Instance;

        public List<WeightedPowerup> Powerups;

        public float PowerupChance;

        private List<string> WeightedPowerupList;

        private Coroutine currentPowerupCoroutine;
        private WeaponStatPowerup currentPowerup;

        private List<WeaponStatPowerup> WeaponPowerupsActive;

        private void Awake()
        {
            Instance = this;
            WeaponPowerupsActive = new List<WeaponStatPowerup>();
        }

        private void Start()
        {
            InitializePowerupList();
        }

        private void InitializePowerupList()
        {
            WeightedPowerupList = new List<string>();

            foreach(WeightedPowerup powerup in Powerups)
            {
                for(int i = 0; i < powerup.Weight; i++)
                {
                    WeightedPowerupList.Add(powerup.Name);
                }
            }
        }

        public string RandomPowerup()
        {
            return WeightedPowerupList[Random.Range(0, WeightedPowerupList.Count)];
        }
        /*
        public void ActivateWeaponPowerup(WeaponStatPowerup weaponStatPowerup)
        {
            StopCoroutine(currentPowerup);
            currentPowerup = StartCoroutine(weaponStatPowerup.ApplyPowerup());
        }
        

        public void AddWeaponPowerup(WeaponStatPowerup powerUp)
        {
            if(currentPowerupCoroutine != null)
            {
                StopCoroutine(currentPowerupCoroutine);
                currentPowerup.DisablePowerup();
            }
            WeaponPowerupsActive.Add(powerUp);
            currentPowerup = powerUp;
            currentPowerupCoroutine = StartCoroutine(powerUp.ApplyPowerup());
        }

        public void RemoveWeaponPowerup(WeaponStatPowerup powerUp)
        {
            WeaponPowerupsActive.Remove(powerUp);
        }

        public void DisableAllPowerups()
        {
            foreach (WeaponStatPowerup powerUp in WeaponPowerupsActive)
            {
                Debug.Log(powerUp.Multiplier);
                powerUp.ReturnToBaseValue();
            }
        }
        */
    }

    [System.Serializable]
    public class WeightedPowerup
    {
        public string Name;
        public int Weight;
    }

}

