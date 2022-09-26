using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Player Ship Settings", order = 1003)]
    public class PlayerShipSettings : SerializedScriptableObject
    {
        public Dictionary<EnumCollections.ShipType, List<Vector2>> WeaponPositions;

        public List<Vector2> GetWeaponPositions(EnumCollections.ShipType shipType)
        {
            return WeaponPositions[shipType];
        }
    }
}
