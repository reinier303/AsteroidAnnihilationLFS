using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UIManager uIManager;
        private EquipmentData equipment;
        private Image icon;

        private void Start()
        {
            uIManager = UIManager.Instance;
        }

        public void InitializeSlot(EquipmentData equip)
        {
            equipment = equip;
            icon.sprite = equip.ItemData.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
