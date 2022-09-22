using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation 
{
    public class WeaponStatPowerup : Powerup
    {
        /*
        public float Multiplier;
        public float Duration;

        [SerializeField] private AudioSource soundEffect;

        [SerializeField] private string statName;
        [SerializeField] private string UIName;

        private PlayerAttack playerAttack;

        private GameManager gameManager;
        private UIManager uIManager;
        private PowerupManager powerupManager;

        private float startValue;

        private Collider2D powerupCollider;
        private Transform poolParent;
        private Vector2 startScale;

        private void Awake()
        {
            powerupCollider = GetComponent<Collider2D>();
            soundEffect = GetComponent<AudioSource>();
            poolParent = transform.parent;
            startScale = transform.localScale;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uIManager = UIManager.Instance;
            powerupManager = PowerupManager.Instance;

            playerAttack = gameManager.RPlayer.RPlayerAttack;
            startValue = playerAttack.GetCurrentWeapon().WeaponStatDictionary[statName].Multiplier;
        }

        private void OnEnable()
        {
            if(playerAttack != null)
            {
                startValue = playerAttack.GetCurrentWeapon().WeaponStatDictionary[statName].Multiplier;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                powerupManager.AddWeaponPowerup(this);
                powerupCollider.enabled = false;
            }
        }

        public override IEnumerator ApplyPowerup()
        {
            //Enable Powerup
            playerAttack.GetCurrentWeapon().WeaponStatDictionary[statName].Multiplier = startValue * Multiplier;

            EnableVFX();
            soundEffect.Play();

            uIManager.StartPowerupTimer(Duration, UIName);

            yield return new WaitForSeconds(Duration);

            DisablePowerup();
        }

        public void DisablePowerup()
        {
            //Disable Powerup
            ReturnToBaseValue();
            powerupManager.RemoveWeaponPowerup(this);

            DisableVFX();

            gameObject.SetActive(false);
        }

        public void EnableVFX()
        {
            transform.parent = playerAttack.transform;
            transform.localPosition = new Vector2(0, 0);
            transform.localScale *= 1.5f;
        }

        public void DisableVFX()
        {
            transform.parent = poolParent;
            transform.localScale = startScale;
            powerupCollider.enabled = false;
        }

        public void ReturnToBaseValue()
        {
            playerAttack.GetCurrentWeapon().WeaponStatDictionary[statName].Multiplier = startValue;
        }
        */
    }
}