using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        List<AudioData> AudioDatas = new List<AudioData>();
        public List<AudioClip> MusicTracks;
        private List<AudioClip> musicPlayed = new List<AudioClip>();

        private AudioSource musicSource;

        public bool RandomAudioEnabled;

        public float regularMusicVolume;
        public float pausedMusicVolume;

        private void Awake()
        {
            Instance = this;
            musicSource = GetComponent<AudioSource>();
            musicSource.volume = regularMusicVolume;
            if (RandomAudioEnabled)
            {
                MoveToNextSongRoundRobin();
            }
            StartCoroutine(CheckIfPlaying());
        }

        public void AddAudio(string name, float duration)
        {
            AudioData data = new AudioData();
            data.name = name;
            data.duration = duration;
            AudioDatas.Add(data);
        }

        public void PlaySound(string audioName)
        {
            //AkSoundEngine.PostEvent(audioName, gameObject);
        }

        public float GetAudioLength(string audioName)
        {
            float length = 0;
            foreach (AudioData data in AudioDatas)
            {
                if (data.name == audioName)
                {
                    length = data.duration;
                }
            }
            if (length == 0)
            {
                Debug.LogWarning("Audio not found");
            }
            return length;
        }
        public virtual void MoveToNextSongRoundRobin()
        {
            //If all moves have been performed refill the moves list
            if (MusicTracks.Count == 0)
            {
                MusicTracks.AddRange(musicPlayed);
                musicPlayed.Clear();
            }

            //select random song from the move list
            AudioClip nextSong = MusicTracks[Random.Range(0, MusicTracks.Count)];

            //remove the next state from moves and add it to moves performed to make sure all moves will be performed in a random order.
            MusicTracks.Remove(nextSong);
            musicPlayed.Add(nextSong);

            musicSource.clip = nextSong;
            musicSource.Play();

            //StartCoroutine(WaitForNextSong());
        }

        private IEnumerator CheckIfPlaying()
        {
            if (!musicSource.isPlaying)
            {
                MoveToNextSongRoundRobin();
            }
            yield return new WaitForSecondsRealtime(1);
            StartCoroutine(CheckIfPlaying());
        }

        private IEnumerator WaitForNextSong()
        {
            Debug.Log(musicSource.clip.length);
            yield return new WaitForSeconds(musicSource.clip.length);
            MoveToNextSongRoundRobin();
        }

        public void AdjustMusicVolumePaused()
        {
            if(Time.timeScale == 0)
            {
                StartCoroutine(lerpVolume(musicSource.volume, pausedMusicVolume, 0.2f));
            }
            else
            {
                StartCoroutine(lerpVolume(musicSource.volume, regularMusicVolume, 0.2f));
            }
        }

        private IEnumerator lerpVolume(float StartValue, float EndValue, float LerpTime)
        {
            float difference = StartValue - EndValue;
            for (int i = 0; i < 20; i++)
            {
                musicSource.volume -= difference / 20;

                yield return new WaitForSecondsRealtime(LerpTime/ 20);
            }

            musicSource.volume = EndValue;
        }
    }

    [System.Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        public string name;
        public float duration;
    }
}