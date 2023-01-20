using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AsteroidAnnihilation
{
    public class MouseButtonPulse : MonoBehaviour
    {
        [SerializeField] private float Time;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        // Start is called before the first frame update
        private void OnEnable()
        {
            StartCoroutine(Pulse());
        }

        private IEnumerator Pulse()
        {
            LeanTween.value(gameObject, image.color.a, 0.6f, Time).setOnUpdate(ChangeAlpha);
            yield return new WaitForSeconds(Time);
            LeanTween.value(gameObject, image.color.a, 1, Time).setOnUpdate(ChangeAlpha);
            yield return new WaitForSeconds(Time);
            StartCoroutine(Pulse());
        }

        private void ChangeAlpha(float value)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, value);
        }
    }
}
