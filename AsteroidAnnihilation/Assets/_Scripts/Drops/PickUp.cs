using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class PickUp : Unit
    {
        public PickUpSO pickUpSO;
        private ResourceDatabase resourceDatabase;
        private PlayerInventory playerInventory;

        protected override void Awake()
        {
            base.Awake();
            //These are here because they are needed before Start
            resourceDatabase = ResourceDatabase.Instance;
            playerInventory = PlayerInventory.Instance;

        }

        protected override void Start()
        {
            base.Start();
        }

        public void InitializePickUp(string pickUpName)
        {
            if(pickUpName == "")
            {
                gameObject.SetActive(false);
            }
            pickUpSO = resourceDatabase.GetPickUp(pickUpName);
            Value = pickUpSO.Value;
            spriteRenderer.sprite = pickUpSO.sprite;
            MoveUnit();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                playerInventory.AddToInventory(pickUpSO.PickUpName, pickUpSO.Value);
                gameObject.SetActive(false);
            }
        }
    }

    public struct PickUpData
    {
        public string PickUpName;
        public float Amount;
    }
}


