using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AsteroidAnnihilation;
using Newtonsoft.Json.Bson;

public class ExpPopUp : MonoBehaviour
{
    private UIManager uiManager;
    RectTransform rect;
    protected Camera cam;
    protected TextMeshProUGUI textComponent;
    [SerializeField] protected LeanTweenType tweenType;
    private Transform poolParent;

    public float TweenTime;

    protected void Awake()
    {
        uiManager = UIManager.Instance;
        poolParent = transform.parent;
        rect = GetComponent<RectTransform>();
        textComponent = GetComponent<TextMeshProUGUI>();
        cam = Camera.main;
    }

    public void Initialize(float exp)
    {
        textComponent.text = "+" + exp + "XP";
        transform.SetParent(uiManager.ExpHolder);
        //Vector3.zero is not zero??? So we have to do with this offset
        transform.localPosition = new Vector3(100, 25, 0);
        transform.localScale = new Vector3(1, 1, 1);

        textComponent.alpha = 1;
        StartCoroutine(UpdatePositionOnScreen());
    }

    protected virtual IEnumerator UpdatePositionOnScreen()
    {
        //LeanTween.moveLocalY(gameObject, -20, TweenTime).setEase(tweenType);
        LeanTween.value(gameObject,1 ,0, TweenTime).setOnUpdate(UpdateTextAlpha).setEase(tweenType);

        yield return new WaitForSeconds(TweenTime + 0.1f);

        transform.SetParent(poolParent);
        gameObject.SetActive(false);
    }

    private void UpdateTextAlpha(float value)
    {
        textComponent.alpha = value;
    }
}
