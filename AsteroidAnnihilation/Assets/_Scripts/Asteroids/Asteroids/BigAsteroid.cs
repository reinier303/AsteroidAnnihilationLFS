using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BigAsteroid : Asteroid
    {
        protected override void Die()
        {
            base.Die();
            int numberToSpawn = Mathf.RoundToInt((transform.localScale.x - 0.3f) * 3);
            for(int i = 0; i < numberToSpawn; i++)
            {
                objectPooler.SpawnFromPool("Asteroid", transform.position, Quaternion.identity);
            }
        }
    }
}