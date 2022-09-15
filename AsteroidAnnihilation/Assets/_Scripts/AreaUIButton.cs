using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class AreaUIButton : Button_Custom
    {
        public Vector2Int area;
        public AreaTooltip Tooltip;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            EnableTooltip();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            DisableTooltip();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            MoveToArea();
        }

        private void EnableTooltip()
        {
            Tooltip.UpdateTooltip(area);
            Tooltip.gameObject.SetActive(true);
        }

        private void DisableTooltip()
        {
            Tooltip.gameObject.SetActive(false);
        }

        private void MoveToArea()
        {
            AreaManager.Instance.SetCurrentArea(area.x, area.y , true);
        }
    }
}
