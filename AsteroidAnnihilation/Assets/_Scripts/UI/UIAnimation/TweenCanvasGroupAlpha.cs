using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class TweenCanvasGroupAlpha : MonoBehaviour
    {
        public float Time, Delay, AlphaTo, AlphaFrom;

        public bool IgnoreTimeScale;

        public LeanTweenType TweenType;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Start is called before the first frame update
        private void OnEnable()
        {
            canvasGroup.alpha = AlphaFrom;
            LeanTween.alphaCanvas(canvasGroup, AlphaTo, Time).setDelay(Delay).setEase(TweenType).setIgnoreTimeScale(IgnoreTimeScale);
        }
    }
}