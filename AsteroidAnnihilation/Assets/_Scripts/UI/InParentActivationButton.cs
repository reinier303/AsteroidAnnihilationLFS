using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class InParentActivationButton : MonoBehaviour
    {
        private bool Open;

        private void Start()
        {
            Open = false;
        }

        public void Click()
        {
            UIHelpers.InParentActivationButton(transform, Open);
            Open = !Open;
        }
    }
}
