using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "DropTable", order = 993)]
    public class DropTable : SerializedScriptableObject
    {
        /// <summary>
        /// Random range for amount of drops from table dropped
        /// </summary>
        public Vector2Int DropRange;
        public Dictionary<Drop, float> Drops;
    }

    public struct Drop
    {
        /// <summary>
        /// Random range for amount of specified item dropped
        /// </summary>
        public Vector2Int AmountRange;
        public Item Item;
    }
}
