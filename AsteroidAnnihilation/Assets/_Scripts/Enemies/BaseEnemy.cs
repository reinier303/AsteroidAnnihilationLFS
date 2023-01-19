using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
namespace AsteroidAnnihilation
{
    public class BaseEnemy : BaseEntity
    {
        protected SpawnManager spawnManager;
        public Transform Player;

        //Stats
        public EnumCollections.DamageType DamageType;
        public float ContactDamage;
        public float RotationSpeed;
        public float DroppedUnits;
        public float ExperienceGained;

        //Aggro
        public float AggroDistance = 8f;
        public float DeAggroDistance = 50f;

        [SerializeField] protected Vector2 sizeRange = new Vector2(0.9f, 1.25f);

        [SerializeField] protected bool grouped = false;
        protected EnemyGroup enemyGroup;
        public float StopDistance = 0.75f;
        public bool DeathOnImpact;

        public string enemyType;

        protected override void Awake()
        {
            base.Awake();
            if (grouped) { enemyGroup = transform.parent.GetComponent<EnemyGroup>(); }
        }

        protected override void Start()
        {
            base.Start();
            Player = GameManager.Instance.RPlayer.transform;
            spawnManager = SpawnManager.Instance;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerEntity player = collision.GetComponent<PlayerEntity>();

            if (player != null)
            {
                player.OnTakeDamage?.Invoke(ContactDamage, false);
                if(DeathOnImpact)
                {
                    Die();
                }
            }
        }

        protected void MoveAwayFromBoss()
        {
            Vector2 target = -Player.position;
            transform.position += transform.up * Time.deltaTime * 3.5f;
            Rotate(target);
        }

        protected virtual void CheckAggroDistance()
        {
            if (Vector2.Distance(transform.position, Player.position) <= AggroDistance)
            {
                Aggro = true;
            }
            if (Vector2.Distance(transform.position, Player.position) >= DeAggroDistance)
            {
                Aggro = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Player = GameManager.Instance.RPlayer.transform;
            StartCoroutine(CheckDistanceToPlayer(1f));
        }

        protected virtual void Rotate(Vector3 target = default(Vector3), float rotSpeedMultiplier = 1)
        {
            Vector3 difference;
            if (target == default(Vector3)) { difference = Player.position - transform.position; }
            else { difference = target - transform.position; }
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, RotationSpeed * rotSpeedMultiplier * Time.deltaTime * 60);
            if (!gameManager.PlayerAlive)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90f);
            }
        }

        protected virtual IEnumerator CheckDistanceToPlayer(float time)
        {
            if (Vector2.Distance(transform.position, Player.position) > 250f)
            {
                gameObject.SetActive(false);
                spawnManager.RemoveEnemyType(enemyType);
            }
            yield return new WaitForSeconds(time);
            StartCoroutine(CheckDistanceToPlayer(time));
        }

        protected override void Die()
        {
            if (grouped) { enemyGroup.StartCoroutine(enemyGroup.EnemyDisabled()); }
            DropUnits(DroppedUnits);
            gameManager.RPlayer.RPlayerStats.AddToExperience(ExperienceGained);
            ExpPopUp expPopUp = objectPooler.SpawnFromPool("ExpPopUp", Vector2.zero, Quaternion.identity).GetComponent<ExpPopUp>();
            expPopUp.Initialize(ExperienceGained);
            spawnManager.RemoveEnemyType(enemyType);
            base.Die();
        }

        protected virtual void DropUnits(float units)
        {
            units *= UnityEngine.Random.Range(0.8f, 1.3f);

            int unitAmount5000 = (int)((units - units % 5000) / 5000);
            units -= unitAmount5000 * 5000;

            int unitAmount250 = (int)(((units - units % 250) / 250));
            units -= unitAmount250 * 250;

            int unitAmount5 = (int)((units / 5));

            for (int i = 0; i < unitAmount5000; i++)
            {
                GameObject unitObj = objectPooler.SpawnFromPool("Unit5000", transform.position, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveUnit();
            }
            for (int i = 0; i < unitAmount250; i++)
            {
                GameObject unitObj = objectPooler.SpawnFromPool("Unit250", transform.position, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveUnit();
            }
            for (int i = 0; i < unitAmount5; i++)
            {
                GameObject unitObj = objectPooler.SpawnFromPool("Unit5", transform.position, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveUnit();
            }
        }
    }
}