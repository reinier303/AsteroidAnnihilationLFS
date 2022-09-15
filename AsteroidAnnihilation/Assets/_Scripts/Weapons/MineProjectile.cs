using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class MineProjectile : PlayerProjectile
    {
        public float Size = 1;

        public override void Initialize()
        {
            float value = Size;
            transform.localScale = new Vector2(value, value);
        }

        protected override void Move()
        {
            //This method is meant to override.
        }
    }
}
