using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] protected Transform Follow;
    RectTransform rect;
    protected Camera cam;
    protected TextMeshProUGUI textComponent;
    [SerializeField] protected LeanTweenType tweenType;

    //Speed at which text moves upward
    public float Speed = 50;

    //Height to start at
    public float StartHeight = 50;

    //How much to move up
    public float MaxTranslation = 50;

    public float TweenTime;

    protected void Awake()
    {
        rect = GetComponent<RectTransform>();
        textComponent = GetComponent<TextMeshProUGUI>();
        cam = Camera.main;
    }

    public void FollowDamagedObject(Transform objectToFollow, Vector3 offset, float damage)
    {
        textComponent.text = "" + damage;
        StartCoroutine(UpdatePositionOnScreen(objectToFollow, offset));
    }

    protected void OnEnable()
    {
        transform.position = new Vector2(0, StartHeight);
        transform.localScale = new Vector3(1, 1, 1);
    }

    protected virtual IEnumerator UpdatePositionOnScreen(Transform objectToFollow, Vector3 offset)
    {
        Follow = objectToFollow;
        float translation = 0;
        LeanTween.scale(rect, Vector2.zero, TweenTime).setEase(tweenType);
        while (translation <= MaxTranslation)
        {
            yield return new WaitForEndOfFrame();
            translation += Mathf.Sin(Time.deltaTime) * Speed;
            Vector2 newPosition = cam.WorldToScreenPoint(objectToFollow.position);
            transform.position = newPosition + new Vector2(0, StartHeight + translation) + (Vector2)offset;
        }
   
        gameObject.SetActive(false);
    }
}
