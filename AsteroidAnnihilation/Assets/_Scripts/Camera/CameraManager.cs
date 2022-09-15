using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace AsteroidAnnihilation
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineVirtualCamera cvCam;
        private CinemachineBasicMultiChannelPerlin NoiseAmplitude;

        private Animator cinemachineAnimator;

        void Awake()
        {
            NoiseAmplitude = cvCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            cvCam.Follow = Player.Instance.transform;
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
            NoiseAmplitude.m_AmplitudeGain = magnitude;

            yield return new WaitForSeconds(duration);

            while (NoiseAmplitude.m_AmplitudeGain < magnitude / 10)
            {
                NoiseAmplitude.m_AmplitudeGain = Mathf.Lerp(magnitude, 0, Time.deltaTime / duration);

                yield return null;
            }

            NoiseAmplitude.m_AmplitudeGain = 0;
        }

        public void DeathCamera()
        {
            cinemachineAnimator.SetBool("Death", true);
        }
    }
}