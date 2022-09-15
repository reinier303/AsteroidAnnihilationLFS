using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Player : MonoBehaviour
    {
        //Player script references
        public PlayerAttack RPlayerAttack;
        public PlayerMovement RPlayerMovement;
        public PlayerEntity RPlayerEntity;
        public PlayerStats RPlayerStats;

        //Script references
        private UIManager RUIManager;

        //Saving and loading
        public PlayerData Data;

        private void Awake()
        {
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