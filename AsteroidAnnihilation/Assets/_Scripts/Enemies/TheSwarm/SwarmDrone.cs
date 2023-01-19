using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmDrone : BaseEnemy
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float aggroMove;
        protected float currSpeed;
        [SerializeField] EnumCollections.EnemyProjectiles projectileType;
        [SerializeField] private float projectileDamage;
        [SerializeField] protected float fireRate = 1.5f;
        [SerializeField] private float projectileSize = 1;
        [SerializeField] private float projectileLifeTime = 10f;
        [SerializeField] private float projectileSpeed = 7.5f;

        [SerializeField] private bool randomRotation = true;
        [SerializeField] protected float fireTimer;
        [SerializeField] private bool randomSizeEnabled = true;

        private float baseStopDistance;

        protected override void Start()
        {
            base.Start();
            
            if (randomRotation) { transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0, 360)); }
            float sizeMultiplier = Random.Range(sizeRange.x, sizeRange.y);
            if (randomSizeEnabled)
            {
                Vector2 randomSize = new Vector2(transform.localScale.x * sizeMultiplier, transform.localScale.y * sizeMultiplier);
                transform.localScale = randomSize; 
            }
            currSpeed = moveSpeed * Random.Range(0.998f, 1.002f);
            RotationSpeed *= Random.Range(0.998f, 1.002f);
            fireTimer = fireRate;
            baseStopDistance = StopDistance;
            StopDistance = baseStopDistance + Random.Range(-5, 3);
            
        }

        protected virtual void Update()
        {
            if (spawnManager != null && spawnManager.BossActive)
            {
                MoveAwayFromBoss();
                return;
            }

            CheckAggroDistance();

            if (Aggro)
            {
                AggroMove();
                fireTimer -= Time.deltaTime;
                if(fireTimer <= 0)
                {
                    Fire();
                    fireTimer = fireRate;
                }
            }
            else
            {
                IdleMove();
            }
        }


        protected virtual void IdleMove()
        {
            if(Vector2.Distance(transform.position, Player.position) > StopDistance) { transform.position += transform.up * Time.deltaTime * moveSpeed; }
        }

        protected virtual void AggroMove()
        {
            if (Vector2.Distance(transform.position, Player.position) > StopDistance) { transform.position += transform.up * Time.deltaTime * aggroMove; }
            Rotate(rotSpeedMultiplier: 175);
        }

        protected virtual void Fire()
        {
            SwarmProjectile projectile = objectPooler.SpawnFromPool(projectileType.ToString(), transform.position + transform.up, transform.rotation).GetComponent<SwarmProjectile>();
            projectile.Initialize(projectileSize, projectileDamage, projectileSpeed, projectileLifeTime, false);
        }
    }
}
