using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class GroupedAsteroid : Asteroid
    {
        [SerializeField] protected AsteroidGroup groupScript;

        protected override IEnumerator CheckScreenEdges(float time)
        {
            if (transform.position.x > maxXandY.x ||
                transform.position.y > maxXandY.y ||
                transform.position.x < -maxXandY.x ||
                transform.position.y < -maxXandY.y)
            {
                EjectTrailEffects();
                groupScript.StartCoroutine(groupScript.AsteroidDisabled());
                gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(time);
            StartCoroutine(CheckScreenEdges(time));
        }

        protected override void Die()
        {
            groupScript.StartCoroutine(groupScript.AsteroidDisabled());
            base.Die();         
        }
    }

}
