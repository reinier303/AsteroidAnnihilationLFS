using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Bounty Hunter Levels", order = 1001)]
    public class BountyHunterLevelSettings : SerializedScriptableObject
    {
        public List<BountyHunterRank> BountyHunterLevels;
    }

    public struct BountyHunterRank
    {
        public string RankName;
        public float TotalExp;
        //Fill this with useful data later
        public List<BaseUnlockable> Unlocks;
    }
}
