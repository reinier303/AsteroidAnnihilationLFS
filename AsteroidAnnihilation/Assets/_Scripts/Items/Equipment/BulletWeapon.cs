using AsteroidAnnihilation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/BulletWeapon", order = 997)]

    public class BulletWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, EquipmentManager equipmentManager)
        {
            base.Initialize(pStats, equipmentManager);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex)
        {
            float spread = GetEquipmentStat(EnumCollections.Stats.ProjectileSpread, weaponIndex);
            //Use ceil to int for powerUp to apply effects of 1.5 upward
            int count = Mathf.FloorToInt(GetEquipmentStat(EnumCollections.Stats.ProjectileCount, weaponIndex));

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
                GameObject projectileObject = objectPooler.SpawnFromPool(EnumCollections.PlayerProjectiles.PlasmaBullet.ToString(), player.position + player.TransformDirection(weaponPosition),
                Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, newRotation));

                //Initialize projectile
                BaseProjectile projectile = projectileObject.GetComponent<BaseProjectile>();
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                float damage = GetEquipmentStat(EnumCollections.Stats.Damage, weaponIndex);
                projectile.PlayerVelocity = velocity;
                float projectileSpeed = GetEquipmentStat(EnumCollections.Stats.ProjectileSpeed, weaponIndex);
                float lifeTime = GetEquipmentStat(EnumCollections.Stats.LifeTime, weaponIndex);
                projectile.Initialize(GetEquipmentStat(EnumCollections.Stats.Size, weaponIndex), damage ,projectileSpeed, lifeTime, IsCrit());

                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }

        public override void Fire2nd(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex)
        {
            float spread = GetEquipmentStat(EnumCollections.Stats.ProjectileSpread, weaponIndex);
            //Use ceil to int for powerUp to apply effects of 1.5 upward
            int count = Mathf.FloorToInt(GetEquipmentStat(EnumCollections.Stats.ProjectileCount, weaponIndex));

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
                GameObject projectileObject = objectPooler.SpawnFromPool(EnumCollections.PlayerProjectiles.PlasmaBullet.ToString(), player.position + player.TransformDirection(weaponPosition),
                Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, newRotation));

                //Initialize projectile
                BaseProjectile projectile = projectileObject.GetComponent<BaseProjectile>();
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                float damage = GetEquipmentStat(EnumCollections.Stats.Damage, weaponIndex) * 1.25f;
                projectile.PlayerVelocity = velocity;
                float projectileSpeed = GetEquipmentStat(EnumCollections.Stats.ProjectileSpeed, weaponIndex) * 1.25f;
                float lifeTime = GetEquipmentStat(EnumCollections.Stats.LifeTime, weaponIndex);
                projectile.Initialize(GetEquipmentStat(EnumCollections.Stats.Size, weaponIndex) * 1.35f, damage, projectileSpeed, lifeTime, IsCrit(), true);

                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }
    }
}
