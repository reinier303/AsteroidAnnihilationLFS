using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/FX Settings", order = 900)]
    public class FXSettings : SerializedScriptableObject
    {
        public Dictionary<EnumCollections.ExplosionFX, string> ExplosionFxs;
        public Dictionary<EnumCollections.PermanenceSprites, List<Sprite>> PermanenceSprites;
    }
}