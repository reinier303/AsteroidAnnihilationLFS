using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class BaseSkill : ScriptableObject
    {
        public string skillName;
        public int Id;
        public int Cost;
        public BaseSkill Prerequisite;

        public virtual bool HasPrerequisite(PlayerStats playerStats)
        {
            if (Prerequisite == null || playerStats.HasSkillNode(Prerequisite.Id)) { return true; }
            else { return false; }
        }

        public virtual void UnlockSkill(PlayerStats playerStats)
        {
            //This method is meant to be overridden
            //Put unlock logic here
        }
    }

}