using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    public IEnumerator ReturnToParentAfterTime(Transform parent)
    {
        float longestTime = 0;
        foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>())
        {
            system.Stop();
            if(system.main.startLifetime.constantMax > longestTime)
            {
                longestTime = system.main.startLifetime.constantMax;
            }
        }
        yield return new WaitForSeconds(longestTime);
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
