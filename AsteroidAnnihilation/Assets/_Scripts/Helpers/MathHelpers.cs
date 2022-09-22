using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public static class MathHelpers
    {
        public static float RoundToDecimal(float value, int decimals)
        {
            float multiplier = Mathf.Pow(10, decimals);
            return Mathf.Round(value * multiplier) / multiplier;
        }
    }

}
