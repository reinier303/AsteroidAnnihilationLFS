using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsteroidAnnihilation
{
    public class MissionComplete : MonoBehaviour
    {
        [SerializeField] float inTime, waitTime ,outTime;
        [SerializeField] LeanTweenType inEaseType, outEaseType;

        [SerializeField] private TextMeshProUGUI unitsReward;
        [SerializeField] private TextMeshProUGUI xpReward;

        private void OnEnable()
        {
            Mission mission = MissionManager.Instance.GetCurrentMission();
            unitsReward.text = "Units: +" + mission.UnitsReward;
            xpReward.text = "XP: +" + mission.ExperienceReward;
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            transform.localScale = Vector3.zero;
            transform.LeanScale(new Vector3(1,1,1), inTime).setEase(inEaseType);
            yield return new WaitForSeconds(inTime + waitTime);
            transform.LeanScale(Vector3.zero, outTime).setEase(outEaseType);
            yield return new WaitForSeconds(outTime);
            gameObject.SetActive(false);
        }
    }
}
