using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmPod : BaseEnemy
    {
        [SerializeField] protected float moveSpeed;
        protected float currSpeed;

        [SerializeField] private bool randomRotation = true;

        protected override void Start()
        {
            base.Start();
            if (randomRotation) { transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0, 360)); }
            float sizeMultiplier = Random.Range(sizeRange.x, sizeRange.y);
            Vector2 randomSize = new Vector2(transform.localScale.x * sizeMultiplier, transform.localScale.y * sizeMultiplier);
            transform.localScale = randomSize;
            currSpeed = moveSpeed * Random.Range(0.998f, 1.002f);
            RotationSpeed *= Random.Range(0.998f, 1.002f);
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
