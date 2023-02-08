using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace AsteroidAnnihilation
{
    public class GameManager : MonoBehaviour
    {
        //Singleton
        public static GameManager Instance;

        #region Script References

        public Player RPlayer;
        public InputManager RInputManager;

        public SpawnManager RSpawnManager;
        public ObjectPooler RObjectPooler;

        public CameraManager RCameraManager;
        public UIManager RUIManager;
        public AudioManager RAudioManager;

        private CameraOffset cameraOffset;
        #endregion

        public float Version;

        public delegate void OnEndGame();
        public OnEndGame onEndGame;

        public delegate void OnChangeScene();
        public OnEndGame onChangeScene;

        public bool PlayerAlive;
        private bool sleeping;

        public GameObject LoadingScreen;

        public bool isPaused;

        //Edge Restriction
        private Transform player;
        private Vector2 screenEdgeValues;
        [SerializeField] private ParticleSystem TeleportEffect;
        [SerializeField] public Transform EjectedTrailEffects;

        public int targetFramerate = -1;

        private void Awake()
        {
            Instance = this;

            Time.timeScale = 1;
            PlayerAlive = true;
            isPaused = false;
            player = RPlayer.transform;
            Application.targetFrameRate = targetFramerate;
            CheckVersion();
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F9))
            {
                Application.targetFrameRate = targetFramerate;
            }
        }

        //TODO EXTREMELY IMPORTANT!!!::Remove this when releasing game. This is testing only to make sure people don't use weird save files
        private void CheckVersion()
        {
            if (!ES3.KeyExists("version"))
            {
                ResetSave();
            }
            else if (ES3.Load<float>("version") < Version)
            {
                ResetSave();
            }
            ES3.Save("version", Version);
        }

        public void ResetSave(bool reloadScene = false)
        {
            //string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
            Debug.Log(Application.persistentDataPath + "/SaveFile.es3");
            File.Delete(Application.persistentDataPath + "/SaveFile.es3");
            onEndGame = null;
            onChangeScene= null;

            if (reloadScene) { LoadSceneAsync(SceneManager.GetActiveScene().buildIndex); }
        }

        private void Start()
        {
            cameraOffset = CameraOffset.Instance;
            screenEdgeValues = cameraOffset.ScreenEdgeValues + new Vector2(19f, 15f);
            /* Mission area might use in future
            if(cameraOffset.EdgeConfinerEnabled)
            {
                StartCoroutine(CheckScreenEdges());
            }
            */
        }

        private void OnApplicationQuit()
        {
            if (onEndGame != null) { onEndGame.Invoke(); }
        }

        public IEnumerator Sleep(float seconds)
        {
            if (sleeping)
            {
                yield break;
            }

            Time.timeScale = 0;
            sleeping = true;
            yield return new WaitForSecondsRealtime(seconds);
            Time.timeScale = 1;

            //Make sure multiple sleeps cant happen in sequence which caused the game to seem laggy
            yield return new WaitForSeconds(0.3f);
            sleeping = false;
        }

        public void LoadSceneAsync(int scene)
        {
            if (onChangeScene != null){ onChangeScene.Invoke();}
            ExtensionMethods.LoadSceneWithLoadingScreen(scene, LoadingScreen, this);
        }

        public void PauseGame(bool pauseMenu = true)
        {
            Time.timeScale = 0;
            isPaused = true;
            RAudioManager.AdjustMusicVolumePaused();
            if(pauseMenu)
            {
                RUIManager.OpenPauseMenu();
            }
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1;
            isPaused = false;
            RAudioManager.AdjustMusicVolumePaused();
            RUIManager.ClosePauseMenu();
        }


        /* Mission area might use in future
        private IEnumerator CheckScreenEdges()
        {
            if (player.position.x > screenEdgeValues.x ||
               player.position.y > screenEdgeValues.y ||
               player.position.x < -screenEdgeValues.x ||
               player.position.y < -screenEdgeValues.y)
            {               
                RUIManager.ShowMissionAreaText();
            }
            else
            {
                RUIManager.DisableMissionAreaText();
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(CheckScreenEdges());
        }

        public void TeleportPlayerToStart()
        {
            //particleEffect
            player.transform.position = Vector2.zero;
            TeleportEffect.transform.position = player.position;
            TeleportEffect.Play();
            cameraOffset.Offset = Vector2.zero;
        }
        */
    }
}

