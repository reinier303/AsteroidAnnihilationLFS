using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class AsteroidGroup_Alligned : AsteroidGroup
    {
        public float MinForce;
        public float MaxForce;
        public float MaxRotationSpeed;

        public Vector2 GroupForce;
        public float GroupTorque;

        public List<Formation> Formations;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetGroupForce();
            CalculateGroupTorque();
            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        protected virtual void Start()
        {
            //In start as well to deal with objectpooler setting it to 0
            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        public void SetGroupForce()
        {
            Vector2 force = new Vector2(Random.Range(-MaxForce, MaxForce), Random.Range(-MaxForce, MaxForce));

            //Ugly if tree to make sure asteroids don't go too slow.
            if (force.x > 0 && force.x < MinForce)
            {
                force.x = MinForce;
            }
            else if (force.x < 0 && force.x > -MinForce)
            {
                force.x = -MinForce;
            }
            if (force.y > 0 && force.y < MinForce)
            {
                force.y = MinForce;
            }
            else if (force.y < 0 && force.y > -MinForce)
            {
                force.y = -MinForce;
            }
            //ew...

            GroupForce = force;
        }

        public void CalculateGroupTorque()
        {
            GroupTorque =  Random.Range(-MaxRotationSpeed, MaxRotationSpeed);
        }

        protected override void ResetAsteroids()
        {
            Formation currentFormation = Formations[Random.Range(0, Formations.Count)];

            //Check if formations fits the amount of asteroids
            if(currentFormation.Positions.Length == asteroids.Count)
            {
                for (int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i].localPosition = currentFormation.Positions[i];
                    asteroids[i].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i].localPosition = asteroidStartPositions[i];
                    asteroids[i].gameObject.SetActive(true);
                }
            }

            AsteroidsDisabled = 0;
        }
    }

    [System.Serializable]
    public struct Formation
    {
        public Vector2[] Positions;
    }
}

