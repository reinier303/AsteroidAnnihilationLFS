using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerEntity : BaseEntity
    {
        private Player player;
        private StatManager statManager;
        private SpawnManager spawnManager;
        [SerializeField] private GameObject PlayerHitEffect;
        public bool canHit = true;
        [SerializeField] private float hitCooldown;
        private UIManager uIManager;
        private EquipmentManager equipmentManager;
        private float healthRegen;
        private float resistance;

        public bool RecentlyHit;
        private Collider2D collider;

        protected override void Awake()
        {
            isDead = false;

            OnTakeDamage += TakeDamage;

            onHitMaterial = (Material)Resources.Load("Materials/FlashWhite", typeof(Material));
            baseMaterial = spriteRenderer.material;

            currentHealth = MaxHealth;
            RecentlyHit = false;
        }

        public void GetDefensiveVariables()
        {
            healthRegen = statManager.GetStat(EnumCollections.Stats.HealthRegen);
            MaxHealth = statManager.GetStat(EnumCollections.Stats.Health);
            resistance = statManager.GetStat(EnumCollections.Stats.PhysicalResistance);

            uIManager.UpdateHealth(currentHealth, MaxHealth);
        }

        public void SetHealthToMax()
        {
            currentHealth = MaxHealth;
            uIManager.UpdateHealth(currentHealth, MaxHealth);
            StartCoroutine(RegenerateHealth());
        }

        protected override void Start()
        {
            base.Start();
            player = GetComponent<Player>();
            uIManager = UIManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            spawnManager = SpawnManager.Instance;
            statManager = StatManager.Instance;
            playerStats = GetComponent<PlayerStats>();
            collider = GetComponent<Collider2D>();
            statManager.OnDefensiveStatsChanged += GetDefensiveVariables;
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
            float finalDamage = Mathf.Clamp(damage - resistance, 1, 99999f);
            base.TakeDamage(finalDamage, isCrit);
            RecentlyHit = true;
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

        protected override void Die()
        {
            uIManager.StartCoroutine(uIManager.ShowDeathScreen());
            SpawnParticleEffect();
            cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
            gameObject.SetActive(false);
        }

        public void OnRespawn()
        {
            gameManager.StartCoroutine(ResetPlayerValues());
        }

        private IEnumerator ResetPlayerValues()
        {
            yield return new WaitForSeconds(0.2f);
            isDead = false;
            canHit = true;
            player.RPlayerAttack.canFire = true;
            gameObject.SetActive(true);
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            player.RPlayerAttack.ResetEnergy();
            player.RPlayerMovement.ResetMovementVariables();
            SetHealthToMax();
        }
    }
}