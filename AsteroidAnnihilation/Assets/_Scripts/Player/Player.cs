using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        //Player script references
        public PlayerAttack RPlayerAttack;
        public PlayerMovement RPlayerMovement;
        public PlayerEntity RPlayerEntity;
        public PlayerStats RPlayerStats;
        public GameObject Equipment;

        //Script references
        private UIManager RUIManager;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
            DontDestroyOnLoad(gameObject);

            GetSaveData();

            RPlayerAttack = GetComponent<PlayerAttack>();
            RPlayerMovement = GetComponent<PlayerMovement>();
            RPlayerEntity = GetComponent<PlayerEntity>();
            RPlayerStats = GetComponent<PlayerStats>();
            RUIManager = GameManager.Instance.RUIManager;
        }

        private void GetSaveData()
        {

        }


        public void SavePlayerData()
        {

        }

        protected virtual void OnBecameVisible()
        {

        }

        protected virtual void OnBecameInvisible()
        {

        }
    }
}