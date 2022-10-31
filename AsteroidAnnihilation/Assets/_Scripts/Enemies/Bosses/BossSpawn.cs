using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "BossMoves/BossSpawn", order = 998)]
    public class BossSpawn : BaseBossMove
    {
        public EnumCollections.EnemyTypes EnemyToSpawn;
        public int SpawnAmount;
        public Vector2 SpawnPosition;

        private ObjectPooler objectPooler;
        private Transform bossTransform;

        public override void ExecuteMove(Transform bossTransform, BaseBoss runOn, ObjectPooler objectPooler)
        {
            if (this.objectPooler == null) { this.objectPooler = objectPooler; }
            if (this.bossTransform == null) { this.bossTransform = bossTransform; }
            runOn.AddActiveMove(runOn.StartCoroutine(Spawn()));
        }
        
        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(MoveStartDelay);
            for (int i = 0; i < SpawnAmount; i++)
            {
                Vector3 spawnOffset = bossTransform.TransformDirection(SpawnPosition);
                objectPooler.SpawnFromPool(EnemyToSpawn.ToString(), bossTransform.position + spawnOffset, bossTransform.rotation);
                yield return new WaitForSeconds(MoveTime / SpawnAmount);
            }
        }
    }
}
