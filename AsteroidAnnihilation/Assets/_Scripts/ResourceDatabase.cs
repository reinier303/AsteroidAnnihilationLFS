using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class ResourceDatabase : MonoBehaviour
    {
        public static ResourceDatabase Instance;

        private Dictionary<string, PickUpSO> PickUps;

        //Shop Item Dictionary

        private void Awake()
        {
            Instance = this;

            LoadPickUps();
        }

        private void LoadPickUps()
        {
            PickUps = new Dictionary<string, PickUpSO>();

            Object[] ResourcedPickUps = Resources.LoadAll("PickUps", typeof(PickUpSO));

            foreach (PickUpSO pickUp in ResourcedPickUps)
            {
                //Check if PickUp info is filled.
                if (pickUp.PickUpName != "" && pickUp.Value != 0 && pickUp.sprite != null)
                {
                    PickUps.Add(pickUp.PickUpName, pickUp);
                }
                else
                {
                    Debug.LogWarning("PickUp: " + pickUp.PickUpName + " is missing some information. \n Please go back to Resources/PickUps and fill in the information correctly");
                }
            }
        }

        public PickUpSO GetPickUp(string name)
        {          
            if (PickUps.ContainsKey(name))
            {
                PickUpSO pickUp = PickUps[name];
                return pickUp;
            }
            else
            {
                Debug.Log("PickUp with name: " + name + " not found...");
                return null;
            }
        }
    }

}
