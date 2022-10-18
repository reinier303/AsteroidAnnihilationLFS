using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private GameManager gameManager;
        private PlayerAttack playerAttack;
        private UIManager uiManager;

        [Header("General")]
        public bool InputEnabled;
        public KeyCode PauseButton;
        public KeyCode PauseButton2;

        [Header("Movement")]
        public KeyCode BoostButton;
        public KeyCode BoostButton2;
        public KeyCode UpButton;
        public KeyCode UpButton2;
        public KeyCode DownButton;
        public KeyCode DownButton2;
        public KeyCode LeftButton;
        public KeyCode LeftButton2;
        public KeyCode RightButton;
        public KeyCode RightButton2;

        public KeyCode Weapon1;
        public KeyCode Weapon2;
        public KeyCode Weapon3;
        public KeyCode Weapon4;
        public KeyCode Weapon5;

        public KeyCode Inventory;

        private float axisX;
        private float axisY;

        private bool boosting;
        private float boost;
        public bool Attacking;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
            playerAttack = gameManager.RPlayer.RPlayerAttack;
            boost = 1;
        }

        private void OnEnable()
        {
            InputEnabled = true;
        }

        public void EnableInput()
        {
            InputEnabled = true;
        }

        private void Update()
        {      
            if (!InputEnabled)
            {
                Attacking = false;
                return;
            }
            GetAxisNormalizedCoef();
            PauseInput();
            if (gameManager.isPaused)
            {
                Attacking = false;
                return;
            }
            //Pause dependent input
            if (Input.GetKeyDown(Inventory))
            {
                uiManager.OpenInventory();
            }
            WeaponInput();
        }

        private void PauseInput()
        {
            if (Input.GetKeyDown(PauseButton) || Input.GetKeyDown(PauseButton2))
            {
                if (Time.timeScale == 1)
                {
                    gameManager.PauseGame();
                }
                else
                {
                    gameManager.UnpauseGame();
                }
            }
        }

        public void WeaponInput()
        {
            if (Input.GetMouseButton(0))
            {
                Attacking = true;
            }
            else { Attacking = false; }

            /*
            if (Input.GetKeyDown(Weapon1))
            {
                ChangeToWeapon(0);
            }
            else if (Input.GetKeyDown(Weapon2))
            {
                ChangeToWeapon(1);
            }
            else if (Input.GetKeyDown(Weapon3))
            {
                ChangeToWeapon(2);
            }
            else if (Input.GetKeyDown(Weapon4))
            {
                ChangeToWeapon(3);
            }
            else if (Input.GetKeyDown(Weapon5))
            {
                ChangeToWeapon(4);
            }
            */
        }

        //Old ChangeWeapon() might use/recycle later
        /*
        public void ChangeToWeapon(int weapon)
        {
            if(playerAttack.ChangeWeapon(weapon))
            {
                UpgradePanel upgradePanel = UpgradePanel.Instance;
                upgradePanel.GetCurrentWeapon();
                upgradePanel.SetupUpgradeUI();
            }
            else
            {
                Debug.Log("Weapon not unlocked");
            }
        }
        */

        public bool MovementInputZero()
        {
            return !Input.GetKey(RightButton) &&
                !Input.GetKey(RightButton2) &&
                !Input.GetKey(LeftButton) &&
                !Input.GetKey(LeftButton2) &&
                !Input.GetKey(UpButton) &&
                !Input.GetKey(UpButton2) &&
                !Input.GetKey(DownButton) &&
                !Input.GetKey(DownButton2);
        }

        private Vector2 GetAxisNormalizedCoef()
        {
            float x = 0;
            float y = 0;
            if(Input.GetKey(RightButton) || Input.GetKey(RightButton2))
            {
                x = 1;
            }
            else if (Input.GetKey(LeftButton) || Input.GetKey(LeftButton2))
            {
                x = -1;
            }
            if (Input.GetKey(UpButton) || Input.GetKey(UpButton2))
            {
                y = 1;
            }
            else if (Input.GetKey(DownButton) || Input.GetKey(DownButton2))
            {
                y = -1;
            }
            Vector2 axises = new Vector2(Mathf.Abs(x), Mathf.Abs(y)).normalized;
            //axises = new Vector2(Mathf.Pow(axises.x, 2) , Mathf.Pow(axises.y ,2));
            return axises;
        }

        public float GetAxisSmoothHorizontal(float speed, float acceleration, float deceleration, float boostSpeed)
        {
            if (!InputEnabled) { return 0; }

            boost = Boost(boostSpeed);

            //Right button input
            if (Input.GetKey(RightButton) || Input.GetKey(RightButton2))
            {
                if (axisX < 0) { axisX = 0; }
                axisX += (acceleration * boost * Time.deltaTime) * GetAxisNormalizedCoef().x;
                if (axisX > speed * boost)
                {
                    axisX = speed * boost;
                }
            }
            else if(axisX > 0)
            {
                axisX -= deceleration * boost * Time.deltaTime;
            }

            //Left button input
            if (Input.GetKey(LeftButton) || Input.GetKey(LeftButton2))
            {
                if (axisX > 0) { axisX = 0; }
                axisX -= (acceleration * boost * Time.deltaTime) * GetAxisNormalizedCoef().x;
                if (axisX < -speed * boost)
                {
                    axisX = -speed * boost;
                }
            }
            else if (axisX < 0)
            {
                axisX += deceleration * boost * Time.deltaTime;
            }
            return axisX * GetAxisNormalizedCoef().x;
        }


        public float GetAxisSmoothVertical(float speed, float acceleration, float deceleration, float boostSpeed)
        {
            if (!InputEnabled) { return 0; }

            boost = Boost(boostSpeed);
            //Right button input
            if (Input.GetKey(UpButton) || Input.GetKey(UpButton2))
            {
                if (axisY < 0) { axisY = 0; }

                axisY += (acceleration * boost * Time.deltaTime) * GetAxisNormalizedCoef().y;
                if (axisY > speed * boost)
                {
                    axisY = speed * boost;
                }
            }
            else if (axisY > 0)
            {
                axisY -= deceleration * boost * Time.deltaTime;
            }

            //Left button input
            if (Input.GetKey(DownButton) || Input.GetKey(DownButton2))
            {
                if (axisY > 0) { axisY = 0; }

                axisY -= (acceleration * boost * Time.deltaTime) * GetAxisNormalizedCoef().y;
                if (axisY < -speed * boost)
                {
                    axisY = -speed * boost;
                }
            }
            else if (axisY < 0)
            {
                axisY += deceleration * boost * Time.deltaTime;
            }

            return axisY * GetAxisNormalizedCoef().y;
        }

        public float Boost(float boostSpeed)
        {
            if (!InputEnabled) { return 0; }

            if (boost <= boostSpeed && Input.GetKey(BoostButton) || Input.GetKey(BoostButton2))
            {
                boosting = true;
            }
            else
            {
                boosting = false;
            }
            if(boosting)
            {
                boost += boostSpeed * Time.deltaTime / 10;
            }
            else if (boost > 1)
            {
                boost -= boostSpeed * Time.deltaTime / 10;

            }
            return boost;
        }
    }
}

