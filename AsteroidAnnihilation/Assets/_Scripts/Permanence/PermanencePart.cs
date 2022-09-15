using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PermanencePart : MonoBehaviour
    {
        public void InitializePart(List<Sprite> sprites, float power, float scaleFactor)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
            Vector2 randomForce = new Vector2(Random.Range(-power, power), Random.Range(-power, power));
            GetComponent<Rigidbody2D>().AddForce(randomForce);
            transform.localScale *= scaleFactor;
        }
    }
}