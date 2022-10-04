using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class BaseBoss : BaseEnemy
    {
        [FoldoutGroup("Boss Variables")][SerializeField] protected List<BaseBossMove> moves;
        protected enum MoveOrders {Random, RandomNonRecursive, Ordered}
        [FoldoutGroup("Boss Variables")][SerializeField] protected MoveOrders moveOrder;

        protected List<BaseBossMove> movesNotExecuted;
        [FoldoutGroup("Boss Variables")] [SerializeField] protected float timeBetweenMoves = 0.1f;
        BaseBossMove lastMove;
        private int currentMove = 0;

        protected override void Awake()
        {
            base.Awake();
            movesNotExecuted = new List<BaseBossMove>();
        }

        protected override void Start()
        {
            base.Start();
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
            Rotate();
        }

        private IEnumerator StartMovesRandom()
        {
            int random = Random.Range(0, moves.Count);
            BaseBossMove move = moves[random];
            move.ExecuteMove(transform, this, objectPooler);
            yield return new WaitForSeconds(move.moveTime + timeBetweenMoves);
            StartCoroutine(StartMovesRandom());
        }

        private IEnumerator StartMovesRandomNonRecursive()
        {
            if (movesNotExecuted.Count == 0) { movesNotExecuted.AddRange(moves); }

            int random = Random.Range(0, movesNotExecuted.Count);
            Debug.Log(random + ", " + movesNotExecuted.Count);
            BaseBossMove move = movesNotExecuted[random];
            while(move == lastMove)
            {
                random = Random.Range(0, movesNotExecuted.Count);
                move = movesNotExecuted[random];
            }
            lastMove = move;
            move.ExecuteMove(transform, this, objectPooler);
            movesNotExecuted.RemoveAt(random);
            yield return new WaitForSeconds(move.moveTime + timeBetweenMoves);
            StartCoroutine(StartMovesRandomNonRecursive());
        }

        private IEnumerator StartMovesOrdered()
        {
            if(currentMove == moves.Count) { currentMove = 0; }
            BaseBossMove move = moves[currentMove];
            move.ExecuteMove(transform, this, objectPooler);
            currentMove++;
            yield return new WaitForSeconds(move.moveTime + timeBetweenMoves);
            StartCoroutine(StartMovesOrdered());
        }
    }

    public class BaseBossMove : SerializedScriptableObject
    {
        public float moveTime;

        public virtual void ExecuteMove(Transform bossTransform, MonoBehaviour runOn, ObjectPooler objectPooler)
        {
            //this method is meant to be overridden
        }
    }
}