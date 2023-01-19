using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class Target : BaseEntity
    {
        private Material material;
        [SerializeField] private float delay;
        private Collider2D collider;

        [FoldoutGroup("Movement")][SerializeField] private bool move;
        [FoldoutGroup("Movement")][SerializeField] private bool inverseMove;
        [FoldoutGroup("Movement")][SerializeField] private bool orbit;
        [FoldoutGroup("Movement")][SerializeField] private float unitsPerSecond;
        [FoldoutGroup("Movement")][SerializeField] private float rotationSpeed;

        private Vector2 startPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            startPosition = transform.position;
            material = GetComponent<SpriteRenderer>().material;
            collider = GetComponent<Collider2D>();
            collider.enabled = false;
            material.SetFloat("_Fade", 0);
            StartCoroutine(FadeIn());
            if (move) { StartCoroutine(MoveUpAndDown(inverseMove ? false : true)); }
        }

        protected virtual void Update()
        {
            if(orbit){ Orbit(); }
        }

        private void Orbit()
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        private IEnumerator MoveUpAndDown(bool up)
        {
            float target = up ? startPosition.y + 5 : startPosition.y - 5;
            float distance = Mathf.Abs(transform.position.y - target);
            float time = distance / unitsPerSecond;

            Debug.Log(target + ", " + distance +", "+ time);
            LeanTween.moveLocalY(gameObject, target, time).setEase(LeanTweenType.easeInOutSine);
            yield return new WaitForSeconds(time);
            StartCoroutine(MoveUpAndDown(!up));
        }

        protected override void Die()
        {
            //SpawnParticleEffect();
            TutorialManager.Instance.TargetDestroyed();
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            yield return new WaitForSeconds(delay);
            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate(SetMaterialFadeValue).setEase(LeanTweenType.easeOutSine);
            yield return new WaitForSeconds(1);
            collider.enabled = true;
        }

        private IEnumerator FadeOut()
        {
            //effect.SetActive(true);
            LeanTween.value(gameObject, 1, 0, 0.25f).setOnUpdate(SetMaterialFadeValue).setEase(LeanTweenType.easeOutSine);
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
        }

        private void SetMaterialFadeValue(float value)
        {
            material.SetFloat("_Fade", value);
        }
    }
}
