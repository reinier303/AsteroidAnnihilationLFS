using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class AreaManager : MonoBehaviour
    {
        public static AreaManager Instance;

        private GameManager gameManager;

        private UIManager uIManager;
        private AreaGenerationSettings generationSettings;
        
        [Header("Amount of areas per tier")]
        public int[] Tiers;

        private TierData tierData;

        public ParallaxBackground Background;

        public int CurrentTier;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            //LoadAreas();

            //CurrentTier = tierData.CurrentTier;
            CurrentTier = 0;

        }

        private void Start()
        {
            GetReferencesAndSubscribe();
        }

        private void GetReferencesAndSubscribe()
        {
            gameManager = GameManager.Instance;
            gameManager.onEndGame += SaveAreas;
            uIManager = UIManager.Instance;
        }

        private void LoadAreas()
        {
            if (!ES3.KeyExists("tierData"))
            {
                tierData = new TierData();
                tierData.CurrentTier = 0;
                //GenerateAreaData();
                SetCurrentArea(0, 0);
                ES3.Save("tierData", tierData);
            }
            else 
            { 
                tierData = ES3.Load<TierData>("tierData"); 
            }
        }

        #region Area Generation
        /*
        private void GenerateAreaData()
        {
            generationSettings = (AreaGenerationSettings)Resources.Load("Settings/AreaGenerationSettings");
            CompletionRewardSettings completionSettings = (CompletionRewardSettings)Resources.Load("Settings/CompletionRewardSettings");

            foreach(TierSettings tier in generationSettings.Tiers)
            {
                tier.PopulateCompletionPool(completionSettings);
            }

            tierData.Tiers = new Dictionary<int, List<AreaData>>();

            for (int i = 0; i < Tiers.Length; i++)
            {
                tierData.Tiers[i] = new List<AreaData>();
            }

            generationSettings.StartArea.PopulateCompletionPool(completionSettings);
            GenerateStartArea(completionSettings);

            for (int i = 0; i < Tiers.Length; i++)
            {
                for (int j = 0; j < Tiers[i]; j++)
                {
                    AreaData area = GenerateArea(i);
                    tierData.Tiers[i].Add(area);
                }
            }
        }

        private void GenerateStartArea(CompletionRewardSettings completionSettings)
        {
            AreaData start = new AreaData();
            start.Tier = 1;
            start.AreaName = generationSettings.StartArea.GetAreaName();
            start.AreaTextColor = generationSettings.StartArea.AreaTextColor;
            start.AreaTextMaterial = generationSettings.StartArea.AreaTextMaterial;
            start.Enemies = generationSettings.StartArea.GetEnemies();
            start.Objectives = generationSettings.StartArea.GetObjectives();
            start.Unlocked = true;
            start.AreaCompleted = false;

            start.StartSpawnRate = generationSettings.StartArea.GetStartSpawnRate();
            start.SpawnRateRampPerSecond = generationSettings.StartArea.SpawnRateRampPerSecond;
            start.MaxSpawnRate = generationSettings.StartArea.GetMaxSpawnRate();

            start.CompletionRewards = generationSettings.StartArea.GetCompletionRewards(1);

            start.Backgrounds = generationSettings.StartArea.GetBackgrounds();

            tierData.startAreaData = start;
            tierData.Tiers[0].Add(start);
        }
        
        private AreaData GenerateArea(int i)
        {
            AreaData area = new AreaData();
            area.Tier = i + 1;
            area.AreaName = generationSettings.Tiers[i].GetAreaName();
            area.AreaTextColor = generationSettings.Tiers[i].AreaTextColor;
            area.AreaTextMaterial = generationSettings.Tiers[i].AreaTextMaterial;

            area.Enemies = generationSettings.Tiers[i].GetEnemies();

            area.Objectives = generationSettings.Tiers[i].GetObjectives();
            //Debug.Log(area.Objectives[0].ObjectiveAmount + ", " + area.AreaName);
            //DE-BUG::everything is still generated fine here
            area.Unlocked = false;
            area.AreaCompleted = false;

            area.StartSpawnRate = generationSettings.Tiers[i].GetStartSpawnRate();
            area.SpawnRateRampPerSecond = generationSettings.Tiers[i].SpawnRateRampPerSecond;
            area.MaxSpawnRate = generationSettings.Tiers[i].GetMaxSpawnRate();

            area.CompletionRewards = generationSettings.Tiers[i].GetCompletionRewards(Mathf.Clamp(area.Tier, 1, 3));
            area.Backgrounds = generationSettings.Tiers[i].GetBackgrounds();

            return area;
        }
        */
        #endregion


        public AreaData GetStartAreaData()
        {
            return tierData.startAreaData;
        }

        public AreaData GetAreaData(int tier, int index)
        {
            return tierData.Tiers[tier][index];
        }

        public List<AreaData> GetTierDatas(int tier)
        {
            return tierData.Tiers[tier];
        }

        public AreaData GetCurrentAreaData()
        {
            return tierData.Tiers[tierData.CurrentArea.x][tierData.CurrentArea.y];
        }

        public void SetCurrentArea(int tier, int index, bool LoadScene = false)
        {
            tierData.CurrentArea = new Vector2Int(tier, index);
            SaveAreas();
            if (LoadScene)
            {
                //Dont use saved reference because gamemanager isnt dontdestroy on load
                GameManager.Instance.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                //StartCoroutine(RenewReferences());
            }
        }
        /* attempt at fixing missing references on scene reload
        private IEnumerator RenewReferences()
        {
            while(ExtensionMethods.sceneLoadOperation != null && !ExtensionMethods.sceneLoadOperation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(2);

            GetReferencesAndSubscribe();
        }
        */

        public void SaveAreas()
        {
            ES3.Save("tierData", tierData);
        }
    }

    [System.Serializable]
    public class TierData
    {
        public Vector2Int CurrentArea;
        public int CurrentTier;

        public Dictionary<int, List<AreaData>> Tiers;
        public AreaData startAreaData;

        public List<StatCompletionReward> CompletionRewards;
        /*
        public List<AreaData> areaDataT1;
        public List<AreaData> areaDataT2;
        public List<AreaData> areaDataT3;
        public List<AreaData> areaDataT4;
        */
    }
}
