using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Player Levels", order = 1001)]
    public class PlayerLevelSettings : SerializedScriptableObject
    {
        public List<PlayerLevel> PlayerLevels;
    }

    public struct PlayerLevel
    {
        public int Level;
        public float TotalExp;
        //Fill this with useful data later
        public List<BaseUnlockable> Unlocks;
    }
}
