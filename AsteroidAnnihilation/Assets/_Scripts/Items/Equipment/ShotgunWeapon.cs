using AsteroidAnnihilation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/ShotgunWeapon", order = 997)]
    public class ShotgunWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, Dictionary<EnumCollections.EquipmentStats, float> weaponStats, Dictionary<EnumCollections.EquipmentStats, float> rarityStats)
        {
            base.Initialize(pStats, weaponStats, rarityStats);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition)
        {
            float spread = GetEquipmentStat(EnumCollections.EquipmentStats.ProjectileSpread);
            float count = (int)GetEquipmentStat(EnumCollections.EquipmentStats.ProjectileCount);

            float angleIncrease;

            //Make sure there are no divisions by 0 in case of single projectile
            if (count == 1)
            {
                angleIncrease = 0;
                spread = 0;
            }
            else
            {
                angleIncrease = spread / (count - 1);
            }

            for (int i = 0; i < count; i++)
            {
                //Calculate new rotation
                float newRotation = (player.eulerAngles.z - angleIncrease * i) + (spread / 2);

                //Spawn Projectile with extra rotation based on projectile count
                GameObject projectileObject = objectPooler.SpawnFromPool(ProjectileType.ToString(), player.position + (player.up / 3),
                Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, newRotation + Random.Range(-4, 4)));

                //Initialize projectile
                PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();
                projectile.Initialize(GetEquipmentStat(EnumCollections.EquipmentStats.Size));
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                projectile.Damage = GetEquipmentStat(EnumCollections.EquipmentStats.Damage);
                if (IsCrit())
                {
                    projectile.SetCrit();
                }

                projectile.PlayerVelocity = velocity;
                projectile.ProjectileSpeed = GetEquipmentStat(EnumCollections.EquipmentStats.ProjectileSpeed) * Random.Range(0.75f, 1.25f);

                //Random lifetime calculations
                float lifeTimeMultiplier = Random.Range(0.1f, 2);

                //Prioritize numbers closer to 1.
                if(lifeTimeMultiplier < 0.75f || lifeTimeMultiplier > 1.25f)
                {
                    lifeTimeMultiplier = Random.Range(0.1f, 2);
                    if (lifeTimeMultiplier < 0.5f || lifeTimeMultiplier > 1.5f)
                    {
                        lifeTimeMultiplier = Random.Range(0.1f, 2);
                        if (lifeTimeMultiplier < 0.25f || lifeTimeMultiplier > 1.75f)
                        {
                            lifeTimeMultiplier = Random.Range(0.1f, 2);
                        }
                    }
                }
                projectile.LifeTime = GetEquipmentStat(EnumCollections.EquipmentStats.LifeTime) * lifeTimeMultiplier;

                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }
    }
}