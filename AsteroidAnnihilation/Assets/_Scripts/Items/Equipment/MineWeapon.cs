using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/MineWeapon", order = 997)]
    public class MineWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, Dictionary<EnumCollections.WeaponStats, float> weaponStats, Dictionary<EnumCollections.WeaponStats, float> rarityStats)
        {
            base.Initialize(pStats, weaponStats, rarityStats);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity)
        {
            GameObject Mine = objectPooler.SpawnFromPool("PlayerMine", player.position - (player.up * 0.75f), Quaternion.identity);
            Mine.GetComponent<Rigidbody2D>().AddForce(-player.up * GetEquipmentStat(EnumCollections.WeaponStats.LaunchVelocity));
            PlayerProjectile projectile = Mine.GetComponent<PlayerProjectile>();

            //Set projectile stat values
            projectile.Damage = GetEquipmentStat(EnumCollections.WeaponStats.Damage);
            projectile.LifeTime = GetEquipmentStat(EnumCollections.WeaponStats.LifeTime);
            if (IsCrit())
            {
                projectile.SetCrit();
            }
            //Cast to projectile type to set type specific variables
            MineProjectile mineProjectile = (MineProjectile)projectile;
            mineProjectile.Size = GetEquipmentStat(EnumCollections.WeaponStats.Size);

            projectile.Initialize(GetEquipmentStat(EnumCollections.WeaponStats.Size));
            projectile.WeaponIndex = WeaponIndex;
            projectile.StartCoroutine(projectile.DisableAfterTime());
        }
    }
}
