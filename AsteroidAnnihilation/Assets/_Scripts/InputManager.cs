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
            InputEnabled = true;
            playerAttack = gameManager.RPlayer.RPlayerAttack;
            boost = 1;
        }

        private void Update()
        {
            
            PauseInput();
            if (!InputEnabled)
            {
                Attacking = false;
                return;
            }
            if (gameManager.isPaused)
            {
                Attacking = false;
                return;
            }

            //Pause dependent input
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
        }

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
                //TODO::Play cant switch sound cue
            }
        }


        public float GetAxisSmoothHorizontal(float speed, float acceleration, float deceleration, float boostSpeed)
        {
            boost = Boost(boostSpeed);

            //Right button input
            if (Input.GetKey(RightButton) || Input.GetKey(RightButton2))
            {
                axisX += acceleration * boost * Time.deltaTime;
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
                axisX -= acceleration * boost * Time.deltaTime;
                if (axisX < -speed * boost)
                {
                    axisX = -speed * boost;
                }
            }
            else if (axisX < 0)
            {
                axisX += deceleration * boost * Time.deltaTime;
            }
            return axisX;
        }


        public float GetAxisSmoothVertical(float speed, float acceleration, float deceleration, float boostSpeed)
        {
            boost = Boost(boostSpeed);

            //Right button input
            if (Input.GetKey(UpButton) || Input.GetKey(UpButton2))
            {
                axisY += acceleration * boost * Time.deltaTime;
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
                axisY -= acceleration * boost * Time.deltaTime;
                if (axisY < -speed * boost)
                {
                    axisY = -speed * boost;
                }
            }
            else if (axisY < 0)
            {
                axisY += deceleration * boost * Time.deltaTime;
            }

            return axisY;
        }

        public float Boost(float boostSpeed)
        {
            if(boost <= boostSpeed && Input.GetKey(BoostButton) || Input.GetKey(BoostButton2))
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

