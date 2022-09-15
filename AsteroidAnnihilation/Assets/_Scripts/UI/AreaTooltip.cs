using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsteroidAnnihilation
{
    public class AreaTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI areaName;
        [SerializeField] private TextMeshProUGUI objectives;
        [SerializeField] private TextMeshProUGUI rewards;
        private AreaManager areaManager;

        private void Start()
        {
            areaManager = AreaManager.Instance;
        }

        public void UpdateTooltip(Vector2Int area)
        {
            AreaData data = areaManager.GetAreaData(area.x, area.y);
            areaName.text = data.AreaName;
            string objectivesText = "";
            for (int i = 0; i < data.Objectives.Count; i++)
            {
                objectivesText += AreaHelper.GetMissionTypeDescription(data.Objectives[i].ObjectiveType) + data.Objectives[i].ObjectiveProgress + "/" + data.Objectives[i].ObjectiveAmount + "\n";
            }
            string rewardsText = "";
            for (int i = 0; i < data.CompletionRewards.Count; i++)
            {
                rewardsText += AreaHelper.GetCompletionRewardDescription(data.CompletionRewards[i]) + "\n";
            }
            objectives.text = objectivesText;
            rewards.text = rewardsText;
        }
    }
}