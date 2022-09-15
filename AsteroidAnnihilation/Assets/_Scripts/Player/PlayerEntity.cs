using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerEntity : BaseEntity
    {
        [SerializeField] private GameObject PlayerHitEffect;
        private bool canHit = true;
        [SerializeField] private float hitCooldown;
        private UIManager RUIManager;

        protected override void Awake()
        {
            base.Awake();
            MaxHealth = GetComponent<PlayerStats>().Stats["Health"];
            currentHealth = MaxHealth.GetBaseValue();
        }

        protected override void Start()
        {
            base.Start();
            RUIManager = UIManager.Instance;
        }

        public override void TakeDamage(float damage, bool isCrit)
        {
            if (!canHit)
            {
                return;
            }
            PlayerHitEffect.SetActive(true);
            RUIManager.StartCoroutine(RUIManager.TweenAlpha(RUIManager.HitVignette.rectTransform, RUIManager.VignetteDuration, RUIManager.AlphaTo, 0));
            if (!gameManager.PlayerAlive)
            {
                return;
            }
            StartCoroutine(HitCooldown());
            base.TakeDamage(damage, isCrit);
            RUIManager.UpdateHealth();
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
            yield return new WaitForSeconds(hitCooldown);
            canHit = true;
        }
    }
}