using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StayInside : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        text.alpha = 1;
        LeanTween.value(gameObject, 1, 0, 3).setOnUpdate(UpdateAlpha);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void UpdateAlpha(float value)
    {
        text.alpha = value;

    }
}
