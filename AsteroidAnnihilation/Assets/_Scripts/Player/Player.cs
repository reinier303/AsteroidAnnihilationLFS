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
        private Rigidbody2D rb;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
            //DontDestroyOnLoad(gameObject);

            GetSaveData();

            RPlayerAttack = GetComponent<PlayerAttack>();
            RPlayerMovement = GetComponent<PlayerMovement>();
            RPlayerEntity = GetComponent<PlayerEntity>();
            RPlayerStats = GetComponent<PlayerStats>();
            RUIManager = GameManager.Instance.RUIManager;
            rb = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetPlayerPositionAfterSeconds(float seconds)
        {
            return (Vector2)transform.position + rb.velocity * seconds;
        }

        public void DisablePlayerVelocity()
        {
            rb.velocity = Vector2.zero;
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