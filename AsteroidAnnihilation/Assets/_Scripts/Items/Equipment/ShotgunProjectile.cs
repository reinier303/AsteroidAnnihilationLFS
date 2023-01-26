using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ShotgunProjectile : BaseProjectile
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Initialize(float size, float damage, float speed, float lifeTime, bool isCrit, bool secondary = false)
        {
            base.Initialize(size, damage, speed, lifeTime, isCrit);
            float sizeMultiplier = Random.Range(0.6f, 1.15f);

            transform.localScale = baseScale * sizeMultiplier * size;
            StartCoroutine(ShrinkOverTime(transform.localScale.x));
        }

        private IEnumerator ShrinkOverTime(float scale)
        {
            if(transform.localScale.x < baseScale.x / 10) { yield break; }
            transform.localScale -= new Vector3(scale / 60, scale /60);
            yield return new WaitForSeconds(LifeTime / 30);
            StartCoroutine(ShrinkOverTime(scale));
        }
    }

}
