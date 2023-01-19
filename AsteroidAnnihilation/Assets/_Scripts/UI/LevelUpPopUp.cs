using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class LevelUpPopUp : MonoBehaviour
    {
        [SerializeField] protected Transform Follow;
        RectTransform rect;
        protected Camera cam;
        protected TextMeshProUGUI textComponent;
        [SerializeField] protected LeanTweenType tweenType1, tweenType2;

        public float StartHeight = 50;

        public float TweenTime1, TweenTime2, WaitTime;

        public Vector3 offset;

        protected void Awake()
        {
            Follow = Player.Instance.transform;
            cam = Camera.main;
            rect = GetComponent<RectTransform>();
        }

        protected void OnEnable()
        {
            transform.localPosition = new Vector3(0, StartHeight, 0);
            transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(Effect());
        }

        protected virtual IEnumerator Effect()
        {
            LeanTween.scale(rect, new Vector3(1, 1, 1), TweenTime1).setEase(tweenType1);
            yield return new WaitForSeconds(TweenTime1 + WaitTime);
            LeanTween.scale(rect, Vector2.zero, TweenTime2).setEase(tweenType2);
            yield return new WaitForSeconds(TweenTime2);


            gameObject.SetActive(false);
        }
    }
}
