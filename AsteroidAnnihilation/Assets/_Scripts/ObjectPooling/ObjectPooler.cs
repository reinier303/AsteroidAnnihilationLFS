using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ObjectPooler : MonoBehaviour
    {
        //CREATE POOL IN RESOURCES FOLDER TO FUNCTION.
        //Assets/Resources/Pools/CreatePoolHere
        public static ObjectPooler Instance;

        private Dictionary<string, ScriptablePool> Pools = new Dictionary<string, ScriptablePool>();
        public Dictionary<string, Queue<GameObject>> PoolDictionary;
        private Dictionary<string, Transform> PoolParents = new Dictionary<string, Transform>();
        public Transform PopUpParent;

        private void Awake()
        {
            Instance = this;

            Object[] ScriptablePools = Resources.LoadAll("Pools", typeof(ScriptablePool));
            foreach (ScriptablePool pool in ScriptablePools)
            {
                //Check if pool info is filled.
                if (pool.Amount > 0 && pool.Prefab != null && pool.Tag != null)
                {
                    Pools.Add(pool.Tag, pool);
                }
                else
                {
                    Debug.LogWarning("Pool: " + pool.name + " is missing some information. \n Please go back to Resources/Pools and fill in the information correctly");
                }
            }
        }

        // Create pools and put them in empty gameObjects to make sure the hierarchy window is clean.
        private void Start()
        {
            PoolDictionary = new Dictionary<string, Queue<GameObject>>();

            if (Pools.Count < 1)
            {
                return;
            }

            GameObject PoolsContainerObject = new GameObject("Pools");

            foreach (ScriptablePool pool in Pools.Values)
            {
                if (!PoolDictionary.ContainsKey(pool.Tag))
                {
                    GameObject containerObject = new GameObject(pool.Tag + "Pool");
                    containerObject.transform.parent = PoolsContainerObject.transform;
                    PoolParents.Add(pool.Tag, containerObject.transform);
                    Queue<GameObject> objectPool = new Queue<GameObject>();

                    for (int i = 0; i < pool.Amount; i++)
                    {
                        GameObject obj;
                        //Set DamagePopUp Pool parent to PopUpParent to make it function
                        if (pool.Tag == "DamagePopUp" || pool.Tag == "CritPopUp")
                        {
                            Destroy(containerObject);
                            if (!PopUpParent)
                            {
                                Debug.LogWarning("PopUp Parent not set up yet, create canvas for popups to be in");
                                continue;
                            }
                            obj = Instantiate(pool.Prefab, PopUpParent);
                        }
                        else
                        {
                            obj = Instantiate(pool.Prefab, containerObject.transform);
                        }
                        if(obj == null)
                        {
                            return;
                        }
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                    }
                    PoolDictionary.Add(pool.Tag, objectPool);
                }
            }
        }

        //Spawn an object from the corresponding pool with the given variables
        public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = PoolDictionary[tag].Peek();
            if (objectToSpawn.activeSelf)
            {
                Debug.Log("Active");
                objectToSpawn = Instantiate(Pools[tag].Prefab, PoolParents[tag].transform);
                PoolDictionary[tag].Enqueue(objectToSpawn);
                Debug.Log(PoolDictionary[tag].Count);
            }
            else
            {
                objectToSpawn = PoolDictionary[tag].Dequeue();
                PoolDictionary[tag].Enqueue(objectToSpawn);
            }

            if (objectToSpawn == null)
            {
                print("object is null");
            }

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            objectToSpawn.SetActive(false);
            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }

        private void ResetObject(GameObject objectToReset)
        {

        }

    }
}