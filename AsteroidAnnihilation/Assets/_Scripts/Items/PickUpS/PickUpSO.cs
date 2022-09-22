using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Inventory/PickUp", order = 998)]
    public class PickUpSO : ScriptableObject
    {
        public string PickUpName;
        public int Value;
        public Sprite sprite;
    }

}
