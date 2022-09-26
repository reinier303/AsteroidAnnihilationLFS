using AsteroidAnnihilation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/BulletWeapon", order = 997)]

    public class BulletWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, Dictionary<EnumCollections.EquipmentStats, float> weaponStats, Dictionary<EnumCollections.EquipmentStats, float> rarityStats)
        {
            base.Initialize(pStats, weaponStats, rarityStats);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition)
        {
            float spread = GetEquipmentStat(EnumCollections.EquipmentStats.ProjectileSpread);
            //Use ceil to int for powerUp to apply effects of 1.5 upward
            int count = Mathf.FloorToInt(GetEquipmentStat(EnumCollections.EquipmentStats.ProjectileCount));

            float angleIncrease;

            //Make spread increase for each bullet
            //Make sure there are no divisions by 0 in case of single projectile
            if (count == 1)
            {
                angleIncrease = 0;
                spread = 0;
            }
            else
            {
                spread += (count - 2) * 7.5f;

                angleIncrease = spread / (count - 1);
            }

            for (int i = 0; i < count; i++)
            {
                //Calculate new rotation
                float newRotation = (player.eulerAngles.z - angleIncrease * i) + (spread / 2);

                //Spawn Projectile with extra rotation based on projectile count
                GameObject projectileObject = objectPooler.SpawnFromPool(EnumCollections.PlayerProjectiles.PlasmaBullet.ToString(), player.position + (player.up / 3),
                Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, newRotation));

                //Initialize projectile
                PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                projectile.Damage = GetEquipmentStat(EnumCollections.EquipmentStats.Damage);
                if(IsCrit())
                {
                    projectile.SetCrit();
                }
                projectile.PlayerVelocity = velocity;
                projectile.ProjectileSpeed = GetEquipmentStat(EnumCollections.EquipmentStats.ProjectileSpeed);
                projectile.LifeTime = GetEquipmentStat(EnumCollections.EquipmentStats.LifeTime);
                projectile.Initialize(GetEquipmentStat(EnumCollections.EquipmentStats.Size));

                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }
    }
}
