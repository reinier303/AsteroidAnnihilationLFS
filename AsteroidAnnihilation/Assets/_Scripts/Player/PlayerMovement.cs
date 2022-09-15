using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace AsteroidAnnihilation
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Variables")]
        private float currentSpeed;
        private float Acceleration;
        private float Deceleration;
        public float AccelerationMultiplier;
        public float DecelerationMultiplier;

        [Header("Boost Variables")]
        public float BoostSpeed = 1.5f;
        public float BaseSpeed;

        public float Fuel;
        public float BoostRegen;

        private float cameraDistance;
        private float screenRatio;

        private Vector3 target;
        private Camera cam;
        [SerializeField] CinemachineConfiner confiner;

        private GameManager gameManager;
        private Player player;
        private PlayerStats playerStats;
        private InputManager inputManager;

        public ParticleSystem EngineFire;

        public PolygonCollider2D BackgroundCollider;
        private Vector2 backGroundSize;

        [SerializeField] private GameObject circle;

        public float offsetMultiplier = 1f;

        public Vector3 MovementInput;

        private void Awake()
        {
            backGroundSize = BackgroundCollider.transform.localScale * 14.5f;
        }

        private void Start()
        {
            cam = Camera.main;
            SceneManager.sceneLoaded += OnSceneLoaded;
            gameManager = GameManager.Instance;
            player = gameManager.RPlayer;
            playerStats = GetComponent<PlayerStats>();
            inputManager = gameManager.RInputManager;
            GetMovementVariables();
            GetBoostVariables();
            //GetCameraDistance();

            screenRatio = (float)Screen.height / (float)Screen.width;
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            cam = Camera.main;
        }

        private void GetCameraDistance()
        {
            CinemachineComponentBase componentBase = confiner.VirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                cameraDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance;
            }
        }

        private void GetMovementVariables()
        {
            if (playerStats.Stats.ContainsKey("MovementSpeed"))
            {
                currentSpeed = playerStats.Stats["MovementSpeed"].GetBaseValue();
                Acceleration = playerStats.Stats["MovementSpeed"].GetBaseValue() * AccelerationMultiplier;
                Deceleration = playerStats.Stats["MovementSpeed"].GetBaseValue() * DecelerationMultiplier;
            }
        }

        private void GetBoostVariables()
        {
            if (playerStats.Stats.ContainsKey("BoostSpeed"))
            {
                BoostSpeed = playerStats.Stats["BoostSpeed"].GetBaseValue();
                BoostRegen = playerStats.Stats["BoostRegen"].GetBaseValue();
                Fuel = playerStats.Stats["Fuel"].GetBaseValue();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if(Time.timeScale == 0)
            {
                return;
            }
            Move();
            Rotate();
        }

        private void Move()
        {
            float axisX = inputManager.GetAxisSmoothHorizontal(currentSpeed, Acceleration, Deceleration, BoostSpeed);
            float axisY = inputManager.GetAxisSmoothVertical(currentSpeed, Acceleration, Deceleration, BoostSpeed);

            MovementInput = new Vector3(axisX, axisY, 0) * currentSpeed * Time.deltaTime;

            transform.position += MovementInput;

        }
        
        private void Rotate()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            target = hit.point + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier;

            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        }
    }
}