using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class OrbitingAsteroid : Asteroid
    {
        private FollowPlayer followPlayer;

        [SerializeField] private float orbitDistance;
        [SerializeField] private float orbitSpeed;

        private bool orbiting;
        private Transform poolParent;

        protected override void Awake()
        {
            base.Awake();
            poolParent = transform.parent;
            RandomizeSpeedAndDistance();
        }

        protected override void Start()
        {
            base.Start();
            followPlayer = FollowPlayer.Instance;
        }

        protected virtual void RandomizeSpeedAndDistance()
        {
            float randomMultiplier = Random.Range(0.75f, 1.25f);
            int direction = Random.Range(0,2);
            if(direction == 0)
            {
                orbitSpeed *= -1;
            }
            orbitSpeed *= randomMultiplier;
            orbitDistance *= randomMultiplier;
        }

        protected virtual void Update()
        {
            float distance = Vector2.Distance(Player.position, transform.position);

            if(distance <= orbitDistance)
            {
                StartOrbit();
            }
            if (orbiting)
            {
                OrbitPlayer();
            }
        }

        protected virtual void StartOrbit()
        {
            if (!orbiting)
            {
                rb.velocity = Vector2.zero;
                transform.parent = followPlayer.transform;
                orbiting = true;
            }
        }


        protected virtual void OrbitPlayer()
        {
            transform.RotateAround(Player.position, Vector3.forward, orbitSpeed * Time.deltaTime);
        }

        protected override void Die()
        {
            transform.parent = poolParent;
            base.Die();
        }
    }
}
