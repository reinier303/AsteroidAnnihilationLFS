using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class HealthBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image fillImage;

        [SerializeField] private GameObject healthText;

        [SerializeField] private float rateOverTime;
        [SerializeField] private ParticleSystem pSystem;

        [SerializeField] private float glowAlpha;
        [SerializeField] private ParticleSystem glow;

        private void Start()
        {
            var emission = pSystem.emission;
            emission.rateOverTime = rateOverTime;

            var glowMain = glow.main;
            glowMain.startColor = new Color(glowMain.startColor.color.r, glowMain.startColor.color.g, glowMain.startColor.color.b, glowAlpha);
        }

        public void UpdateHealth(float currentValue, float MaxValue)
        {
            float fillAmount = currentValue / MaxValue;
            fillImage.fillAmount = fillAmount;

            var emission = pSystem.emission;
            emission.rateOverTime = (rateOverTime * fillAmount) * fillAmount;

            var glowMain = glow.main;
            glowMain.startColor = new Color(glowMain.startColor.color.r, glowMain.startColor.color.g, glowMain.startColor.color.b, glowAlpha * fillAmount);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            healthText.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            healthText.SetActive(false);
        }
    }
}
