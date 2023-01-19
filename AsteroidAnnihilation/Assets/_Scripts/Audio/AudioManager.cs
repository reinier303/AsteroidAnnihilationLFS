using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace AsteroidAnnihilation
{
    public class AudioManager : MonoBehaviour
    {
        [Range(0f,100f)]public float MasterVolumePercentage = 75;
        [Range(0f, 100f)] public float MusicVolumePercentage = 75;
        [Range(0f, 100f)] public float SFXVolumePercentage = 75;

        public static AudioManager Instance;

        private ObjectPooler objectPooler;
        private Dictionary<string, ScriptableAudio> Audios;
        [SerializeField] List<AudioMixerGroup> audioMixers;
        [SerializeField] private Coroutine dampenCoroutine;

        public AudioClip TutorialMusic;

        public List<AudioClip> MusicTracks;
        private List<AudioClip> musicPlayed = new List<AudioClip>();

        private AudioSource musicSource;

        public bool RandomAudioEnabled;

        public float regularMusicVolume;
        public float pausedMusicVolume;

        private void Awake()
        {
            Instance = this;
            LoadAudioFromResources();
            musicSource = GetComponent<AudioSource>();
            musicSource.volume = regularMusicVolume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100);
            if (RandomAudioEnabled)
            {
                MoveToNextSongRoundRobin();
            }
            StartCoroutine(CheckIfPlaying());
        }

        public void SetMasterVolume(System.Single volume)
        {
            MasterVolumePercentage = volume * 100;
            musicSource.volume = pausedMusicVolume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100);
        }

        public void SetMusicVolume(System.Single volume)
        {
            MusicVolumePercentage = volume * 100;
            musicSource.volume = pausedMusicVolume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100);
        }

        public void SetSFXVolume(System.Single volume)
        {
            SFXVolumePercentage = volume * 100;
        }

        private void LoadAudioFromResources()
        {
            Audios = new Dictionary<string, ScriptableAudio>();
            Object[] ScriptableAudios = Resources.LoadAll("Audios", typeof(ScriptableAudio));
            foreach (ScriptableAudio audio in ScriptableAudios)
            {
                //Check if pool info is filled.
                if (audio.Clips.Length > 0 && audio.Tag != null)
                {
                    Audios.Add(audio.Tag, audio);
                }
                else
                {
                    Debug.LogWarning("Pool: " + audio.name + " is missing some information. \n Please go back to Resources/Pools and fill in the information correctly");
                }
            }
        }

        private void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }

        public void PlayAudio(string tag)
        {
            ScriptableAudio sa = Audios[tag];
            AudioSource audio = objectPooler.SpawnFromPool("AudioSource", transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.clip = sa.Clips[Random.Range(0, sa.Clips.Length)];
            audio.volume = Random.Range(sa.VolumeMinMax.x, sa.VolumeMinMax.y) * (MasterVolumePercentage /100) * (SFXVolumePercentage / 100);
            audio.pitch = Random.Range(sa.PitchMinMax.x, sa.PitchMinMax.y);
            audio.outputAudioMixerGroup = sa.MixerGroup;
            audio.Play();
            audio.GetComponent<DisableAfterTime>().Disable(audio.clip.length + 1f);
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
                StartCoroutine(lerpVolume(musicSource.volume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100), pausedMusicVolume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100), 0.2f));
            }
            else
            {
                StartCoroutine(lerpVolume(musicSource.volume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100), regularMusicVolume * (MasterVolumePercentage / 100) * (MusicVolumePercentage / 100), 0.2f));
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

        public void DampenNonShotMixers()
        {
            if(dampenCoroutine == null)
            {
                dampenCoroutine = StartCoroutine(DampenNonShotMixersVolume());
            }
        }

        private IEnumerator DampenNonShotMixersVolume()
        {
    
            for(int i = 0; i < 5; i++)
            {
                foreach (AudioMixerGroup mixer in audioMixers)
                {
                    mixer.audioMixer.SetFloat("Volume", -0.8f * (i + 1));
                    yield return new WaitForSeconds(0.01f);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                foreach (AudioMixerGroup mixer in audioMixers)
                {
                    mixer.audioMixer.SetFloat("Volume", -4 + (0.8f * (i + 1)));
                    yield return new WaitForSeconds(0.02f);
                }
            }
            dampenCoroutine = null;
        }

    }
}