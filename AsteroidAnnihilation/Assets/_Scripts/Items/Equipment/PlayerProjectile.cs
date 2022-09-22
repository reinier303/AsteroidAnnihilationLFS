using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PlayerProjectile : MonoBehaviour
    {
        public string OnHitEffectName;
        protected ObjectPooler objectPooler;
        protected GameManager gameManager;
        [HideInInspector] public int WeaponIndex;

        public float Damage, ProjectileSpeed, LifeTime = 0;
        public bool IsCrit;
        public float Size = 1;

        public Vector2 PlayerVelocity;

        protected virtual void Awake()
        {
            objectPooler = ObjectPooler.Instance;
            gameManager = GameManager.Instance;
        }

        public virtual void Initialize(float size)
        {
            Size = size;
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y) * Size;
        }

        protected virtual void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            transform.position += (transform.up * Time.deltaTime * ProjectileSpeed) + (Vector3)PlayerVelocity;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            BaseEntity entity = collider.GetComponent<BaseEntity>();
            if (entity != null && !entity.isDead)
            {
                //gameManager.StartCoroutine(gameManager.Sleep(0.001f));
                entity.KilledByIndex = WeaponIndex;
                entity.OnTakeDamage?.Invoke(Damage, IsCrit);
                objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
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