using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "BossMoves/Collection", order = 951)]
    public class BossMoveCollection : SerializedScriptableObject
    {
        //This is in baseSO because we will be able to access it easily from the boss script this way. Leave at 0 if not wanted/not applicable
        public float PreFire = 0.05f;
        public float MoveStartDelay = 0;
        public float MoveEndDelay = 0;

        public List<BaseBossMove> Moves;

        public BaseBossMove GetRandomMove()
        {
            return Moves[Random.Range(0, Moves.Count)];
        }

        public void ExecuteMoves(Transform transform, BaseBoss runOn, ObjectPooler objectPooler)
        {
            foreach (BaseBossMove move in Moves)
            {
                move.ExecuteMove(transform, runOn, objectPooler);
            }
        }

        public float GetMovesTime()
        {
            float time = 0;
            float highestDelay = 0;
            foreach (BaseBossMove move in Moves)
            {
                if(move.AddDelayToMoveTime && move.MoveStartDelay > highestDelay)
                {
                    highestDelay = move.MoveStartDelay;
                }
                if (move.MoveTime > time)
                {
                    time = move.MoveTime;
                }
            }
            return time + highestDelay;
        }
    }
}

