using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BaseUnlockable : ScriptableObject
    {
        public virtual void Unlock()
        {
            //This method is meant to be overridden.
        }
    }

}
