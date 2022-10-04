﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/MineWeapon", order = 997)]
    public class MineWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, EquipmentManager equipmentManager)
        {
            base.Initialize(pStats, equipmentManager);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex)
        {
            GameObject Mine = objectPooler.SpawnFromPool("PlayerMine", player.position - (player.up * 0.75f), Quaternion.identity);
            Mine.GetComponent<Rigidbody2D>().AddForce(-player.up * GetEquipmentStat(EnumCollections.EquipmentStats.LaunchVelocity, weaponIndex));
            PlayerProjectile projectile = Mine.GetComponent<PlayerProjectile>();

            //Set projectile stat values
            float damage = GetEquipmentStat(EnumCollections.EquipmentStats.Damage, weaponIndex);
            float lifeTime = GetEquipmentStat(EnumCollections.EquipmentStats.LifeTime, weaponIndex);
            if (IsCrit())
            {
                projectile.SetCrit();
            }
            //Cast to projectile type to set type specific variables
            MineProjectile mineProjectile = (MineProjectile)projectile;

            projectile.Initialize(GetEquipmentStat(EnumCollections.EquipmentStats.Size, weaponIndex), damage, 0 ,lifeTime, IsCrit());
            projectile.WeaponIndex = WeaponIndex;
            projectile.StartCoroutine(projectile.DisableAfterTime());
        }
    }
}
