using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsteroidAnnihilation
{
    public class HubButton : MonoBehaviour
    {
        public void OnButtonPress()
        {
            MissionManager.Instance.StartCoroutine(MissionManager.Instance.MoveToHub());
        }
    }
}
