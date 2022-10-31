using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace AsteroidAnnihilation
{
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance;

        private GameManager gameManager;
        private UIManager uiManager;
        private InputManager inputManager;
        private ParallaxBackground parallaxBackground;

        [SerializeField] private SpawnManager spawnManager;
        private AreaGenerationSettings generationSettingsT1;
        private AreaGenerationSettings generationSettingsT2;

        private List<Mission> currentMissions;
        private int currentMissionIndex;

        //TODO::Save this in playerdata as a stat
        public int maxMissions = 3;

        [SerializeField] private Transform missionCardHolder;
        [SerializeField] private GameObject missionCard;

        [SerializeField] private GameObject gameElements, hubElements;

        private bool newMissionGenerated, canGainProgress;

        private void Awake()
        {
            if(Instance != null) { Destroy(gameObject);} else { Instance = this; }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SettingsManager settingsManager = SettingsManager.Instance;
            generationSettingsT1 = settingsManager.generationSettingsT1;
            generationSettingsT2 = settingsManager.generationSettingsT2;

            newMissionGenerated = false;
            LoadMissions();
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
            inputManager = InputManager.Instance;
            parallaxBackground = ParallaxBackground.Instance;
            gameManager.onEndGame += SaveMissions;

        }

        public void ShowMissionCards()
        {
            //We're taking into account the RankProgressBar and checking if the mission cards have already been generated
            if (missionCardHolder.childCount - 1 != maxMissions || newMissionGenerated)
            {
                for(int i = 0; i < missionCardHolder.childCount; i++)
                {
                    Transform child = missionCardHolder.GetChild(i);
                    if (child.GetComponent<LayoutElement>() == null)
                    {
                        Destroy(child.gameObject);
                    }
                }
                newMissionGenerated = false;
                foreach (Mission mission in currentMissions)
                {
                    MissionCard card = Instantiate(missionCard, missionCardHolder).GetComponent<MissionCard>();
                    card.InitializeCard(mission);
                }
            }
        }

        public IEnumerator MoveToMissionArea(int missionIndex)
        {
            currentMissionIndex = missionIndex;
            uiManager.LoadingScreen.SetActive(true);
            inputManager.InputEnabled = false;
            yield return new WaitForSecondsRealtime(1f);
            gameElements.SetActive(true);
            hubElements.SetActive(false);
            yield return new WaitForSecondsRealtime(3f);
            spawnManager.gameObject.SetActive(true);
            parallaxBackground.SetMissionBackgrounds();
            spawnManager.Initialize();
            uiManager.UpdateMissionUI();
            canGainProgress = true;
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(uiManager.LoadingScreen.GetComponent<LoadingScreen>().FadeOutLoadingScreen());
            inputManager.InputEnabled = true;
        }

        public IEnumerator MoveToHub()
        {
            uiManager.LoadingScreen.SetActive(true);
            inputManager.InputEnabled = false;
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(1.1f);
            Player.Instance.transform.position = Vector3.zero;
            spawnManager.gameObject.SetActive(false);
            gameElements.SetActive(false);
            hubElements.SetActive(true);
            parallaxBackground.SetHubBackgrounds();
            Time.timeScale = 1;
            yield return new WaitForSecondsRealtime(2.25f);
            StartCoroutine(uiManager.LoadingScreen.GetComponent<LoadingScreen>().FadeOutLoadingScreen());
            inputManager.InputEnabled = true;

        }

        public Mission GetCurrentMission()
        {
            return currentMissions[currentMissionIndex];
        }

        private void LoadMissions()
        {
            if (!ES3.KeyExists("currentMissions"))
            {
                currentMissions = new List<Mission>();
                GenerateMissions(maxMissions);
                ES3.Save("currentMissions", currentMissions);
            }
            else
            {
                currentMissions = ES3.Load<List<Mission>>("currentMissions");
            }
        }

        private void SaveMissions()
        {
            ES3.Save("currentMissions", currentMissions);
        }

        private void GenerateMissions(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                currentMissions.Add(GenerateMission());
            }
        }

        private Mission GenerateMission(int tier = 0, List<EnumCollections.EnemyFactions> enemyFactions = null)
        {
            Mission mission = new Mission();

            if (tier == 0) { mission.Tier = 1; }
            else { mission.Tier = tier; }
            AreaGenerationSettings settings = GetGenerationSettings(tier);

            int factionCount = EnumCollections.EnemyFactions.GetNames(typeof(EnumCollections.EnemyFactions)).Length;
            if (enemyFactions == null) { mission.Faction = settings.Factions.ElementAt(Random.Range(0, settings.Factions.Count)).Key;}
            else { mission.Faction = enemyFactions[Random.Range(0, enemyFactions.Count)];}

            mission.AreaName = settings.Factions[mission.Faction].GetAreaName();
            mission.AreaTextColor = settings.Factions[mission.Faction].AreaTextColor;
            mission.AreaTextMaterial = settings.Factions[mission.Faction].AreaTextMaterial;

            mission.Enemies = settings.Factions[mission.Faction].GetEnemies();
            mission.SeekerEnemies = new List<EnemyAreaData>();
            foreach(EnemyAreaData data in settings.Factions[mission.Faction].SeekerEnemies)
            {
                mission.SeekerEnemies.Add(data);
            }
            mission.Boss = settings.Factions[mission.Faction].GetBoss();

            mission.Objectives = settings.Factions[mission.Faction].GetObjectives();
            Vector2 rewards = settings.Factions[mission.Faction].GetRewards();
            mission.UnitsReward = rewards.x;
            mission.ExperienceReward = rewards.y;

            mission.StartSpawnRate = settings.Factions[mission.Faction].GetStartSpawnRate();
            mission.SpawnRateRampPerSecond = settings.Factions[mission.Faction].SpawnRateRampPerSecond;
            mission.MaxSpawnRate = settings.Factions[mission.Faction].GetMaxSpawnRate();

            mission.Backgrounds = settings.Factions[mission.Faction].GetBackgrounds();

            return mission;
        }

        private AreaGenerationSettings GetGenerationSettings(int tier)
        {
            switch (tier)
            {
                case 1: return generationSettingsT1;
                case 2: return generationSettingsT2;
                default: return generationSettingsT1;
            }
        }

        public List<Sprite> GetCurrentBackgrounds()
        {
            List<Sprite> backgrounds = new List<Sprite>();
            foreach (EnumCollections.Backgrounds bg in GetCurrentMission().Backgrounds)
            {
                backgrounds.Add(Resources.Load<Sprite>("Backgrounds/" + bg.ToString()));
            }
            return backgrounds;
        }

        #region Objectives

        public void AddObjectiveProgress(MissionObjectiveType type)
        {
            if (!canGainProgress) { return; }
            Mission mission = GetCurrentMission();
            for (int i = 0; i < mission.Objectives.Count; i++)
            {
                if (mission.Objectives[i].ObjectiveType == type && !mission.Objectives[i].ObjectiveDone)
                {
                    AddToObjective(mission, i);
                    UIManager.Instance.UpdateObjectives();
                    if (CheckObjectiveDone(mission, i)) { return; }
                    return;
                }
            }
        }

        private bool CheckObjectiveDone(Mission mission, int objectiveIndex)
        {
            if (mission.Objectives[objectiveIndex].ObjectiveProgress >= mission.Objectives[objectiveIndex].ObjectiveAmount)
            {
                mission.Objectives[objectiveIndex].ObjectiveDone = true;

                //Check if all objectives area done for area completion
                int objectivesDone = 0;
                for (int i = 0; i < mission.Objectives.Count; i++)
                {
                    if (mission.Objectives[i].ObjectiveDone) { objectivesDone++; }
                }
                if (objectivesDone == mission.Objectives.Count)
                {
                    spawnManager.SpawnBoss(mission);
                    //MissionCompleted(mission);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MissionCompleted()
        {
            Mission mission = GetCurrentMission();
            //mission.AreaCompleted = true;
            //CompletionRewardStats.Instance.AddRewardedStat(area.CompletionRewards);
            gameManager.RPlayer.RPlayerStats.AddToUnits(mission.UnitsReward);
            gameManager.RPlayer.RPlayerStats.AddToExperience(mission.ExperienceReward);
            uiManager.OnMissionComplete();
            currentMissions.RemoveAt(currentMissionIndex);
            currentMissions.Insert(currentMissionIndex, GenerateMission());
            newMissionGenerated = true;
            canGainProgress = false;
        }

        public void AddToObjective(Mission mission, int objectiveIndex)
        {
            mission.Objectives[objectiveIndex].ObjectiveProgress++;
        }

        #endregion
    }

    public class Mission
    {
        public int Tier;

        public EnumCollections.EnemyFactions Faction;
        public string AreaName;
        public Color AreaTextColor;
        public Material AreaTextMaterial;

        public List<EnemyAreaData> Enemies;
        public List<EnemyAreaData> SeekerEnemies;
        public string Boss;

        public List<AreaObjective> Objectives;

        public float UnitsReward;
        public float ExperienceReward;

        public float StartSpawnRate;
        public float SpawnRateRampPerSecond;
        public float MaxSpawnRate;

        //public List<StatCompletionReward> StatCompletionRewards;

        public List<EnumCollections.Backgrounds> Backgrounds;
    }
}
