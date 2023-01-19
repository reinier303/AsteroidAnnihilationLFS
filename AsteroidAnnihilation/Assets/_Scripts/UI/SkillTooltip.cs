using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsteroidAnnihilation
{
    public class SkillTooltip : MonoBehaviour
    {
        private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillStatsText;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }


        public void SetTooltip(Skill_Stat skill, Vector2 position)
        {
            string skillName = skill.skillName;
            string skillStats = "+" + skill.Value + " " + skill.Stat.ToString();
            skillNameText.text = skillName;
            skillStatsText.text = skillStats;
            rectTransform.anchoredPosition = new Vector2(position.x - 35, position.y + 120f);
        }
    }
}