using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class LoadingScreen : MonoBehaviour
    {
        public float FadeTime;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            FadeInLoadingScren();
        }

        private void FadeInLoadingScren()
        {
            canvasGroup.alpha = 0;
            //Fade music volume maybe?
            canvasGroup.LeanAlpha(1, FadeTime).setIgnoreTimeScale(true);
        }

        public IEnumerator FadeOutLoadingScreen()
        {
            canvasGroup.alpha = 1;
            //Fade music volume maybe?
            canvasGroup.LeanAlpha(0, FadeTime).setIgnoreTimeScale(true);
            yield return new WaitForSecondsRealtime(FadeTime);
            gameObject.SetActive(false);
        }

    }

}
