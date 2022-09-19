using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class GroupedAsteroid_Alligned : GroupedAsteroid
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        protected override Vector2 CalculateForce()
        {
            return groupScript.transform.GetComponent<AsteroidGroup_Alligned>().GroupForce;
        }

        protected override float CalculateTorque()
        {
            return groupScript.transform.GetComponent<AsteroidGroup_Alligned>().GroupTorque;
        }
    }
}
