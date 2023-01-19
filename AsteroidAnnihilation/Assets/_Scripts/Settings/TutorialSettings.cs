using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "Settings/Tutorial Settings", order = 1001)]
    public class TutorialSettings : SerializedScriptableObject
    {
        public List<Message> Messages;
    }

    public struct Message
    {
        [TextArea(1, 5)] public string MessageText;
        public float timeOnScreen;
    }
}
