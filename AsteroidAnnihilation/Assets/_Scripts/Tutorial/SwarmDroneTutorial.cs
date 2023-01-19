using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmDroneTutorial : SwarmDrone
    {
        [SerializeField] private float delay;
        private Collider2D collider;
        private Material material;
        private bool spawned;

        protected override void OnEnable()
        {
            spawned = false;
            collider = GetComponent<Collider2D>();
            material = spriteRenderer.material;
            base.OnEnable();
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            material.SetFloat("_Fade", 0);
            yield return new WaitForSeconds(delay);
            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate(SetMaterialFadeValue).setEase(LeanTweenType.easeOutSine);
            yield return new WaitForSeconds(1);
            collider.enabled = true;
            spawned = true;
        }

        protected override void Update()
        {
            if (!spawned) { return; }
            base.Update();
        }

        private void SetMaterialFadeValue(float value)
        {
            material.SetFloat("_Fade", value);
        }

        protected override void Die()
        {
            SpawnSegments();
            SpawnParticleEffect();
            DropUnits(DroppedUnits);
            DropPickUps();
            gameManager.RPlayer.RPlayerStats.AddToExperience(ExperienceGained);
            cameraManager.StartCoroutine(cameraManager.Shake(ShakeDuration, ShakeMagnitude));
            TutorialManager.Instance.TargetDestroyed();
            gameObject.SetActive(false);
        }
    }
}
