using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsteroidAnnihilation
{
    public class MissionCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI area;
        [SerializeField] private TextMeshProUGUI objective;
        [SerializeField] private TextMeshProUGUI units;
        [SerializeField] private TextMeshProUGUI experience;

        public void InitializeCard(Mission mission)
        {
            title.text =  mission.Faction.ToString() + " - " + mission.Objectives[0].ObjectiveType.ToString();
            area.text = "Area:\n" + mission.AreaName;
            objective.text = "Objective:\n" + AreaHelper.GetMissionTypeDescription(mission.Objectives[0].ObjectiveType) + mission.Objectives[0].ObjectiveAmount;
            units.text = "Units: " + mission.UnitsReward;
            experience.text = "XP: " + mission.ExperienceReward;
        }

        public void OnButtonPressed()
        {
            //We take the Rank bar into account by subtracting 1.
            MissionManager.Instance.StartCoroutine(MissionManager.Instance.MoveToMissionArea(transform.GetSiblingIndex() - 1));
        }
    }
}
