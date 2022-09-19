using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class AsteroidGroup : MonoBehaviour
    {
        [SerializeField] protected List<Transform> asteroids;
        protected List<Vector3> asteroidStartPositions;

        protected float AsteroidsDisabled;

        private void Awake()
        {
            asteroids = new List<Transform>();
            asteroidStartPositions = new List<Vector3>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform asteroid = transform.GetChild(i);
                asteroidStartPositions.Add(asteroid.localPosition);
                asteroids.Add(asteroid);
            }
        }

        protected virtual void OnEnable()
        {
            ResetAsteroids();
        }

        public IEnumerator AsteroidDisabled()
        {
            AsteroidsDisabled++;
            if(AsteroidsDisabled == asteroids.Count)
            {
                yield return new WaitForSeconds(15f);
                ResetAsteroids();
                gameObject.SetActive(false);
            }
        }

        protected virtual void ResetAsteroids()
        {
            for(int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].localPosition = asteroidStartPositions[i];
                asteroids[i].gameObject.SetActive(true);
            }
            AsteroidsDisabled = 0;          
        }
    }

}