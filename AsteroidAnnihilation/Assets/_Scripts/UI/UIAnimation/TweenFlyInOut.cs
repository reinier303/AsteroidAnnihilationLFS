using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenFlyInOut : MonoBehaviour
{
    RectTransform rectTransform;
    public float Time = 1;
    public LeanTweenType inTweenType;
    public LeanTweenType outTweenType;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StopCoroutine(StartTween());
        StartCoroutine(StartTween());
    }

    private IEnumerator StartTween()
    {
        rectTransform.anchoredPosition = new Vector2(0, 650);
        rectTransform.LeanMoveLocalY(0, Time).setEase(inTweenType);
        yield return new WaitForSeconds(Time * 2);
        rectTransform.LeanMoveLocalY(650, Time).setEase(outTweenType);
        yield return new WaitForSeconds(Time);
        gameObject.SetActive(false);
    }
}
