using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class SpawnManager : SerializedMonoBehaviour
    {
        public static SpawnManager Instance;

        //Script References
        private GameManager gameManager;
        private ObjectPooler RObjectPooler;
        private MissionManager missionManager;
        private PlayerEntity playerEntity;

        //Current Wave Data
        public Mission currentMission;

        [SerializeField] private float currentSpawnTime;

        //General Data
        public bool Spawning;

        //Background Renderer: info for spawn location
        public SpriteRenderer MapRenderer;
        private List<string> enemyNames = new List<string>();
        private List<string> seekerEnemyNames = new List<string>();

        private Transform player;

        //Spawn variables
        [SerializeField] private float spawnDistance = 100;
        private List<GameObject> enemiesAlive;
        [SerializeField] private Dictionary<string, int> enemyTypeCount;
        [SerializeField] private Dictionary<string, int> enemyTypeMax;

        private void Awake()
        {
            Instance = this;
            enemyTypeCount = new Dictionary<string, int>();
            enemyTypeMax = new Dictionary<string, int>();
        }

        //Time measurement = 0-2ms
        public void Initialize()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            RObjectPooler = ObjectPooler.Instance;
            gameManager = GameManager.Instance;
            missionManager = MissionManager.Instance;
            playerEntity = Player.Instance.RPlayerEntity;
            enemiesAlive = new List<GameObject>();

            SetMission();

            currentSpawnTime = currentMission.StartSpawnRate;
            player = gameManager.RPlayer.transform;
            Spawning = true;

            StartCoroutine(StartSpawning());
            StartCoroutine(RampSpawnRate());
            StartCoroutine(CheckRecentlyHit());
            stopWatch.Stop();
            Debug.Log("Elapsed time is: " + stopWatch.ElapsedMilliseconds);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            DisableAllEnemies();
        }

        public void SetMission()
        {
            currentMission = missionManager.GetCurrentMission(); 

            GenerateRandomEnemyList();
            GenerateRandomSeekerEnemyList();
        }

        private IEnumerator StartSpawning()
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnTime);
            StartCoroutine(StartSpawning());
        }

        private IEnumerator CheckRecentlyHit()
        {
            yield return new WaitForSeconds(Random.Range(20, 45));
            if (!playerEntity.RecentlyHit)
            {
                SpawnSeekerEnemy();
            }
            playerEntity.RecentlyHit = false;
            StartCoroutine(CheckRecentlyHit());
        }

        private void SpawnEnemy()
        {
            if (Spawning)
            {
                Vector2 spawnPosition = GenerateSpawnPosition();
                string enemy = enemyNames[Random.Range(0, enemyNames.Count)];
                if(CheckEnemyType(enemy))
                {
                    GameObject enemyObject = RObjectPooler.SpawnFromPool(enemy, spawnPosition, Quaternion.identity);
                    BaseEnemy enemyScipt = enemyObject.GetComponent<BaseEnemy>();
                    if (enemyScipt == null)
                    {
                        EnemyGroup groupScipt = enemyObject.GetComponent<EnemyGroup>();
                        if(groupScipt != null) { groupScipt.enemyType = enemy; }          
                    }
                    else 
                    {
                        enemyScipt.enemyType = enemy;
                    }
                    enemiesAlive.Add(enemyObject);

                }
            }
        }

        public void SpawnSeekerEnemy()
        {
            if (Spawning)
            {
                Vector2 spawnPosition = GenerateSpawnPosition();
                string enemy = seekerEnemyNames[Random.Range(0, seekerEnemyNames.Count)];
                if (CheckEnemyType(enemy))
                {
                    enemiesAlive.Add(RObjectPooler.SpawnFromPool(enemy, spawnPosition, Quaternion.identity));
                }
            }
        }

        public void SpawnBoss(Mission mission)
        {
            //O(1)
            Spawning = false;
            Vector2 spawnPosition = GenerateSpawnPosition();
            enemiesAlive.Add(RObjectPooler.SpawnFromPool(mission.Boss, spawnPosition, Quaternion.identity));
        }

        private bool CheckEnemyType(string enemy)
        {
            //O(1)
            Debug.Log(enemy);

            if (!enemyTypeCount.ContainsKey(enemy)) 
            {
                enemyTypeCount.Add(enemy, 1);
                return true;
            }
            else {
                if (enemyTypeCount[enemy] >= enemyTypeMax[enemy])
                {
                    return false;
                }
                enemyTypeCount[enemy]++;
                return true;
            }
        }

        public void RemoveEnemyType(string enemyType)
        {
            if (enemyTypeCount.ContainsKey(enemyType))
            {
                Debug.Log("s");
                enemyTypeCount[enemyType] = enemyTypeCount[enemyType] - 1;
            }
        }

        private IEnumerator RampSpawnRate()
        {
            yield return new WaitForSeconds(1);
            currentSpawnTime += currentMission.SpawnRateRampPerSecond;
            if (currentSpawnTime >= currentMission.MaxSpawnRate)
            {
                StartCoroutine(RampSpawnRate());
            }
        }

        private Vector2 GenerateSpawnPosition()
        {
            Vector2 playerPos = player.position;
            Vector2 spawnPosition = new Vector2(Random.Range(playerPos.x - spawnDistance, playerPos.x + spawnDistance), Random.Range(playerPos.y - spawnDistance, playerPos.y + spawnDistance));
            Vector2 viewPos = Camera.main.WorldToViewportPoint(spawnPosition);

            while (viewPos.x > -0.2f && viewPos.x < 1.2f && viewPos.y > -0.2f && viewPos.y < 1.2f)
            {
                spawnPosition = new Vector2(Random.Range(playerPos.x - spawnDistance, playerPos.x + spawnDistance), Random.Range(playerPos.y - spawnDistance, playerPos.y + spawnDistance));
                viewPos = Camera.main.WorldToViewportPoint(spawnPosition);
            }

            return spawnPosition;
        }

        private void GenerateRandomEnemyList()
        {
            enemyNames.Clear();
            enemyTypeMax.Clear();
            //O(i*j)
            for (int i = 0; i < currentMission.Enemies.Count; i++)
            {
                string enemy = currentMission.Enemies[i].EnemyType.ToString();
                if (!enemyTypeMax.ContainsKey(enemy)) { enemyTypeMax.Add(enemy, currentMission.Enemies[i].MaxAmount); }
                else { enemyTypeMax[enemy] += currentMission.Enemies[i].MaxAmount; }
                for (int j = 0; j < currentMission.Enemies[i].Priority; j++)
                {
                    enemyNames.Add(enemy);
                }
            }

        }

        private void GenerateRandomSeekerEnemyList()
        {
            //O(i*j)
            for (int i = 0; i < currentMission.SeekerEnemies.Count; i++)
            {
                string enemy = currentMission.SeekerEnemies[i].EnemyType.ToString();
                if (!enemyTypeMax.ContainsKey(enemy)) { enemyTypeMax.Add(enemy, currentMission.SeekerEnemies[i].MaxAmount); }
                else { enemyTypeMax[enemy] += currentMission.SeekerEnemies[i].MaxAmount; }
                for (int j = 0; j < currentMission.SeekerEnemies[i].Priority; j++)
                {
                    seekerEnemyNames.Add(enemy);
                }
            }
        }

        public void DisableAllEnemies()
        {
            //O(N)
            foreach(GameObject gameObject in enemiesAlive)
            {
                if (gameObject != null) { gameObject.SetActive(false); }
            }
        }
    }
}