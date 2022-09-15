using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField] public List<Sprite> Sprites;

        public void SetSprites(List<Sprite> sprites)
        {
            Sprites = sprites;
            GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, Sprites.Count)];
        }
    }
}

