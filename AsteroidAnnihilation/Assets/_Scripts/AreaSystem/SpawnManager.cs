using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace AsteroidAnnihilation
{
    public class SpawnManager : MonoBehaviour
    {
        //Script References
        private GameManager gameManager;
        private ObjectPooler RObjectPooler;
        private MissionManager missionManager;

        //Current Wave Data
        public Mission currentMission;

        [SerializeField] private float currentSpawnTime;

        //General Data
        public bool Spawning;

        //Background Renderer: info for spawn location
        public SpriteRenderer MapRenderer;
        private List<string> enemyNames = new List<string>();

        private Transform player;

        //Spawn variables
        [SerializeField] private float spawnDistance = 100;
        private List<GameObject> enemiesAlive;

        public void Initialize()
        {
            RObjectPooler = ObjectPooler.Instance;
            gameManager = GameManager.Instance;
            missionManager = MissionManager.Instance;
            enemiesAlive = new List<GameObject>();

            SetMission();

            currentSpawnTime = currentMission.StartSpawnRate;
            player = gameManager.RPlayer.transform;

            StartCoroutine(StartSpawning());
            StartCoroutine(RampSpawnRate());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            DisableAllEnemies();
        }

        public void SetMission()
        {
            //TODO::Change this to be the current area from save instead of always startarea
            currentMission = missionManager.GetCurrentMission(); 

            GenerateRandomEnemyList();
        }

        private IEnumerator StartSpawning()
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnTime);
            StartCoroutine(StartSpawning());
        }

        private void SpawnEnemy()
        {
            if (Spawning)
            {
                Vector2 spawnPosition = GenerateSpawnPosition();
                string enemy = enemyNames[Random.Range(0, enemyNames.Count)];
                enemiesAlive.Add(RObjectPooler.SpawnFromPool(enemy, spawnPosition, Quaternion.identity));
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
            for (int i = 0; i < currentMission.Enemies.Count; i++)
            {
                for (int j = 0; j < currentMission.Enemies[i].Priority; j++)
                {
                    enemyNames.Add(currentMission.Enemies[i].EnemyType.ToString());
                }
            }
        }

        public void DisableAllEnemies()
        {
            foreach(GameObject gameObject in enemiesAlive)
            {
                gameObject.SetActive(false);
            }
        }
    }
}