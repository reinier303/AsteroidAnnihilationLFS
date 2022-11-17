using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PermanencePart : MonoBehaviour
    {
        private ObjectPooler objectPooler;
        private Vector2 baseScale;

        private void Awake()
        {
            baseScale = transform.localScale;
        }

        /*
        private void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }
        */
        public void InitializePart(List<Sprite> sprites, float power, float scaleFactor)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
            Vector2 randomForce = new Vector2(Random.Range(-power, power), Random.Range(-power, power));
            GetComponent<Rigidbody2D>().AddForce(randomForce);
            transform.localScale = baseScale * scaleFactor;
        }

        /*
        private IEnumerator Explode(float scaleFactor)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            GameObject explosion = objectPooler.SpawnFromPool("SwarmExplosion2Small", transform.position, transform.rotation);
            explosion.transform.localScale = baseScale * scaleFactor * 4;
            gameObject.SetActive(false);
        }
        */
    }
}