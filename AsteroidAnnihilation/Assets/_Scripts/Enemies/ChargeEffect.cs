using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ChargeEffect : MonoBehaviour
    {
        private ParticleSystem particleSystem;
        private Transform poolParent;
        private Vector2 baseScale;

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
            poolParent = transform.parent;
            baseScale = transform.localScale;
        }

        public void Initialize(float lifeTime, float size)
        {
            StartCoroutine(DisableAfterTime(lifeTime));
            transform.localScale = baseScale * size;
            particleSystem.Play();
        }

        private IEnumerator DisableAfterTime(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            particleSystem.Stop();
            yield return new WaitForSeconds(1.5f);
            transform.SetParent(poolParent);
            gameObject.SetActive(false);
        }
    }
}