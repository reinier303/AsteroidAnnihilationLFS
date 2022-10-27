using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class DisableAfterTime : MonoBehaviour
    {
        public void Disable(float seconds)
        {
            StartCoroutine(DisableAfter(seconds));
        }

        private IEnumerator DisableAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(false);
        }
    }
}
