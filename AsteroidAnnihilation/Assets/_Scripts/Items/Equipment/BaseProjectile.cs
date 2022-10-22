using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BaseProjectile : MonoBehaviour
    {
        public string OnHitEffectName;
        protected ObjectPooler objectPooler;
        protected GameManager gameManager;
        [HideInInspector] public int WeaponIndex;

        public float Damage, ProjectileSpeed, LifeTime = 0;
        public bool IsCrit;
        public float Size = 1;
        protected bool canDamage;

        public Vector2 PlayerVelocity;

        protected Vector2 baseScale;

        protected virtual void Awake()
        {
            objectPooler = ObjectPooler.Instance;
            gameManager = GameManager.Instance;
            baseScale = transform.localScale;
        }

        public virtual void Initialize(float size, float damage, float speed, float lifeTime, bool isCrit)
        {
            canDamage = true;
            Size = size;
            Damage = damage;
            ProjectileSpeed = speed;
            LifeTime = lifeTime;
            IsCrit = isCrit;
            if (IsCrit) { SetCrit(); }
            transform.localScale = baseScale * Size;
            StartCoroutine(DisableAfterTime());
        }

        protected virtual void Update()
        {
            if (gameManager.isPaused) { return; }
            Move();
        }

        protected virtual void Move()
        {
            Vector3 extraUp = transform.up * new Vector2(Mathf.Abs(PlayerVelocity.x), Mathf.Abs(PlayerVelocity.y));
            Vector3 finalUp = extraUp + transform.up;
            Debug.Log(PlayerVelocity + ", " + finalUp);

            transform.position += (finalUp * Time.deltaTime * ProjectileSpeed);
            Debug.Log(transform.up);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            BaseEntity entity = collider.GetComponent<BaseEntity>();
            if (canDamage && entity != null && !entity.isDead)
            {
                canDamage = false;
                //gameManager.StartCoroutine(gameManager.Sleep(0.001f));
                entity.KilledByIndex = WeaponIndex;
                entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                entity.Aggro = true;
                GameObject hitEffect = objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
                hitEffect.transform.localScale = new Vector2(Size, Size);
                gameObject.SetActive(false);
            }
        }

        public virtual IEnumerator DisableAfterTime()
        {
            yield return new WaitForSeconds(LifeTime);
            objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }

        public void SetCrit()
        {
            IsCrit = true;
            Damage *= gameManager.RPlayer.RPlayerStats.GetStatValue(EnumCollections.PlayerStats.CritMultiplier);
        }
    }
}