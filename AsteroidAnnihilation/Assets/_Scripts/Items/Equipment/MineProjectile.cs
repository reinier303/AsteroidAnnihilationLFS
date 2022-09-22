using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class MineProjectile : PlayerProjectile
    {
        public override void Initialize(float size)
        {
            base.Initialize(size);
        }

        protected override void Move()
        {
            //This method is meant to override.
        }
    }
}
