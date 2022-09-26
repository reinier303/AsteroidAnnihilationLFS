using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/MineWeapon", order = 997)]
    public class MineWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, Dictionary<EnumCollections.EquipmentStats, float> weaponStats, Dictionary<EnumCollections.EquipmentStats, float> rarityStats)
        {
            base.Initialize(pStats, weaponStats, rarityStats);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition)
        {
            GameObject Mine = objectPooler.SpawnFromPool("PlayerMine", player.position - (player.up * 0.75f), Quaternion.identity);
            Mine.GetComponent<Rigidbody2D>().AddForce(-player.up * GetEquipmentStat(EnumCollections.EquipmentStats.LaunchVelocity));
            PlayerProjectile projectile = Mine.GetComponent<PlayerProjectile>();

            //Set projectile stat values
            projectile.Damage = GetEquipmentStat(EnumCollections.EquipmentStats.Damage);
            projectile.LifeTime = GetEquipmentStat(EnumCollections.EquipmentStats.LifeTime);
            if (IsCrit())
            {
                projectile.SetCrit();
            }
            //Cast to projectile type to set type specific variables
            MineProjectile mineProjectile = (MineProjectile)projectile;
            mineProjectile.Size = GetEquipmentStat(EnumCollections.EquipmentStats.Size);

            projectile.Initialize(GetEquipmentStat(EnumCollections.EquipmentStats.Size));
            projectile.WeaponIndex = WeaponIndex;
            projectile.StartCoroutine(projectile.DisableAfterTime());
        }
    }
}
