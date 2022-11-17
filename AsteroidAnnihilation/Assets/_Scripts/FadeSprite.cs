using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class FadeSprite : MonoBehaviour
    {
        private SpriteRenderer sprite;
        [SerializeField] private float fadeTime;
        private float currentTime;
        [SerializeField] private AnimationCurve fadeCurve;
        private Color spriteBaseColor;

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            spriteBaseColor = sprite.color;
        }

        private void OnEnable()
        {
            currentTime = 0;
            sprite.color = new Color(spriteBaseColor.r, spriteBaseColor.g, spriteBaseColor.b, 0.7f);

            StartCoroutine(Fade());
        }
        // Update is called once per frame
        private IEnumerator Fade()
        {
            float evaluation = currentTime / fadeTime;
            currentTime += Time.deltaTime;
            sprite.color = new Color(spriteBaseColor.r, spriteBaseColor.g, spriteBaseColor.b, fadeCurve.Evaluate(evaluation));
            yield return new WaitForEndOfFrame();
            if(sprite.color.a < 0.01f)
            {
                gameObject.SetActive(false);
                yield break;
            }
            StartCoroutine(Fade());
        }
    }
}
