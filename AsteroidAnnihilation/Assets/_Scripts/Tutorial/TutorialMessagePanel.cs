using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class TutorialMessagePanel : MonoBehaviour, IPointerClickHandler
    {
        private TutorialManager tutorialManager;

        private void Start()
        {
            tutorialManager = TutorialManager.Instance;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tutorialManager.ClickMessagePanel();
        }
    }
}
