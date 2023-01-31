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
        protected AudioManager audioManager;
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
            audioManager = AudioManager.Instance;
            baseScale = transform.localScale;
        }

        public virtual void Initialize(float size, float damage, float speed, float lifeTime, bool isCrit, bool secondary = false)
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
            //Vector3 extraUp = transform.up * new Vector2(Mathf.Abs(PlayerVelocity.x), Mathf.Abs(PlayerVelocity.y));
            //Vector3 finalUp = extraUp + transform.up;
            Vector2 direction = Vector2.up * ProjectileSpeed;
            Vector2 heritedVelocity = new Vector2(PlayerVelocity.x, PlayerVelocity.y);
            Vector2 heritedMove = transform.InverseTransformDirection(heritedVelocity);
            Vector3 move = direction + (heritedMove * 0.1f) /** ProjectileSpeed*/;

            move *= Time.deltaTime;
            move = transform.TransformDirection(move);
            //transform.position += (finalUp * Time.deltaTime * ProjectileSpeed);
            transform.position += move;

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
                audioManager.PlayAudio("PlasmaGunHit");
                gameObject.SetActive(false);
            }
        }

        public virtual IEnumerator DisableAfterTime()
        {
            yield return new WaitForSeconds(LifeTime);
            objectPooler.SpawnFromPool(OnHitEffectName, transform.position, Quaternion.identity);
            //audioManager.PlayAudio("PlasmaGunHit");
            gameObject.SetActive(false);
        }

        public void SetCrit()
        {
            IsCrit = true;
            Damage *= gameManager.RPlayer.RPlayerStats.GetStatValue(EnumCollections.PlayerStats.CritMultiplier);
        }
    }
}