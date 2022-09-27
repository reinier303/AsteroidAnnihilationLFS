using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmPod : BaseEnemy
    {
        [SerializeField] protected float moveSpeed;

        private Vector2 RandomPos;

        protected override void Start()
        {
            base.Start();
            transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0,360));
            Vector2 randomSize = new Vector2(Random.Range(0.9f, 1.25f), Random.Range(0.9f, 1.25f));
            transform.localScale = randomSize;
        }


        protected virtual void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            transform.position += transform.up * Time.deltaTime * moveSpeed;
            Rotate();
        }
    }
}
