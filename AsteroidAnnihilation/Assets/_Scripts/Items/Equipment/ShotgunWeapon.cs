using AsteroidAnnihilation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/ShotgunWeapon", order = 997)]
    public class ShotgunWeapon : Weapon
    {
        public override void Initialize(PlayerStats pStats, EquipmentManager equipmentManager)
        {
            base.Initialize(pStats, equipmentManager);
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity, Vector2 weaponPosition, int weaponIndex)
        {
            float spread = GetEquipmentStat(EnumCollections.Stats.ProjectileSpread, weaponIndex);
            float count = (int)GetEquipmentStat(EnumCollections.Stats.ProjectileCount, weaponIndex);

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
                BaseProjectile projectile = projectileObject.GetComponent<BaseProjectile>();
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                float damage = GetEquipmentStat(EnumCollections.Stats.Damage, weaponIndex);

                projectile.PlayerVelocity = velocity;
                float projectileSpeed = GetEquipmentStat(EnumCollections.Stats.ProjectileSpeed, weaponIndex) * Random.Range(0.75f, 1.25f);

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
                float lifeTime = GetEquipmentStat(EnumCollections.Stats.LifeTime, weaponIndex) * lifeTimeMultiplier;
                projectile.Initialize(GetEquipmentStat(EnumCollections.Stats.Size, weaponIndex), damage, projectileSpeed, lifeTime, IsCrit());

                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }
    }
}