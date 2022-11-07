using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Bounty Hunter Levels", order = 1001)]
    public class BountyHunterLevelSettings : SerializedScriptableObject
    {
        public List<BountyHunterLevel> BountyHunterLevels;
    }

    public struct BountyHunterLevel
    {
        public int Level;
        public float TotalExp;
        //Fill this with useful data later
        public List<string> Unlocks;
    }
}
