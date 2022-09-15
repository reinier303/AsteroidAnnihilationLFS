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
            FadeLoadingScren();
        }

        private void FadeLoadingScren()
        {
            canvasGroup.alpha = 0;
            GameManager.Instance.RPlayer.GetComponent<InputManager>().InputEnabled = false;
            //Fade music volume maybe?
            canvasGroup.LeanAlpha(1, FadeTime);
        }

    }

}
