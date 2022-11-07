using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class EnemyGroup : MonoBehaviour
    {
        private SpawnManager spawnManager;
        [SerializeField] protected List<Transform> enemies;
        protected List<Vector3> enemyStartPositions;
        [SerializeField] protected Vector2Int enemyAmountRange = new Vector2Int(3, 5);
        protected int enemyAmount;

        protected float enemiesDisabled;

        public string enemyType;

        private void Awake()
        {
            enemies = new List<Transform>();
            enemyStartPositions = new List<Vector3>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                enemyStartPositions.Add(child.position);
                enemies.Add(child);
                child.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            spawnManager = SpawnManager.Instance;
        }

        protected virtual void OnEnable()
        {
            ResetEnemies();
            EnableEnemies();
        }

        public IEnumerator EnemyDisabled()
        {
            enemiesDisabled++;
            if (enemiesDisabled == enemyAmount)
            {
                //We wait here for the particle effects to be returned
                yield return new WaitForSeconds(15f);
                ResetEnemies();
                gameObject.SetActive(false);
            }
        }

        protected virtual void ResetEnemies()
        {
            if(spawnManager != null)
            {
                spawnManager.RemoveEnemyType(enemyType);
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].localPosition = enemyStartPositions[i];
                enemies[i].gameObject.SetActive(false);
            }
            enemiesDisabled = 0;
        }

        protected void EnableEnemies()
        {
            enemyAmount = Random.Range(enemyAmountRange.x, enemyAmountRange.y);
            List<int> possibleNumbers = new List<int>();
            List<int> randomNumbers = new List<int>();
            //Select random enemies from children
            for(int i = 0; i < enemies.Count; i++)
            {
                possibleNumbers.Add(i);
            }
            for(int i = 0; i < enemyAmount; i ++)
            {
                int index = Random.Range(0, possibleNumbers.Count);
                randomNumbers.Add(possibleNumbers[index]);
                possibleNumbers.RemoveAt(index);
            }
            //Debug.Log(randomNumbers.Count);

            for (int i = 0; i < enemyAmount; i++)
            {
                Transform child = enemies[randomNumbers[i]];
                child.gameObject.SetActive(true);
            }
        }
    }
}
