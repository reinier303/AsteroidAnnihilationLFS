using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ArrowIndicator : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            LeanTween.moveLocalY(gameObject, -325, 1);
            yield return new WaitForSeconds(1);
            LeanTween.moveLocalY(gameObject, -275, 1);
            yield return new WaitForSeconds(1);
            StartCoroutine(Move());
        }
    }
}
