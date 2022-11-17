using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void UpdateEnergy(float currentValue, float MaxValue)
        {
            float fillAmount = (currentValue / MaxValue) / 2;
            fillImage.fillAmount = fillAmount;
        }
    }
}

