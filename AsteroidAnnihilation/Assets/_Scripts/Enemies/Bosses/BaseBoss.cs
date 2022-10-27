using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class BaseBoss : BaseEnemy
    {
        private UIManager uiManager;

        [FoldoutGroup("Boss Variables")][SerializeField] protected List<BaseBossMove> moves;
        protected enum MoveOrders {Random, RandomNonRecursive, Ordered}
        [FoldoutGroup("Boss Variables")][SerializeField] protected MoveOrders moveOrder;

        protected List<BaseBossMove> movesNotExecuted;
        [FoldoutGroup("Boss Variables")] [SerializeField] protected float timeBetweenMoves = 0.1f;
        BaseBossMove lastMove;
        private BaseBossMove moveBeingExecuted;
        private int currentMove = 0;
        //This is te vector rotated towards based on the prefire variable.
        private Player playerScript;

        protected override void Awake()
        {
            base.Awake();
            uiManager = UIManager.Instance;
            movesNotExecuted = new List<BaseBossMove>();
        }

        protected override void Start()
        {
            base.Start();
            playerScript = Player.GetComponent<Player>();
            uiManager.EnableBossHealthBar();
            uiManager.UpdateBossHealthBar(MaxHealth, MaxHealth);
            switch (moveOrder)
            {
                case MoveOrders.Random:
                    StartCoroutine(StartMovesRandom());
                    break;
                case MoveOrders.RandomNonRecursive:
                    StartCoroutine(StartMovesRandomNonRecursive());
                    break;
                case MoveOrders.Ordered:
                    StartCoroutine(StartMovesOrdered());
                    break;
            }
        }

        protected virtual void Update()
        {
            if(moveBeingExecuted != null && moveBeingExecuted.PreFire != 0)
            {
                Rotate(playerScript.GetPlayerPositionAfterSeconds(moveBeingExecuted.PreFire));
            }
            else { Rotate(); }
        }

        private IEnumerator StartMovesRandom()
        {
            int random = Random.Range(0, moves.Count);
            BaseBossMove move = moves[random];
            yield return new WaitForSeconds(move.MoveStartDelay);
            moveBeingExecuted = move;
            move.ExecuteMove(transform, this, objectPooler);
            yield return new WaitForSeconds(move.MoveTime + timeBetweenMoves);
            yield return new WaitForSeconds(move.MoveEndDelay);
            StartCoroutine(StartMovesRandom());
        }

        private IEnumerator StartMovesRandomNonRecursive()
        {
            if (movesNotExecuted.Count == 0) { movesNotExecuted.AddRange(moves); }

            int random = Random.Range(0, movesNotExecuted.Count);
            BaseBossMove move = movesNotExecuted[random];
            while(move == lastMove && moves.Count != 1)
            {
                random = Random.Range(0, movesNotExecuted.Count);
                move = movesNotExecuted[random];
            }
            lastMove = move;
            yield return new WaitForSeconds(move.MoveStartDelay);
            moveBeingExecuted = move;
            move.ExecuteMove(transform, this, objectPooler);
            movesNotExecuted.RemoveAt(random);
            yield return new WaitForSeconds(move.MoveTime + timeBetweenMoves);
            yield return new WaitForSeconds(move.MoveEndDelay);
            StartCoroutine(StartMovesRandomNonRecursive());
        }

        private IEnumerator StartMovesOrdered()
        {
            if(currentMove == moves.Count) { currentMove = 0; }
            BaseBossMove move = moves[currentMove];
            yield return new WaitForSeconds(move.MoveStartDelay);
            moveBeingExecuted = move;
            move.ExecuteMove(transform, this, objectPooler);
            currentMove++;
            yield return new WaitForSeconds(move.MoveTime + timeBetweenMoves);
            yield return new WaitForSeconds(move.MoveEndDelay);
            StartCoroutine(StartMovesOrdered());
        }

        protected override void Die()
        {
            uiManager.DisableBossHealthBar();
            base.Die();
        }

        public override void TakeDamage(float damage, bool isCrit)
        {
            base.TakeDamage(damage, isCrit);
            uiManager.UpdateBossHealthBar(currentHealth, MaxHealth);
        }
    }

    public class BaseBossMove : SerializedScriptableObject
    {
        public float MoveTime;
        public float MoveStartDelay = 0;
        public float MoveEndDelay = 0;
        //This is in baseSO because we will be able to access it easily from the boss script this way. Leave at 0 if not wanted/not applicable
        public float PreFire = 0.05f;

        public virtual void ExecuteMove(Transform bossTransform, MonoBehaviour runOn, ObjectPooler objectPooler)
        {
            //this method is meant to be overridden
        }
    }

    [System.Serializable]
    public class BossMoveCollection
    {
        public List<BaseBossMove> Moves;

        public BaseBossMove GetRandomMove()
        {
            return Moves[Random.Range(0, Moves.Count)];
        }
    }
}