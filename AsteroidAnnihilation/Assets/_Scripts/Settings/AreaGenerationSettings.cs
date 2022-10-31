using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Area Generation", order = 999)]
    public class AreaGenerationSettings : SerializedScriptableObject
    {
        public TierSettings StartArea;

        [SerializeField] public Dictionary<EnumCollections.EnemyFactions, TierSettings> Factions;
    }

    [System.Serializable]
    public class TierSettings
    {
        public string[] BaseAreaName;
        public bool RandomNameAddition;
        public Color AreaTextColor;
        public Material AreaTextMaterial;
        public List<StatCompletionReward> CompletionRewardPool;

        public string GetAreaName()
        {
            int randomAlphabet = Random.Range(0, 26);
            char randomLetter = (char)('a' + randomAlphabet);
            string randomString = randomLetter.ToString();

            return BaseAreaName[Random.Range(0, BaseAreaName.Length)] + (RandomNameAddition == true ? (Random.Range(1, 1000) + randomString.ToUpper()) : "");
        }

        public int EnemyTypeAmount;
        /// <summary>
        /// These enemies will not be randomised and thus will always be present in this tier
        /// </summary>
        public List<EnemyAreaData> AlwaysPresentEnemies;
        public List<EnemyAreaData> RandomisedEnemies;
        public List<EnemyAreaData> SeekerEnemies;
        public List<EnumCollections.Bosses> Bosses;

        public List<EnemyAreaData> GetEnemies()
        {
            List<EnemyAreaData> enemies = new List<EnemyAreaData>();
            for(int i = 0; i < AlwaysPresentEnemies.Count; i++)
            {
                enemies.Add(AlwaysPresentEnemies[i]);
            }
            for (int i = 0; i < EnemyTypeAmount; i++)
            {
                enemies.Add(RandomisedEnemies[Random.Range(0, RandomisedEnemies.Count)]);
            }
            return enemies;
        }

        public string GetBoss()
        {
            if(Bosses == null) { return ""; }
            return Bosses[Random.Range(0, Bosses.Count)].ToString();
        }

        public int NumberOfObjectives;
        public List<AreaObjective> Objectives;
        public List<AreaObjective> GetObjectives()
        {
            List<AreaObjective> objectives = new List<AreaObjective>();
            for (int i = 0; i < NumberOfObjectives; i++)
            {
                AreaObjective objective = new AreaObjective();
                AreaObjective objectivePreset = Objectives[Random.Range(0, Objectives.Count)];
                objective.ObjectiveType = objectivePreset.ObjectiveType;
                objective.ObjectiveAmount = Random.Range(objectivePreset.ObjectiveAmountRange.x, objectivePreset.ObjectiveAmountRange.y);
                objective.ObjectiveProgress = 0;
                objective.ObjectiveDone = false;
                for (int j = 0; j < objectives.Count; j++)
                {
                    if(objectives[j].ObjectiveType == objective.ObjectiveType)
                    {
                        objective.ObjectiveAmount += objectives[j].ObjectiveAmount;
                        objectives.Remove(objectives[j]);
                    }
                }             
                objectives.Add(objective);
            }

            return objectives;
        }

        public Vector2 UnitsRewardMinMax;
        public Vector2 ExperienceRewardMinMax;

        public Vector2 GetRewards()
        {
            Vector2 rewards = new Vector2();
            rewards.x = Random.Range(UnitsRewardMinMax.x , UnitsRewardMinMax.y);
            rewards.y = Random.Range(ExperienceRewardMinMax.x, ExperienceRewardMinMax.y);
            rewards.x = ((int)(rewards.x / 5)) * 5;
            rewards.y = ((int)(rewards.y / 5)) * 5;
            return rewards;
        }

        public List<AreaBackgrounds> Backgrounds;
        public List<EnumCollections.Backgrounds> GetBackgrounds()
        {
            return Backgrounds[Random.Range(0, Backgrounds.Count)].Backgrounds;
        }

        [FoldoutGroup("SpawnRate")] public float StartSpawnRateMin, StartSpawnRateMax = 1;
        [FoldoutGroup("SpawnRate")] public float SpawnRateRampPerSecond = -0.01f;
        [FoldoutGroup("SpawnRate")] public float MaxSpawnRateMin, MaxSpawnRateMax = 0.5f;

        public float GetStartSpawnRate() { return Random.Range(StartSpawnRateMin, StartSpawnRateMax); }
        public float GetMaxSpawnRate() { return Random.Range(MaxSpawnRateMin, MaxSpawnRateMax); }

        /// <summary>
        /// Fill reward pool for each tier before assigning rewards to areas
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="amount"></param>
        public void PopulateCompletionPool(StatCompletionRewardSettings settings)
        {
            CompletionRewardPool = settings.GetCompletionRewardPool(3);
        }

        public List<StatCompletionReward> GetStatCompletionRewards(int amount)
        {
            List<StatCompletionReward> rewards = new List<StatCompletionReward>();
            for(int i = 0; i < amount; i++)
            {
                rewards.Add(CompletionRewardPool[Random.Range(0, CompletionRewardPool.Count)]);
            }
            return rewards;
        }
    }

    [System.Serializable]
    public class AreaData
    {
        public int Tier;

        public string AreaName;
        public Color AreaTextColor;
        public Material AreaTextMaterial;

        public List<EnemyAreaData> Enemies;

        public List<AreaObjective> Objectives;
        public bool Unlocked;
        public bool AreaCompleted;

        public float StartSpawnRate;
        public float SpawnRateRampPerSecond;
        public float MaxSpawnRate;

        public List<StatCompletionReward> CompletionRewards;

        public List<EnumCollections.Backgrounds> Backgrounds;
    }

    [System.Serializable]
    public struct EnemyAreaData
    {
        [Header("This name has to be the same as in the pool")]
        public EnumCollections.EnemyTypes EnemyType;
        public int MaxAmount;
        public int Priority;
    }

    [System.Serializable] public enum MissionObjectiveType { Elimination, Defense, Survival, /*PointCapture ,GoldenSnitch , ScanEnemies, BossRush, ObstacleCouresRace, Invaders */}

    [System.Serializable]
    public class AreaObjective
    {
        public MissionObjectiveType ObjectiveType;
        public Vector2Int ObjectiveAmountRange;
        public int ObjectiveAmount;
        public int ObjectiveProgress;
        public bool ObjectiveDone;
    }

    [System.Serializable]
    public struct AreaBackgrounds
    {
        public List<EnumCollections.Backgrounds> Backgrounds;
    }
}
