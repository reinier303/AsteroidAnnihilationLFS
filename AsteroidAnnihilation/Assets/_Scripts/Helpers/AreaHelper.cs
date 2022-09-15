using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public static class AreaHelper
    {
        public static string GetMissionTypeDescription(MissionObjectiveType type)
        {
            switch (type)
            {
                case MissionObjectiveType.Defense:
                    return "- Defend the Objective: ";
                case MissionObjectiveType.Elimination:
                    return "- Kill Enemies: ";
                case MissionObjectiveType.Survival:
                    return "- Survive: ";
                /*case MissionObjectiveType.PointCapture:
                    return "- Capture points: ";*/
            }
            return "No objective found";
        }

        public static string GetCompletionRewardDescription(StatCompletionReward reward)
        {
            if(reward.IsWeapon)
            {
                return "+" + reward.RewardAmount + " " + reward.WeaponType + " " + reward.WeaponStatType;
            }
            else
            {
                return "+" + reward.RewardAmount + " " + reward.PlayerStatType;
            }
        }
    }
}
