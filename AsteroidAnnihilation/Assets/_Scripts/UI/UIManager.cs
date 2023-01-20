using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Bson;

namespace AsteroidAnnihilation
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameObject LoadingScreen;

        [FoldoutGroup("GUI Components")] public TMP_Text LivesText;
        [FoldoutGroup("GUI Components")] public TMP_Text UnitsText;
        [FoldoutGroup("GUI Components")][SerializeField] private TMP_Text playerLevelText;
        [FoldoutGroup("GUI Components")] public TMP_Text WaveEnterText;
        [FoldoutGroup("GUI Components")] public TMP_Text BossEnterText;
        [FoldoutGroup("GUI Components")] public HealthBar PlayerHealthBar;
        [FoldoutGroup("GUI Components")] public EnergyBar EnergyBar;
        [FoldoutGroup("GUI Components")] public Slider ExperienceBar;

        [FoldoutGroup("Missions")] public TMP_Text MissionAreaText;
        [FoldoutGroup("Missions")][SerializeField] private GameObject MissionComplete;
        //TODO:: Make this work as mission complete panel
        [FoldoutGroup("Missions")] public PostGamePanel PostGamePanelScript;

        [FoldoutGroup("Inventory")][SerializeField] private GameObject inventoryScreen;
        [FoldoutGroup("Inventory")][SerializeField] private EquipmentTooltip inventoryTooltip;
        [FoldoutGroup("Inventory")][SerializeField] private Transform inventoryPanel;
        [FoldoutGroup("Inventory")][SerializeField] private Transform weaponSlotParent;
        [FoldoutGroup("Inventory")][SerializeField] private Transform gearSlotParent;
        [FoldoutGroup("Inventory")][SerializeField] private Transform componentSlotParent;

        public TMP_Text AreaText;

        [FoldoutGroup("Boss")] private CanvasGroup waveEnterCanvasGroup;
        [FoldoutGroup("Boss")] private CanvasGroup bossEnterCanvasGroup;
        [FoldoutGroup("Boss")][SerializeField] private Slider bossBar;
        [FoldoutGroup("Boss")][SerializeField] private GameObject bossWarning;

        //Powerup
        public Slider PowerUpTimer;
        public TMP_Text PowerUpText;
        
        private int currentID;

        public GameObject PostGamePanel;
        public GameObject PauseMenu;

        [FoldoutGroup("Objectives")] public GameObject ObjectiveMenu;
        [FoldoutGroup("Objectives")] public Transform ObjectivesPanel;
        [FoldoutGroup("Objectives")][SerializeField] private GameObject ObjectiveUI;
        [FoldoutGroup("Objectives")] private List<TMP_Text> ObjectiveTexts;
     
        public Image BossIcon;

        [FoldoutGroup("BounceTweenValues")] public float BounceTime;
        [FoldoutGroup("BounceTweenValues")] public float BounceSize;
        [FoldoutGroup("BounceTweenValues")] public LeanTweenType BounceType;

        [FoldoutGroup("On Hit Vignette")] public Image HitVignette;
        [FoldoutGroup("On Hit Vignette")] public float VignetteDuration;
        [FoldoutGroup("On Hit Vignette")] public float AlphaTo;
        [FoldoutGroup("On Hit Vignette")] public LeanTweenType EaseType;

        [FoldoutGroup("Wave Text Values")] public float WaveTextDuration;
        [FoldoutGroup("Wave Text Values")] public float AlphaToWave;
        [FoldoutGroup("Wave Text Values")] public LeanTweenType EaseTypeWave;

        [FoldoutGroup("Skills")][SerializeField] private TextMeshProUGUI SkillPoints;
        [FoldoutGroup("Skills")][SerializeField] private SkillTooltip skillTooltip;

        [FoldoutGroup("Tutorial")][SerializeField] private TextMeshProUGUI TutorialText;
        [FoldoutGroup("Tutorial")][SerializeField] private GameObject StayInsideText;
        [FoldoutGroup("Tutorial")][SerializeField] private GameObject QuestionPrompt;
        [FoldoutGroup("Tutorial")][SerializeField] private GameObject LmbPrompt;
        [FoldoutGroup("Tutorial")][SerializeField] private GameObject FirstLevelUpPanel;
        [FoldoutGroup("Tutorial")][SerializeField] private GameObject IndicationArrow;


        //Script References
        private GameManager gameManager;
        private MissionManager missionManager;
        private InventoryManager inventoryManager;
        private PlayerEntity RPlayerEntity;
        private PlayerStats playerStats;
        private PlayerAttack playerAttack;

        private Player RPlayer;

        public delegate void OnPauseMenuOpen();
        public OnPauseMenuOpen OnOpenPauseMenu;

        public delegate void OnPauseMenuClose();
        public OnPauseMenuClose OnClosePauseMenu;

        public static GameObject currentHoveringButton;

        private Mission currentMission;

        private EventSystem eventSystem;

        public bool MouseOverUI = false;
        public bool MenuOpen;

        [SerializeField] private GameObject deathScreen;
        [SerializeField] private GameObject LevelUpText;
        [SerializeField] private GameObject Plus;
        public Transform ExpHolder;

        private void Awake()
        {
            Instance = this;
            gameManager = GameManager.Instance;
            missionManager = MissionManager.Instance;
            inventoryManager = InventoryManager.Instance;
            RPlayer = gameManager.RPlayer;
            RPlayerEntity = RPlayer.RPlayerEntity;
            playerStats = RPlayer.RPlayerStats;
            playerAttack = RPlayer.RPlayerAttack;
            waveEnterCanvasGroup = WaveEnterText.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            UpdateUnits();
            UpdateHealth(RPlayerEntity.currentHealth, RPlayerEntity.MaxHealth);
            UpdateLevel();
            UpdateSkillPoints();
            eventSystem = EventSystem.current;

            currentMission = missionManager.GetCurrentMission();
            InitializeMissionUI();
            InitializeObjectiveMenu();
            EquipmentManager.Instance.InitializeEquipment();
            inventoryManager.SetUIElements(inventoryPanel, weaponSlotParent, gearSlotParent, componentSlotParent);
            inventoryManager.InitializeInventory();
        }

        public void CloseAllMenus()
        {
            inventoryScreen.SetActive(false);
            MenuOpen = false;
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

        private void SetupEliminationUI()
        {

        }

        private void SetupSurvivalUI()
        {

        }

        private void SetupDefenseUI()
        {

        }

        #region Tutorial

        public void EnableStayInsideMessage()
        {
            if (!StayInsideText.activeSelf) { StayInsideText.SetActive(true); }
        }

        public void ChangeTutorialMessage(string message)
        {
            TutorialText.text = message;
        }

        public void QuestionPromptSwitch(bool enable)
        {
            QuestionPrompt.SetActive(enable);
        }

        public void LmbPromptSwitch(bool enable)
        {
            LmbPrompt.SetActive(enable);
        }

        public void EnableLevelTutorial()
        {
            FirstLevelUpPanel.SetActive(true);
        }

        public void EnableHubIndicator()
        {
            IndicationArrow.SetActive(true);
        }

        #endregion

        public void UpdateHealth(float currentHealth, float maxHealth)
        {
            LivesText.text = Mathf.Round(currentHealth) + "/" + Mathf.Round(maxHealth);
            PlayerHealthBar.UpdateHealth(currentHealth, maxHealth);
        }

        public void UpdateEnergy(float current, float max)
        {
            EnergyBar.UpdateEnergy(current, max);
        }

        public void UpdateUnits()
        {
            UnitsText.text = "Units:" + playerStats.GetStatValue(EnumCollections.PlayerStats.CurrentUnits);
            StopCoroutine(BounceSizeTween(UnitsText.gameObject));
            StartCoroutine(BounceSizeTween(UnitsText.gameObject));
        }

        public void UpdateExperience()
        {
            ExperienceBar.value = playerStats.GetExperienceLevelProgress();
        }

        public void UpdateLevel()
        {
            playerLevelText.text = "" + playerStats.GetPlayerLevel();
            ExperienceBar.maxValue = playerStats.GetExperienceDifference();
            UpdateExperience();
        }

        public void OnLevelUp()
        {
            LevelUpText.SetActive(true);
            CheckPlusSign();
        }

        private void CheckPlusSign()
        {
            if (playerStats.GetCurrentSkillPoints() > 0) { Plus.SetActive(true); }
            else { Plus.SetActive(false); }
        }

        public void UpdateSkillPoints()
        {
            SkillPoints.text = "" + playerStats.GetCurrentSkillPoints();
            CheckPlusSign();
        }

        public void SetSkillTooltip(Skill_Stat skill, Vector2 position)
        {
            skillTooltip.SetTooltip(skill, position);
            skillTooltip.gameObject.SetActive(true);
        }

        public void HideSkillTooltip()
        {
            skillTooltip.gameObject.SetActive(false);
        }

        public IEnumerator ShowDeathScreen()
        {
            yield return new WaitForSeconds(3f);
            //deathScreen.LeanAlpha(1);
            deathScreen.SetActive(true);
        }

        public void EnableBossHealthBar()
        {
            bossBar.gameObject.SetActive(true);
        }

        public void DisableBossHealthBar()
        {
            if (bossBar.gameObject.activeSelf) { bossBar.gameObject.SetActive(false); }
        }

        public void UpdateBossHealthBar(float currentHealth, float maxHealth)
        {
            bossBar.maxValue = maxHealth;
            bossBar.value = currentHealth;
        }

        public void EnableBossWarning()
        {
            bossWarning.SetActive(true);
        }

        public void OpenInventory()
        {
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
            if(inventoryScreen.activeSelf) 
            {
                MenuOpen = true;
                UpdateSkillPoints();
                inventoryManager.OpenInventory(); 
            }
        }

        public void ShowItemTooltip(ItemData item)
        {
            inventoryTooltip.gameObject.SetActive(true);
            inventoryTooltip.ShowTooltip(item);
        }
        public void ShowItemTooltip(EquipmentData equipment)
        {
            inventoryTooltip.gameObject.SetActive(true);
            inventoryTooltip.ShowTooltip(equipment);
        }
        public void ShowItemTooltip(WeaponData equipment)
        {
            inventoryTooltip.gameObject.SetActive(true);
            inventoryTooltip.ShowTooltip(equipment);
        }

        public void HideItemTooltip()
        {
            inventoryTooltip.gameObject.SetActive(false);
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
            ObjectiveMenu.SetActive(true);
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
            //OnClosePauseMenu.Invoke();
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