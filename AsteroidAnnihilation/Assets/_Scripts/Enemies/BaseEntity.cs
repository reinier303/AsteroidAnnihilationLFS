using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class BaseEntity : MonoBehaviour
    {
        public List<Sprite> Sprites;

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
        public EnumCollections.ExplosionFX ParticleEffect;
        protected string particleEffectName;
        public float ParticleEffectScale;

        [Header("Permanence Parts")]
        public EnumCollections.PermanenceSprites PermanenceSprites;
        protected List<Sprite> permanenceSpriteList;
        public Vector2 PermanencePartMinMaxAmount;
        public float PermanencePartOutwardsPower;
        public float PermanenceScaleFactor;

        [Header("Drops")]
        public DropTable DropTable;

        public System.Action<float, bool> OnTakeDamage;

        protected GameManager gameManager;
        protected ObjectPooler objectPooler;
        protected CameraManager cameraManager;
        protected PowerupManager powerupManager;
        protected MissionManager missionManager;
        protected PlayerStats playerStats;

        protected Material onHitMaterial;
        protected Material baseMaterial;

        [SerializeField] protected SpriteRenderer spriteRenderer;

        [HideInInspector] public bool isDead;
        [HideInInspector] public bool isInitialized;

        public int KilledByIndex;

        public bool Aggro;

        protected virtual void Awake()
        {
            isDead = false;

            OnTakeDamage += TakeDamage;

            onHitMaterial = (Material)Resources.Load("Materials/FlashWhite", typeof(Material));
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if(Sprites.Count > 0)
            {
                spriteRenderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
            }
            baseMaterial = spriteRenderer.material;
        }

        protected virtual void Start()
        {
            cameraManager = GameManager.Instance.RCameraManager;
            gameManager = GameManager.Instance;
            objectPooler = ObjectPooler.Instance;
            missionManager = MissionManager.Instance;
            SettingsManager settingsManager = SettingsManager.Instance;
            particleEffectName = settingsManager.fxSettings.ExplosionFxs[ParticleEffect];
            permanenceSpriteList = settingsManager.fxSettings.PermanenceSprites[PermanenceSprites];
            playerStats = gameManager.RPlayer.RPlayerStats;
        }

        protected virtual void OnEnable()
        {
            if (objectPooler == null)
            {
                if(ObjectPooler.Instance != null)
                {
                    objectPooler = ObjectPooler.Instance;
                }
            }
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

                //ADD OFFSET TO FOLLOW
                GameObject damagePopUp = objectPooler.SpawnFromPool("CritPopUp", transform.position + offset, Quaternion.identity);
                damagePopUp.GetComponent<DamagePopUp>().FollowDamagedObject(transform, offset, damage);
            }
            else
            {
                if (objectPooler == null)
                {
                    Debug.Log(gameObject.name);
                }
                Debug.Log(objectPooler);
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
            GameObject effect = objectPooler.SpawnFromPool(particleEffectName, transform.position, Quaternion.identity);
            effect.transform.localScale *= ParticleEffectScale;
        }

        protected virtual void SpawnSegments()
        {
            for (int i = 0; i < Random.Range(PermanencePartMinMaxAmount.x, PermanencePartMinMaxAmount.y); i++)
            {
                GameObject permanencePart = objectPooler.SpawnFromPool("PermanencePart", transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                permanencePart.transform.localScale = transform.localScale;
                permanencePart.GetComponent<PermanencePart>().InitializePart(permanenceSpriteList, PermanencePartOutwardsPower, PermanenceScaleFactor);
            }
        }

        protected virtual void DropPickUps()
        {
            if(DropTable == null) { return; }
            int dropAmount = Random.Range(DropTable.DropRange.x, DropTable.DropRange.y);
            for(int i = 0; i < dropAmount; i++)
            {
                PickUp pickUp = objectPooler.SpawnFromPool("PickUp", transform.position, Quaternion.identity).GetComponent<PickUp>();

                Drop drop = DropTable.GetDrop();
                pickUp.Initialize(drop);
                /*
                switch (drop.ItemType)
                {
                    case EnumCollections.ItemType.Weapon:
                        pickUp.Initialize();
                        break;
                    case EnumCollections.ItemType.ShipComponent:
                        pickUp.Initialize();
                        break;
                    case EnumCollections.ItemType.Material:
                        pickUp.Initialize();
                        break;
                }
                */
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