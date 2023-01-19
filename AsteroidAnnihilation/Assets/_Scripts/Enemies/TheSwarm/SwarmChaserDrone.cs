using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmChaserDrone : BaseEnemy
    {
        [SerializeField]private Coroutine randomIdleMoveRoutine;

        [SerializeField] protected float idleWaitTime = 3.5f;
        [Header("Aggro Movement Speed")]
        [SerializeField] protected float idleMove = 1f, aggroMove = 3f;

        private Vector2 RandomPos;

        protected override void Start()
        {
            base.Start();
            float randomScale = Random.Range(sizeRange.x, sizeRange.y);
            Vector2 randomSize = new Vector2(randomScale, randomScale);
            transform.localScale = randomSize;
        }

        protected virtual void Update()
        {
            if (spawnManager.BossActive)
            {
                //TODO::Fix this
                //MoveAwayFromBoss();
                //return;
            }

            CheckAggroDistance();

            if (Aggro)
            {
                if (randomIdleMoveRoutine != null)
                {
                    StopCoroutine(randomIdleMoveRoutine);
                    randomIdleMoveRoutine = null;
                }

                if (Vector2.Distance(transform.position, Player.position) >= StopDistance) { AggroMove(); }
            }
            else
            {
                if (randomIdleMoveRoutine == null)
                {
                    randomIdleMoveRoutine = StartCoroutine(RandomIdlePos());
                }
                IdleMove();
            }
        }

        protected virtual IEnumerator RandomIdlePos()
        {
            RandomPos = transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
            yield return new WaitForSeconds(idleWaitTime);
            randomIdleMoveRoutine = StartCoroutine(RandomIdlePos());
        }

        protected virtual void AggroMove()
        {
            transform.position += transform.up * Time.deltaTime * aggroMove;
            Rotate();
        }

        protected virtual void IdleMove()
        {
            transform.position += transform.up * Time.deltaTime * idleMove;
            if(Vector2.Distance(transform.position, RandomPos) > 1f)Rotate(RandomPos, 0.5f);
        }
    }
}
