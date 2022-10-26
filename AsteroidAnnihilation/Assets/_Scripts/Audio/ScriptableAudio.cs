using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Audio", order = 999)]
    public class ScriptableAudio : ScriptableObject
    {
        public string Tag;
        public AudioClip[] Clips;
        public AudioMixerGroup MixerGroup;
        public Vector2 VolumeMinMax;
        public Vector2 PitchMinMax;

    }
}