using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Swarmling : SwarmChaserDrone
    {
        private bool pooled;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (pooled)
            {
                objectPooler.SpawnFromPool("SwarmExplosion2Small", transform.position, Quaternion.identity);
            }
            pooled = true;
        }
    }
}

