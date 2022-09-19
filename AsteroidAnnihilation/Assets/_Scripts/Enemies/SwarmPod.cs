using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmPod : BaseEnemy
    {
        public float AggroDistance = 8f;
        public float StopDistance = 0.75f;


        [SerializeField]private Coroutine idleMoveRoutine;

        [SerializeField] protected float idleMoveTime = 2, idleWaitTime = 3.5f;
        [Header("Aggro Movement Speed")]
        [SerializeField] protected float aggroMove = 3f;

        protected virtual void Update()
        {
            if(Vector2.Distance(transform.position, Player.position) <= AggroDistance)
            {
                if (idleMoveRoutine != null) 
                {
                    Debug.Log("stopt");
                    StopCoroutine(idleMoveRoutine);
                    idleMoveRoutine = null;
                }

                if(Vector2.Distance(transform.position, Player.position) >= StopDistance) { AggroMove();}
            }
            else if(idleMoveRoutine == null)
            {
                idleMoveRoutine = StartCoroutine(IdleMove());
            }
        }

        protected virtual IEnumerator IdleMove()
        {
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), transform.position.z);
            LeanTween.move(gameObject, randomPos , idleMoveTime);
            yield return new WaitForSeconds(idleWaitTime);
            idleMoveRoutine = StartCoroutine(IdleMove());
        }

        protected virtual void AggroMove()
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, Time.deltaTime * aggroMove);

        }
    }
}
