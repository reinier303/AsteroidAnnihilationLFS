using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class TutorialManager : MonoBehaviour
    { 
        public static TutorialManager Instance;

        public bool inventoryTutorialDone;
        private SettingsManager settingsManager;
        private UIManager uiManager;
        private AudioManager audioManager;
        private int currentMessage;
        [SerializeField] private float typingSpeed = 0.02f;
        private bool lineTyping;
        private Message currentMessageData;

        [SerializeField] private GameObject movementElements;
        [SerializeField] private GameObject targets1, targets2, targets3, targets4;
        [SerializeField] private GameObject enemies1, enemies2, enemies3;
        [SerializeField] private GameObject walls;
        [SerializeField] private GameObject UIElements;

        private int targetsDestroyed;
        public bool TutorialDone;

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (CheckTutorialDone()) { return;}
            settingsManager = SettingsManager.Instance;
            uiManager = UIManager.Instance;
            audioManager = AudioManager.Instance;
            currentMessage = 0;
            ShowNextTutorialMessage();
        }

        //Ugly I know....
        //-1 == yes or no buttons
        public void ClickMessagePanel()
        {      
            if (lineTyping){ SkipTypeAnimation(); }
            else { 
                if (currentMessageData.timeOnScreen > 0) 
                {
                    uiManager.LmbPromptSwitch(true);
                    ShowNextTutorialMessage(); 
                } 
                switch(currentMessageData.timeOnScreen)
                {
                    case -1:
                        uiManager.QuestionPromptSwitch(true);
                        uiManager.LmbPromptSwitch(false);
                        break;
                    case -2:
                        EnableMovementTutorial();
                        uiManager.LmbPromptSwitch(false);
                        break;
                    case -3:
                        ShowNextTutorialMessage(1);
                        break;
                    case -4:
                        enemies1.SetActive(true);
                        uiManager.LmbPromptSwitch(false);
                        break;
                    case -5:
                        MissionManager.Instance.ToHub();
                        walls.SetActive(false);
                        ShowNextTutorialMessage();
                        break;
                    case -6:
                        UIElements.SetActive(false);
                        StopAllCoroutines();
                        SpawnManager.Instance.Spawning = true;
                        ES3.Save("tutorialDone", true);
                        break;
                }
            }
        }

        private bool CheckTutorialDone()
        {
            if (ES3.KeyExists("tutorialDone") || TutorialDone)
            {
                ES3.Save("tutorialDone", true);
                gameObject.SetActive(false);
                UIElements.SetActive(false);
                MissionManager.Instance.ToHub();
                return true;
            } else { return false;}
        }

        public void ShowNextTutorialMessage(int skip = 0)
        {
            currentMessageData = NextMessage(skip);
            StartCoroutine(DisplayLine(currentMessageData.MessageText));
        }

        private void SkipTypeAnimation()
        {
            StopAllCoroutines();
            uiManager.ChangeTutorialMessage(currentMessageData.MessageText);
            lineTyping = false;
        }

        private IEnumerator DisplayLine(string line)
        {        
            if(line == "") { yield break; }
            uiManager.ChangeTutorialMessage("");
            string currentText = "";

            lineTyping = true;
            string word = "";
            int closedArrowCount = 0;
            bool countingLetters = false;
            foreach (char letter in line.ToCharArray())
            {
                if(letter == '<' || countingLetters)
                {
                    countingLetters = true;
                    if (letter == '>') { closedArrowCount++; }
                    word += letter;
                    if(closedArrowCount == 2) 
                    { 
                        currentText += word;
                        countingLetters = false;
                    }
                    continue;
                }
                else
                {
                    currentText += letter;
                }
                audioManager.PlayAudio("Tutorial_Type");
                uiManager.ChangeTutorialMessage(currentText);
                yield return new WaitForSeconds(typingSpeed);
            }
            lineTyping = false;
        }

        public Message NextMessage(int skip = 0)
        {
            Message msg = settingsManager.tutorialSettings.Messages[currentMessage + skip];
            currentMessage += 1 + skip;
            return msg;
        }

        public void EnableMovementTutorial()
        {
            movementElements.SetActive(true);
        }

        public void EnableTargets()
        {
            targets1.SetActive(true);
        }

        public void TargetDestroyed()
        {
            targetsDestroyed++;
            CheckNextTargetGroup();
        }

        private void CheckNextTargetGroup()
        {
            switch(targetsDestroyed)
            {
                case 8:
                    targets2.SetActive(true);
                    break;
                case 16:
                    targets3.SetActive(true);
                    break;
                case 24:
                    targets4.SetActive(true);
                    break;
                case 32:
                    ShowNextTutorialMessage();
                    break;
                case 33:
                    enemies2.SetActive(true);
                    ShowNextTutorialMessage();
                    break;
                case 36:
                    enemies3.SetActive(true);
                    break;
                case 37:                   
                    ShowNextTutorialMessage();
                    break;
            }
        }
    }
}
