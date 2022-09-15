﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
namespace AsteroidAnnihilation
{
    public class BaseEnemy : BaseEntity
    {
        public Transform Player;

        //Stats
        public Stat ContactDamage;
        public Stat RotationSpeed;
        public Stat DroppedUnits;

        protected override void Start()
        {
            base.Start();
            Player = GameManager.Instance.RPlayer.transform;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerEntity player = collision.GetComponent<PlayerEntity>();

            if (player != null)
            {
                player.OnTakeDamage?.Invoke(ContactDamage.GetBaseValue(), false);
            }
        }

        protected virtual void Rotate()
        {
            Vector3 difference = Player.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, RotationSpeed.GetBaseValue() * Time.deltaTime);
            if (!gameManager.PlayerAlive)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90f);
            }
        }

        protected override void Die()
        {
            DropUnits(DroppedUnits.GetBaseValue());
            base.Die();
        }

        protected virtual void DropUnits(float units)
        {
            units *= UnityEngine.Random.Range(0.8f, 1.3f);

            int unitAmount5000 = (int)((units - units % 5000) / 5000);
            units -= unitAmount5000 * 5000;

            int unitAmount250 = (int)(((units - units % 250) / 250));
            units -= unitAmount250 * 250;

            int unitAmount5 = (int)((units / 5));

            for (int i = 0; i < unitAmount5000; i++)
            {
                GameObject unitObj = objectPooler.SpawnFromPool("Unit5000", transform.position, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveUnit();
            }
            for (int i = 0; i < unitAmount250; i++)
            {
                GameObject unitObj = objectPooler.SpawnFromPool("Unit250", transform.position, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveUnit();
            }
            for (int i = 0; i < unitAmount5; i++)
            {
                GameObject unitObj = objectPooler.SpawnFromPool("Unit5", transform.position, Quaternion.identity);
                Unit unit = unitObj.GetComponent<Unit>();
                unit.MoveUnit();
            }
        }
    }
}