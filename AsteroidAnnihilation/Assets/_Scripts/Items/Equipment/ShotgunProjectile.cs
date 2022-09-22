using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ShotgunProjectile : PlayerProjectile
    {
        private Vector2 baseScale;
        protected override void Awake()
        {
            base.Awake();
            baseScale = transform.localScale;
        }

        public override void Initialize(float size)
        {
            base.Initialize(size);
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
