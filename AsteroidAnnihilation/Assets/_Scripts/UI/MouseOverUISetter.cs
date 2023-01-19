using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    /// <summary>
    /// This script will set UIManagers MouseOverUI boolean to true on mouse enter and vice versa on mouse exit.
    /// Use this on UI objects that should not allow firing when hovering over it.
    /// </summary>
    public class MouseOverUISetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UIManager uiManager;

        private void Start()
        {
            uiManager = UIManager.Instance;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            uiManager.MouseOverUI = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiManager.MouseOverUI = false;
        }

        private void OnDisable()
        {
            if (uiManager != null) { uiManager.MouseOverUI = false; }
        }
    }
}