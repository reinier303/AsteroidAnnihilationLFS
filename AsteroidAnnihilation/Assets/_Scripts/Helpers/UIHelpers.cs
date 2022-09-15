using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class UIHelpers : MonoBehaviour
    {
        /// <summary>
        /// Use for buttons inside parent UI to deactivate or activate everything besides the button itself
        /// </summary>
        /// <param name="buttonObject"></param>
        public static void InParentActivationButton(Transform buttonTransform, bool open = false)
        {
            buttonTransform.parent.GetComponent<Image>().enabled = open;
            for(int i = 0; i < buttonTransform.parent.childCount; i++)
            {
                if(buttonTransform.parent.GetChild(i) == buttonTransform)
                {
                    continue;
                }

                buttonTransform.parent.GetChild(i).gameObject.SetActive(open);
            }
        }
    }
}

