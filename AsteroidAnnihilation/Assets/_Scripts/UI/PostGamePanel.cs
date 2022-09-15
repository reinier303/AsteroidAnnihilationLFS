using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsteroidAnnihilation
{
    public class PostGamePanel : MonoBehaviour
    {
        public TMP_Text UnitsEarnedText;
        public TMP_Text ExperienceEarnedText;
        public TMP_Text RareDropsFoundText;
        public TMP_Text EnemiesKilledText;
        public TMP_Text BossesKilledText;
        public TMP_Text RunTimeSecondsText;
        public TMP_Text KilledByText;

        public float UnitsEarned;
        public float ExperienceEarned;
        public int RareDropsFound;
        public float EnemiesKilled;
        public float BossesKilled;
        public float RunTimeSeconds;
        public string KilledBy;

        private Dictionary<TMP_Text, float> textsAndValues = new Dictionary<TMP_Text, float>();

        public IEnumerator UpdateSummary()
        {
            yield return new WaitForSeconds(4.5f);
            textsAndValues.Add(UnitsEarnedText, UnitsEarned);
            textsAndValues.Add(ExperienceEarnedText, ExperienceEarned);
            textsAndValues.Add(RareDropsFoundText, RareDropsFound);
            textsAndValues.Add(EnemiesKilledText, EnemiesKilled);
            textsAndValues.Add(BossesKilledText, BossesKilled);

            foreach(TMP_Text textt in textsAndValues.Keys)
            {

                float LerpTime = 1.5f;
                float StartTime = Time.time;
                float EndTime = StartTime + LerpTime;

                if (textsAndValues[textt] <= 1)
                {
                    LerpTime = 0.1f;
                }

                while (Time.time < EndTime)
                {
                    float timeProgressed = (Time.time - StartTime) / LerpTime;  // this will be 0 at the beginning and 1 at the end.
                    textt.text = "\n" + Mathf.Round(Mathf.Lerp(0, textsAndValues[textt], timeProgressed));

                    yield return new WaitForFixedUpdate();
                }
                textt.text = "\n" + Mathf.Round(textsAndValues[textt]);
            }

            /*
            UnitsEarnedText.text += "\n" +  UnitsEarned;
            ExperienceEarnedText.text = "Experience Gained:\n" + ExperienceEarned;
            RareDropsFoundText.text = "Rare Drops Found:\n" + RareDropsFound;
            EnemiesKilledText.text = "Enemies Killed:\n" + EnemiesKilled;
            BossesKilledText.text = "Bosses Killed:\n" + BossesKilled;
            */
            //Use 3 lerps for each value
            RunTimeSecondsText.text = "Run Time:\n" + ConvertToTime(RunTimeSeconds);
            KilledByText.text = "Killed By:\n" + KilledBy;

        }

        private string ConvertToTime(float totalSeconds)
        {
            int hours;
            int minutes;
            int seconds;
            hours = (int)totalSeconds / 3600;
            minutes = (int)(totalSeconds % 3600) / 60;
            seconds = (int)(totalSeconds % 3600) % 60;

            return hours + ":" + minutes + ":" + seconds;
        }
    }
}