using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class MenuOpener : MonoBehaviour
    {
        private int direction;
        [SerializeField] private float translation;

        //Public Variables
        public float Time = 1;
        public LeanTweenType Ease;

        private bool onCooldown;
        private bool open;

        private void Start()
        {
            direction = 1;
            //UIManager.Instance.OnClosePauseMenu += CloseMenu;
            //UIManager.Instance.OnOpenPauseMenu += OpenMenu;

            open = false;
        }

        public void MoveMenu()
        {
            if (onCooldown) {
                return;
            }

            LeanTween.moveLocalX(gameObject, transform.localPosition.x + translation * direction, Time).setEase(Ease).setIgnoreTimeScale(true);

            StartCoroutine(ExpandCooldown());
        }

        private void OpenMenu()
        {
            if (!open)
            {
                MoveMenu();
            }
        }

        private void CloseMenu()
        {
            if(open)
            {
                MoveMenu();
            }
        }

        private IEnumerator ExpandCooldown()
        {
            onCooldown = true;
            yield return new WaitForSecondsRealtime(Time);
            onCooldown = false;

            if (direction == 1) {
                open = true;
            }
            else {
                open = false;
            }

            direction *= -1;
        }

        public bool OnCooldown()
        {
            return onCooldown;
        }
    }
}

