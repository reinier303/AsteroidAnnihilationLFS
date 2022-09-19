﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameObject LoadingScreen;

        [Header("UI Components")]
        public TMP_Text LivesText;
        public TMP_Text UnitsText;
        [SerializeField] private TMP_Text playerLevelText;
        public TMP_Text WaveEnterText;
        public TMP_Text BossEnterText;

        [Header("Missions")]
        public TMP_Text MissionAreaText;
        [SerializeField] private GameObject MissionComplete;

        public TMP_Text AreaText;

        private CanvasGroup waveEnterCanvasGroup;
        private CanvasGroup bossEnterCanvasGroup;

        public HealthBar PlayerHealthBar;

        //Powerup
        public Slider PowerUpTimer;
        public Slider ExperienceBar;
        public TMP_Text PowerUpText;
        public Slider BossBar;
        private int currentID;

        public GameObject PostGamePanel;
        public GameObject PauseMenu;

        [Header("Objectives")]
        public GameObject ObjectiveMenu;
        public Transform ObjectivesPanel;
        [SerializeField] private GameObject ObjectiveUI;
        private List<TMP_Text> ObjectiveTexts;

        public Image HitVignette;
        public Image BossIcon;

        [Header("BounceTweenValues")]
        public float BounceTime;
        public float BounceSize;
        public LeanTweenType BounceType;

        [Header("On Hit Vignette Values")]
        public float VignetteDuration;
        public float AlphaTo;
        public LeanTweenType EaseType;

        [Header("Wave Text Values")]
        public float WaveTextDuration;
        public float AlphaToWave;
        public LeanTweenType EaseTypeWave;

        [Header("Script References")]

        public PostGamePanel PostGamePanelScript;

        private GameManager gameManager;
        private MissionManager missionManager;
        private PlayerEntity RPlayerEntity;
        private PlayerStats playerStats;
        private Player RPlayer;

        public delegate void OnPauseMenuOpen();
        public OnPauseMenuOpen OnOpenPauseMenu;

        public delegate void OnPauseMenuClose();
        public OnPauseMenuClose OnClosePauseMenu;

        public static GameObject currentHoveringButton;

        private Mission currentMission;

        private void Awake()
        {
            Instance = this;
            gameManager = GameManager.Instance;
            missionManager = MissionManager.Instance;
            RPlayer = gameManager.RPlayer;
            RPlayerEntity = RPlayer.RPlayerEntity;
            playerStats = RPlayer.RPlayerStats;

            waveEnterCanvasGroup = WaveEnterText.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            UpdateUnits();
            UpdateHealth();
            UpdateLevel();

            currentMission = missionManager.GetCurrentMission();
            InitializeMissionUI();
            InitializeObjectiveMenu();
        }

        public void UpdateMissionUI()
        {
            currentMission = missionManager.GetCurrentMission();
            InitializeMissionUI();
            InitializeObjectiveMenu();
        }

        public void OnMissionComplete()
        {
            MissionComplete.SetActive(true);
        }

        public void UpdateHealth()
        {
            float currentHealth = RPlayerEntity.currentHealth;
            float maxHealth = RPlayerEntity.MaxHealth.GetBaseValue();

            LivesText.text = currentHealth + "/" + maxHealth;
            PlayerHealthBar.UpdateHealth(currentHealth, maxHealth);
        }

        public void UpdateUnits()
        {
            UnitsText.text = "Units:" + playerStats.Stats["Units"].GetBaseValue();
            StopCoroutine(BounceSizeTween(UnitsText.gameObject));
            StartCoroutine(BounceSizeTween(UnitsText.gameObject));
        }

        public void UpdateExperience()
        {
            ExperienceBar.value = playerStats.GetExperienceLevelProgress();
        }

        public void UpdateLevel()
        {
            Debug.Log(playerStats.GetPlayerLevel());
            playerLevelText.text = "" + playerStats.GetPlayerLevel();
            ExperienceBar.maxValue = playerStats.GetExperienceDifference();
            UpdateExperience();
        }

        #region Powerups

        public void StartPowerupTimer(float seconds, string powerUpName)
        {
            PowerUpText.text = powerUpName;
            PowerupTimer(seconds);
        }

        public void PowerupTimer(float seconds)
        {
            PowerUpTimer.gameObject.SetActive(true);

            PowerUpTimer.maxValue = seconds;
            PowerUpTimer.value = seconds;

            if(LeanTween.isTweening(gameObject))
            {
                LeanTween.cancel(currentID);

            }

            currentID = LeanTween.value(gameObject, seconds, 0, seconds).setOnUpdate(UpdatePowerupTimer).setOnComplete(DisablePowerupTimer).id;
        }

        private void UpdatePowerupTimer(float value)
        {
            PowerUpTimer.value = value;
        }

        private void DisablePowerupTimer()
        {
            PowerUpTimer.gameObject.SetActive(false);
        }

        #endregion

        #region Areas

        public void InitializeObjectiveMenu()
        {
            for(int i = 0; i < ObjectivesPanel.childCount; i++)
            {
                Destroy(ObjectivesPanel.GetChild(i).gameObject);
            }
            AreaText.text = currentMission.AreaName;

            ObjectiveTexts = new List<TMP_Text>();
            for (int i = 0; i < currentMission.Objectives.Count; i++)
            {
                TMP_Text objectiveText = Instantiate(ObjectiveUI, ObjectivesPanel).GetComponent<TMP_Text>();
                objectiveText.text = AreaHelper.GetMissionTypeDescription(currentMission.Objectives[i].ObjectiveType) + currentMission.Objectives[i].ObjectiveProgress + "/" + currentMission.Objectives[i].ObjectiveAmount;
                ObjectiveTexts.Add(objectiveText);
            }
        }

        public void InitializeMissionUI()
        {
            StartCoroutine(ShowWaveText(currentMission.AreaName, currentMission.AreaTextColor, currentMission.AreaTextMaterial));
        }

        public void UpdateObjectives()
        {
            for (int i = 0; i < currentMission.Objectives.Count; i++)
            {
                ObjectiveTexts[i].text = AreaHelper.GetMissionTypeDescription(currentMission.Objectives[i].ObjectiveType) + currentMission.Objectives[i].ObjectiveProgress + "/" + currentMission.Objectives[i].ObjectiveAmount;
            }
        }

        #endregion

        private IEnumerator BounceSizeTween(GameObject uIElement)
        {
            uIElement.transform.localScale = new Vector2(1, 1);
            LeanTween.scale(uIElement, new Vector2(BounceSize, BounceSize), BounceTime).setEase(BounceType).setIgnoreTimeScale(true);
            yield return new WaitForSeconds(BounceTime);
            LeanTween.scale(uIElement, new Vector2(1, 1), BounceTime).setEase(BounceType).setIgnoreTimeScale(true);
        }

        public void OpenPauseMenu()
        {
            if(OnOpenPauseMenu != null)
            {
                OnOpenPauseMenu.Invoke();
            }
            PauseMenu.SetActive(true);
        }

        public void ClosePauseMenu()
        {
            OnClosePauseMenu.Invoke();
            PauseMenu.SetActive(false);
        }

        public void OnPlayerDeathUI()
        {
            PostGamePanel.SetActive(true);
            StartCoroutine(PostGamePanelScript.UpdateSummary());
        }

        public IEnumerator ShowWaveText(string waveName, Color textColor, Material textMaterial)
        {
            WaveEnterText.text = "<size=50>Now entering</size>\n" + waveName;
            WaveEnterText.color = textColor;
            WaveEnterText.material = textMaterial;
            LeanTween.alphaCanvas(waveEnterCanvasGroup, AlphaToWave, WaveTextDuration / 2).setEase(EaseTypeWave);
            yield return new WaitForSeconds(WaveTextDuration);
            LeanTween.alphaCanvas(waveEnterCanvasGroup, 0, WaveTextDuration / 2).setEase(EaseTypeWave);
        }

        public IEnumerator TweenAlpha(RectTransform rectTransform, float duration, float alphaTo, float alphaFrom)
        {
            LeanTween.alpha(rectTransform, AlphaTo, duration / 2).setEase(EaseType);
            yield return new WaitForSeconds(duration / 2);
            LeanTween.alpha(rectTransform, alphaFrom, duration / 2).setEase(EaseType);
        }

        #region Leaving Mission Area.. Might use in future
        /*
        public void ShowMissionAreaText()
        {
            MissionAreaText.gameObject.SetActive(true);
            if(!areaCountdown)
            {
                areaCountdownCoroutine = StartCoroutine(AreaCountDown());
            }
        }

        public void DisableMissionAreaText()
        {
            MissionAreaText.gameObject.SetActive(false);
            if(areaCountdownCoroutine != null)
            {
                StopCoroutine(areaCountdownCoroutine);
            }
            areaCountdown = false;
        }

        private IEnumerator AreaCountDown()
        {
            areaCountdown = true;

            int seconds = 3;

            MissionAreaText.text = "Leaving Mission Area...\nYou will be teleported back in\n<size=100>" + seconds;
            for(int i = 1; i < seconds + 1; i++)
            {
                yield return new WaitForSeconds(1f);
                MissionAreaText.text = "Leaving Mission Area...\nYou will be teleported back in\n<size=100>" + (seconds - i);
            }

            gameManager.TeleportPlayerToStart();

            areaCountdown = false;
        }
        */
        #endregion
    }
}