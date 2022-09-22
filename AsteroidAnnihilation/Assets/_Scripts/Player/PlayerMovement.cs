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

        public List<ParticleSystem> Engines;

        public PolygonCollider2D BackgroundCollider;
        private Vector2 backGroundSize;

        [SerializeField] private GameObject circle;

        public float offsetMultiplier = 1f;

        public Vector2 MovementInput;

        [SerializeField]private List<Vector3> lastInputs;

        private void Awake()
        {
            backGroundSize = BackgroundCollider.transform.localScale * 14.5f;
            lastInputs = new List<Vector3>();
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
            Engines = new List<ParticleSystem>();
            foreach (ParticleSystem ps in player.Equipment.GetComponentsInChildren<ParticleSystem>())
            {
                Engines.Add(ps);
            }
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
            if (playerStats.HasStat(EnumCollections.PlayerStats.MovementSpeed))
            {
                currentSpeed = playerStats.GetStatValue(EnumCollections.PlayerStats.MovementSpeed);
                Acceleration = playerStats.GetStatValue(EnumCollections.PlayerStats.MovementSpeed) * AccelerationMultiplier;
                Deceleration = playerStats.GetStatValue(EnumCollections.PlayerStats.MovementSpeed) * DecelerationMultiplier;
            }
        }

        private void GetBoostVariables()
        {
            if (playerStats.HasStat(EnumCollections.PlayerStats.BoostSpeed))
            {
                BoostSpeed = playerStats.GetStatValue(EnumCollections.PlayerStats.BoostSpeed);
                BoostRegen = playerStats.GetStatValue(EnumCollections.PlayerStats.BoostRegen);
                Fuel = playerStats.GetStatValue(EnumCollections.PlayerStats.BoostFuel);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if(Time.timeScale == 0)
            {
                return;
            }
            bool driftCheck = MovementInput.x <= 0.01f && MovementInput.y <= 0.01f && MovementInput.x >= -0.01f && MovementInput.y >= -0.01f ? true : false;
            if (inputManager.Attacking)
            {
                RotateToMouse();
            } else if (!inputManager.MovementInputZero()) { RotateToMoveDirection();}
            
            CheckEnginePS(inputManager.MovementInputZero());

            //fix drift
            if (driftCheck) 
            { 
                MovementInput = Vector2.zero;
            }
            Move();         
        }

        private void Move()
        {
            float axisX = inputManager.GetAxisSmoothHorizontal(currentSpeed, Acceleration, Deceleration, BoostSpeed);
            float axisY = inputManager.GetAxisSmoothVertical(currentSpeed, Acceleration, Deceleration, BoostSpeed);
            
            Vector2 input = new Vector2(axisX, axisY);

            //Debug.Log("Regular: " + new Vector2(axisX,axisY) + "Normalized: " + new Vector2(axisX, axisY).normalized);
            MovementInput = input * currentSpeed * Time.deltaTime;

            //MovementInput = new Vector2(axisX, axisY) * currentSpeed * Time.deltaTime;
            transform.position += (Vector3)MovementInput;

        }
        private void CheckEnginePS(bool input)
        {
            if(input)
            {
                foreach (ParticleSystem ps in Engines)
                {
                    if (!ps.isStopped) { ps.Stop(); }
                }
            }
            else
            {
                foreach (ParticleSystem ps in Engines)
                {
                    if (!ps.isEmitting) { ps.Play(); }
                }
            }
        }
        
        private void RotateToMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            target = hit.point + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier;

            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        }

        private void RotateToMoveDirection()
        {
            /*if (MovementInput.x <= 0.01f && MovementInput.y <= 0.01f && MovementInput.x >= -0.01f && MovementInput.y >= -0.01f && lastInputs.Count > 9)
            {
                Debug.Log(lastInputs[10]);
                target = transform.position + lastInputs[10] + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier;
            }
            else {
                if (lastInputs.Count < 10) {lastInputs.Add(MovementInput); }
                else 
                { 
                    lastInputs.RemoveAt(0);
                    lastInputs.Add(MovementInput);
                }*/
                target = transform.position + ((Vector3)MovementInput * 0.5f) + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier; 
            //}

            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }
}