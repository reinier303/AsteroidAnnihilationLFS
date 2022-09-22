using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsteroidAnnihilation
{
    public class BaseEntity : MonoBehaviour
    {
        [Header("Stats")]
        public float MaxHealth;
        public float currentHealth;

        [Header("Screen Shake")]
        public float ShakeMagnitude;
        public float ShakeDuration;

        [Header("Audio")]
        public string HitSound;
        public string DeathSound;

        [Header("Particle Effect")]
        public string ParticleEffect;
        public float ParticleEffectScale;

        [Header("Permanence Parts")]
        public List<Sprite> PermanenceSprites;
        public Vector2 PermanencePartMinMaxAmount;
        public float PermanencePartOutwardsPower;
        public float PermanenceScaleFactor;

        [Header("Drops")]
        public List<DropTableEntry> Drops;

        public System.Action<float, bool> OnTakeDamage;

        protected GameManager gameManager;
        protected ObjectPooler objectPooler;
        protected CameraManager cameraManager;
        protected PowerupManager powerupManager;
        protected MissionManager missionManager;
        protected PlayerStats playerStats;

        protected Material onHitMaterial;
        protected Material baseMaterial;

        protected SpriteRenderer spriteRenderer;

        [HideInInspector] public bool isDead;
        [HideInInspector] public bool isInitialized;

        public int KilledByIndex;

        protected virtual void Awake()
        {
            OnTakeDamage += TakeDamage;

            onHitMaterial = (Material)Resources.Load("Materials/FlashWhite", typeof(Material));
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            baseMaterial = spriteRenderer.material;

            InitializeDropPool();
        }

        private void InitializeDropPool()
        {
            //Get drops from resource database
        }

        protected virtual void Start()
        {
            cameraManager = GameManager.Instance.RCameraManager;
            gameManager = GameManager.Instance;
            objectPooler = ObjectPooler.Instance;
            missionManager = MissionManager.Instance;
            playerStats = gameManager.RPlayer.RPlayerStats;
        }

        protected virtual void OnEnable()
        {
            gameManager = GameManager.Instance;
            isDead = false;
            currentHealth = MaxHealth;
            isInitialized = true;
            spriteRenderer.material = baseMaterial;
        }

        public virtual void TakeDamage(float damage, bool isCrit)
        {
            damage *= Random.Range(0.75f, 1.25f);
            damage = Mathf.RoundToInt(damage);
            currentHealth -= damage;

            Vector3 offset = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            if (isCrit)
            {
                //ADD OFSET TO FOLLOW
                GameObject damagePopUp = objectPooler.SpawnFromPool("CritPopUp", transform.position + offset, Quaternion.identity);
                damagePopUp.GetComponent<DamagePopUp>().FollowDamagedObject(transform, offset, damage);
            }
            else
            {
                GameObject damagePopUp = objectPooler.SpawnFromPool("DamagePopUp", transform.position, Quaternion.identity);
                damagePopUp.GetComponent<DamagePopUp>().FollowDamagedObject(transform, offset, damage);
            }

            if (currentHealth <= 0)
            {
                isDead = true;
                currentHealth = 0;
                Die();
            }
            else
            {
                StartCoroutine(FlashWhite());
            }
        }

        protected IEnumerator FlashWhite()
        {
            spriteRenderer.material = onHitMaterial;
            yield return new WaitForSeconds(0.03f);
            spriteRenderer.material = baseMaterial;
        }

        protected virtual void Die()
        {
            SpawnSegments();
            SpawnParticleEffect();
            DropPickUps();
            missionManager.AddObjectiveProgress(MissionObjectiveType.Elimination);
            cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
            gameObject.SetActive(false);
        }

        protected virtual void SpawnParticleEffect()
        {
            GameObject effect = objectPooler.SpawnFromPool(ParticleEffect, transform.position, Quaternion.identity);
            effect.transform.localScale *= ParticleEffectScale;
        }

        protected virtual void SpawnSegments()
        {
            for (int i = 0; i < Random.Range(PermanencePartMinMaxAmount.x, PermanencePartMinMaxAmount.y); i++)
            {
                GameObject permanencePart = objectPooler.SpawnFromPool("PermanencePart", transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                permanencePart.transform.localScale = transform.localScale;
                permanencePart.GetComponent<PermanencePart>().InitializePart(PermanenceSprites, PermanencePartOutwardsPower, PermanenceScaleFactor);
            }
        }

        protected virtual void DropPickUps()
        {
            for (int i = 0; i < Drops.Count; i++)
            {
                if (Random.Range(0.0f, 100.0f) < Drops[i].DropChance && Drops[i].PickUp != null)
                {
                    GameObject pickUp = objectPooler.SpawnFromPool(Drops[i].PickUp.PickUpName, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    pickUp.GetComponent<PickUp>().InitializePickUp(Drops[i].PickUp.PickUpName);
                }
            }
        }

        //Old DropPowerUps() method, might use/recycle later
        /*
        protected virtual void DropPowerUps()
        {
            string powerupName = powerupManager.RandomPowerup();
            if(Random.Range(0,100) < powerupManager.PowerupChance)
            {
                objectPooler.SpawnFromPool(powerupName, transform.position, Quaternion.identity);
            }
        }
        */
    }


    [System.Serializable]
    public struct DropTableEntry
    {
        public PickUpSO PickUp;
        public float DropChance;
    }
}