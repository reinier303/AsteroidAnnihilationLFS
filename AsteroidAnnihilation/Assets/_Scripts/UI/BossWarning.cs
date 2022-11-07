using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class BossWarning : MonoBehaviour
    {
        public float TweenInTime;
        public float TweenOutTime;
        public LeanTweenType TweenInType;
        public LeanTweenType TweenOutType;
        public float OnScreenTime;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            transform.localScale = Vector2.zero;
            StartCoroutine(LerpInAndPulse());
        }

        private IEnumerator LerpInAndPulse()
        {
            LeanTween.scale(gameObject, Vector2.one, TweenInTime).setEase(TweenInType);
            float timer = 0;
            while(timer < OnScreenTime)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha =  0.75f + Mathf.PingPong(Time.time / 2f, 0.25f);
                yield return new WaitForEndOfFrame();
            }
            LeanTween.scale(gameObject, Vector2.zero, TweenOutTime).setEase(TweenOutType);
            yield return new WaitForSeconds(TweenOutTime + 1f);
            gameObject.SetActive(false);
        }
    }
}

