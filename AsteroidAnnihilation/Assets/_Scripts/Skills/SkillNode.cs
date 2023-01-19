using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class SkillNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private PlayerStats playerStats;
        private AudioManager audioManager;
        private UIManager uiManager;
        private StatManager statManager;

        [SerializeField] private BaseSkill skill;
        [SerializeField] private GameObject unlockedImage;
        [SerializeField] private GameObject availableImage;

        private bool canClick;
        private RectTransform rectTransform;

        private void Start()
        {
            playerStats = Player.Instance.RPlayerStats;
            audioManager = AudioManager.Instance;
            uiManager = UIManager.Instance;
            statManager = StatManager.Instance;
            rectTransform = GetComponent<RectTransform>();
            CheckUnlocked();
        }

        private void CheckUnlocked()
        {
            canClick = true;
            if (playerStats.HasSkillNode(skill.Id)) { unlockedImage.SetActive(true);
            canClick = false;}
            else { unlockedImage.SetActive(false); }

            if (skill.HasPrerequisite(playerStats)) { availableImage.SetActive(true);}
            else { availableImage.SetActive(false); }
        }

        public virtual void ClickSkill()
        {
            if (!canClick) { return; }
            if (playerStats.TryUnlockSkill(skill.Cost) && skill.HasPrerequisite(playerStats))
            {
                skill.UnlockSkill(playerStats);
                audioManager.PlayAudio("SkillUnlock");
                statManager.OnOffensiveStatsChanged();
                statManager.OnDefensiveStatsChanged();
                //statManager.OnMovementStatsChanged();

                unlockedImage.SetActive(true);
                canClick = false;
            }
            else
            {
                //TODO::Add cant afford animation
                audioManager.PlayAudio("CantAfford");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            uiManager.SetSkillTooltip((Skill_Stat)skill, rectTransform.anchoredPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiManager.HideSkillTooltip();
        }
    }
}

