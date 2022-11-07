using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace AsteroidAnnihilation
{
    public class SecretTip : MonoBehaviour, IPointerClickHandler
    {
        private TextMeshProUGUI secretTip;

        // Start is called before the first frame update
        void Awake()
        {
            secretTip = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Achievement unlocked
            if (secretTip.fontSize == 16)
            {
                secretTip.fontSize = 1.5f;
            }     
            else { secretTip.fontSize = 16; }
        }
    }
}
