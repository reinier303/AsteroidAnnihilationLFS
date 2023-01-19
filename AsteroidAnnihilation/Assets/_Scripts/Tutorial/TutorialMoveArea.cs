using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class TutorialMoveArea : MonoBehaviour
    {
        private UIManager uiManager;
        private AudioManager audioManager;
        private TutorialManager tutorialManager;

        [SerializeField] private GameObject nextArea;
        [SerializeField] private GameObject effect;
        private Material material;

        private bool back;
        // Start is called before the first frame update
        void Start()
        {
            uiManager = UIManager.Instance;
            audioManager = AudioManager.Instance;
            tutorialManager = TutorialManager.Instance;
            back = false;
            StartCoroutine(Pulse());
        }

        private void OnEnable()
        {
            material = GetComponent<SpriteRenderer>().material;
            FadeIn();
        }

        private IEnumerator Pulse()
        {
            float alpha = back ? 0.6f : 0.35f;
            LeanTween.alpha(gameObject, alpha, 2f).setEase(LeanTweenType.easeInOutSine);
            yield return new WaitForSeconds(2f);
            back = !back;
            StartCoroutine(Pulse());
        }

        private void FadeIn()
        {
            //effect.SetActive(true);
            LeanTween.value(gameObject, 0, 1, 0.75f).setOnUpdate(SetMaterialFadeValue).setEase(LeanTweenType.easeOutSine);
        }

        private IEnumerator FadeOut()
        {
            //effect.SetActive(true);
            LeanTween.value(gameObject, 1, 0, 0.75f).setOnUpdate(SetMaterialFadeValue).setEase(LeanTweenType.easeOutSine);
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
        }

        private void SetMaterialFadeValue(float value)
        {
            material.SetFloat("_Fade", value);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if(collision.tag == "Player")
            {
                Debug.Log(collision.name);
                audioManager.PlayAudio("Tutorial_MovementPlane");
                LeanTween.cancel(gameObject);
                StopAllCoroutines();
                StartCoroutine(FadeOut());
                if (nextArea != null)
                {
                    nextArea.SetActive(true);
                }
                else
                {
                    //Tutorial done
                    tutorialManager.ShowNextTutorialMessage();
                    tutorialManager.EnableTargets();
                }
            }
        }
    }
}