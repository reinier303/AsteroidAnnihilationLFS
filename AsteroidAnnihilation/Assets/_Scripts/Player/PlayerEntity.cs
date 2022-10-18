using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerEntity : BaseEntity
    {
        [SerializeField] private GameObject PlayerHitEffect;
        public bool canHit = true;
        [SerializeField] private float hitCooldown;
        private UIManager uIManager;
        private EquipmentManager equipmentManager;
        private float healthRegen;

        protected override void Awake()
        {
            isDead = false;

            OnTakeDamage += TakeDamage;

            onHitMaterial = (Material)Resources.Load("Materials/FlashWhite", typeof(Material));
            baseMaterial = spriteRenderer.material;

            InitializeDropPool();
            currentHealth = MaxHealth;
        }

        public void GetHealthVariables()
        {
            PlayerStats pStats = GetComponent<PlayerStats>();
            float baseHealth = pStats.GetStatValue(EnumCollections.PlayerStats.BaseHealth);
            float hullHealth = equipmentManager.GetGearStatValue(EnumCollections.ItemType.HullPlating, EnumCollections.EquipmentStats.Health);
            float baseRegen = pStats.GetStatValue(EnumCollections.PlayerStats.BaseHealthRegen);
            float hullRegen = equipmentManager.GetGearStatValue(EnumCollections.ItemType.HullPlating, EnumCollections.EquipmentStats.HealthRegen);
            healthRegen = baseRegen + hullRegen;
            float accessoriesHealth = 0;//TODO::Get from accessories

            MaxHealth = baseHealth + hullHealth + accessoriesHealth;
            uIManager.UpdateHealth(currentHealth, MaxHealth);
        }

        public void SetHealthToMax()
        {
            currentHealth = MaxHealth;
        }

        protected override void Start()
        {
            base.Start();
            uIManager = UIManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            StartCoroutine(RegenerateHealth());
        }

        public override void TakeDamage(float damage, bool isCrit)
        {
            if (!canHit)
            {
                return;
            }
            PlayerHitEffect.SetActive(true);
            uIManager.StartCoroutine(uIManager.TweenAlpha(uIManager.HitVignette.rectTransform, uIManager.VignetteDuration, uIManager.AlphaTo, 0));
            if (!gameManager.PlayerAlive)
            {
                return;
            }
            StartCoroutine(HitCooldown());
            base.TakeDamage(damage, isCrit);
            uIManager.UpdateHealth(currentHealth, MaxHealth);
        }

        protected override void SpawnParticleEffect()
        {
            gameManager.StartCoroutine(SpawnDeathEffect());
        }

        private IEnumerator SpawnDeathEffect()
        {
            for (int i = 0; i < 10; i++)
            {
                objectPooler.SpawnFromPool("PlayerExplosion",
                (Vector2)transform.position + new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f)),
                Quaternion.identity);
                cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
                yield return new WaitForSeconds(0.2f + Random.Range(-0.05f, 0.15f));
            }
            objectPooler.SpawnFromPool("PlayerEndExplosion", (Vector2)transform.position + new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)), Quaternion.identity);
            cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude * 1.5f));
            SpawnSegments();
        }

        private IEnumerator HitCooldown()
        {
            canHit = false;
            bool back = false;
            for(int i = 0; i < 16; i++)
            {
                if(back)
                {
                    spriteRenderer.color = new Color(1, 1, 1, 1f);
                }
                else
                {
                    spriteRenderer.color = new Color(1, 1, 1, 0.35f);
                }
                back = !back;
                yield return new WaitForSeconds(hitCooldown / 16);
            }
            //yield return new WaitForSeconds(hitCooldown);
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            canHit = true;
        }

        private IEnumerator RegenerateHealth()
        {
            if (currentHealth < MaxHealth) { currentHealth += healthRegen / 30; }
            else { currentHealth = MaxHealth; }
            uIManager.UpdateHealth(currentHealth, MaxHealth);
            yield return new WaitForSeconds(1f / 30);
            StartCoroutine(RegenerateHealth());
        }
    }
}