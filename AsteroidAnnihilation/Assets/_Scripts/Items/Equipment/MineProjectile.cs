using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class MineProjectile : BaseProjectile
    {
        public override void Initialize(float size, float damage, float speed, float lifeTime, bool isCrit, bool seconday = false)
        {
            base.Initialize(size, damage, speed, lifeTime, isCrit);
        }

        protected override void Move()
        {
            //This method is meant to override.
        }
    }
}
