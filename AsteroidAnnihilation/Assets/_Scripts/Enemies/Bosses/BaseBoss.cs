using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class BaseBoss : BaseEnemy
    {
        private UIManager uiManager;

        protected enum MoveOrders {Random, RandomNonRecursive, Ordered}

        [FoldoutGroup("Boss Variables")][SerializeField] protected List<BossPhase> phases;
        [FoldoutGroup("Boss Variables")][SerializeField] protected MoveOrders moveOrder;
        [FoldoutGroup("Boss Variables")][SerializeField] protected float timeBetweenMoves = 0.1f;
        

        protected List<BossMoveCollection> movesNotExecuted;
        BossMoveCollection lastMove;
        private BossMoveCollection moveCollectionBeingExecuted;
        private List<Coroutine> movesActive;
        private Coroutine moveLoop;

        private int currentPhase;
        private int currentMove = 0;
        private Player playerScript;

        protected override void Awake()
        {
            base.Awake();
            movesNotExecuted = new List<BossMoveCollection>();
            movesActive = new List<Coroutine>();
        }

        protected override void Start()
        {
            base.Start();
            uiManager = UIManager.Instance;
            playerScript = Player.GetComponent<Player>();
            uiManager.EnableBossHealthBar();
            uiManager.UpdateBossHealthBar(MaxHealth, MaxHealth);
            currentPhase = 0;
            if(moveLoop == null) { StartMoves(); }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (UIManager.Instance != null)
            {
                if (uiManager == null)
                {
                    uiManager = UIManager.Instance;
                }
                uiManager.EnableBossHealthBar();
                uiManager.UpdateBossHealthBar(MaxHealth, MaxHealth);
                currentPhase = 0;
                if (moveLoop == null) { StartMoves(); }
            }
        }

        private void OnDisable()
        {
            uiManager.DisableBossHealthBar();
            StopAllMoves();
        }

        protected void StartMoves()
        {
            switch (moveOrder)
            {
                case MoveOrders.Random:
                    moveLoop = StartCoroutine(StartMovesRandom());
                    break;
                case MoveOrders.RandomNonRecursive:
                    moveLoop = StartCoroutine(StartMovesRandomNonRecursive());
                    break;
                case MoveOrders.Ordered:
                    moveLoop = StartCoroutine(StartMovesOrdered());
                    break;
            }
        }

        protected virtual void Update()
        {
            if (isDead) { return; }
            if(moveCollectionBeingExecuted != null && moveCollectionBeingExecuted.PreFire != 0)
            {
                Rotate(playerScript.GetPlayerPositionAfterSeconds(moveCollectionBeingExecuted.PreFire));
            }
            else { Rotate(); }
        }

        private IEnumerator StartMovesRandom()
        {
            movesActive.Clear();
            int random = Random.Range(0, phases[currentPhase].moves.Count);
            BossMoveCollection move = phases[currentPhase].moves[random];
            yield return new WaitForSeconds(move.MoveStartDelay);
            moveCollectionBeingExecuted = move;
            move.ExecuteMoves(transform, this, objectPooler);
            yield return new WaitForSeconds(move.GetMovesTime() + timeBetweenMoves);
            yield return new WaitForSeconds(move.MoveEndDelay);
            moveLoop = StartCoroutine(StartMovesRandom());
        }

        private IEnumerator StartMovesRandomNonRecursive()
        {
            movesActive.Clear();

            if (movesNotExecuted.Count == 0) { movesNotExecuted.AddRange(phases[currentPhase].moves); }

            int random = Random.Range(0, movesNotExecuted.Count);
            BossMoveCollection move = movesNotExecuted[random];
            while(move == lastMove && phases[currentPhase].moves.Count != 1)
            {
                random = Random.Range(0, movesNotExecuted.Count);
                move = movesNotExecuted[random];
            }
            lastMove = move;
            yield return new WaitForSeconds(move.MoveStartDelay);
            moveCollectionBeingExecuted = move;
            move.ExecuteMoves(transform, this, objectPooler);
            //TODO::FIX THIS THROWING OUT OF RANGE
            movesNotExecuted.RemoveAt(random);
            yield return new WaitForSeconds(move.GetMovesTime() + timeBetweenMoves);
            yield return new WaitForSeconds(move.MoveEndDelay);
            moveLoop = StartCoroutine(StartMovesRandomNonRecursive());
        }

        private IEnumerator StartMovesOrdered()
        {
            movesActive.Clear();

            if (currentMove == phases[currentPhase].moves.Count) { currentMove = 0; }
            BossMoveCollection move = phases[currentPhase].moves[currentMove];
            yield return new WaitForSeconds(move.MoveStartDelay);
            moveCollectionBeingExecuted = move;
            move.ExecuteMoves(transform, this, objectPooler);
            currentMove++;
            yield return new WaitForSeconds(move.GetMovesTime() + timeBetweenMoves);
            yield return new WaitForSeconds(move.MoveEndDelay);
            moveLoop = StartCoroutine(StartMovesOrdered());
        }

        protected override void Die()
        {
            StopAllMoves();
            spawnManager.BossActive = false;
            isDead = true;
            uiManager.DisableBossHealthBar();
            missionManager.MissionCompleted();
            SpawnParticleEffect();
        }

        public override void TakeDamage(float damage, bool isCrit)
        {
            base.TakeDamage(damage, isCrit);
            CheckPhase();
            uiManager.UpdateBossHealthBar(currentHealth, MaxHealth);
        }

        public void CheckPhase()
        {
            if(currentHealth <= 0) { return; }
            float healthPercentage = currentHealth / MaxHealth * 100;
            if(phases.Count - 1 > currentPhase && healthPercentage < phases[currentPhase + 1].Percentage)
            {
                currentPhase++;
                movesNotExecuted.Clear();
                cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration / 2, ShakeMagnitude / 2));
                objectPooler.SpawnFromPool(EnumCollections.ExplosionFX.SwarmExplosion1.ToString(), transform.position, transform.rotation);
                StopAllMoves();
                StartMoves();
                GameManager.Instance.Sleep(0.1f);
            }
        }

        public void AddActiveMove(Coroutine move)
        {
            movesActive.Add(move);
        }

        private void StopAllMoves()
        {
            if (moveLoop != null)
            {
                StopCoroutine(moveLoop);
                moveLoop = null;
            }
            foreach (Coroutine move in movesActive)
            {
                StopCoroutine(move);
            }
        }

        protected override IEnumerator CheckDistanceToPlayer(float time)
        {
            yield return new WaitForSeconds(time / 2);

            if (Vector2.Distance(transform.position, Player.position) > 70f)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.position, 55f);
                Debug.Log(objectPooler);
                GameObject teleport = objectPooler.SpawnFromPool("SwarmTeleport", transform.position, Quaternion.identity);
                teleport.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
            }
            yield return new WaitForSeconds(time / 2);
            StartCoroutine(CheckDistanceToPlayer(time));
        }

        protected override void SpawnParticleEffect()
        {
            StartCoroutine(SpawnDeathEffect());
        }

        private IEnumerator SpawnDeathEffect()
        {
            for (int i = 0; i < 15; i++)
            {
                objectPooler.SpawnFromPool("BossExplosion",
                (Vector2)transform.position + new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f)),
                Quaternion.identity);
                cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
                yield return new WaitForSeconds(0.2f + Random.Range(-0.05f, 0.15f));
            }
            cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude * 1.5f));
            objectPooler.SpawnFromPool("BossEndExplosion", (Vector2)transform.position + new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)), Quaternion.identity);

            DropUnits(DroppedUnits);
            gameManager.RPlayer.RPlayerStats.AddToExperience(ExperienceGained);
            ExpPopUp expPopUp = objectPooler.SpawnFromPool("ExpPopUp", Vector2.zero, Quaternion.identity).GetComponent<ExpPopUp>();
            expPopUp.Initialize(ExperienceGained);
            spawnManager.RemoveEnemyType(enemyType);
            SpawnSegments();
            DropPickUps();
            missionManager.AddObjectiveProgress(MissionObjectiveType.Elimination);
            cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
            gameObject.SetActive(false);
        }
    }

    public class BaseBossMove : SerializedScriptableObject
    {
        public float MoveTime;
        public float MoveStartDelay;
        public bool AddDelayToMoveTime;

        public virtual void ExecuteMove(Transform bossTransform, BaseBoss runOn, ObjectPooler objectPooler)
        {
            //this method is meant to be overridden
        }
    }

    [System.Serializable]
    public struct BossPhase
    {
        public float Percentage;
        public List<BossMoveCollection> moves;
    }
}