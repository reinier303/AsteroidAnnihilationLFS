using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ObjectPoolerPerformanceTest : MonoBehaviour
    {
        private ObjectPooler objectPooler;
        public GameObject prefab;

        // Start is called before the first frame update
        void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                UnityEngine.Profiling.Profiler.BeginSample("OPTest");
                for (int i = 0; i < 1000; i++)
                {
                    objectPooler.SpawnFromPool(EnumCollections.PlayerProjectiles.PlasmaBullet.ToString(), transform.position, Quaternion.identity);
                }
                UnityEngine.Profiling.Profiler.EndSample();
            }
        }
    }
}
