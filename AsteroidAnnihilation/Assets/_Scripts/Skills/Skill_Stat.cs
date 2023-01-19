using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Skills/Skill_Stat", order = 999)]
    public class Skill_Stat : BaseSkill
    {
        //TODO::Remove Seperate Stat/Value and put everything into Stats List
        public EnumCollections.Stats Stat;
        public float Value;
        public List<SkillStatData> Stats;

        public override void UnlockSkill(PlayerStats playerStats)
        {
            base.UnlockSkill(playerStats);
            if(Id == 0) { Id = this.GetInstanceID(); }
            playerStats.AddSkillUnlocked(Id, Stat, Value);
            //This method is meant to be overridden
            //Put unlock logic here
        }
    }

    [System.Serializable]
    public struct SkillStatData
    {
        public EnumCollections.Stats Stat;
        public float Value;
    }
}