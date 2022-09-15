using AsteroidAnnihilation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/ShotgunWeapon", order = 997)]
    public class ShotgunWeapon : Weapon
    {
        private string spreadId;
        private string countId;
        private string damageId;
        private string projectileSpeedId;
        private string lifeTimeId;

        public override void Initialize(PlayerStats pStats)
        {
            base.Initialize(pStats);

            spreadId = EnumCollections.WeaponStats.ProjectileSpread.ToString();
            countId = EnumCollections.WeaponStats.ProjectileCount.ToString();
            damageId = EnumCollections.WeaponStats.Damage.ToString();
            projectileSpeedId = EnumCollections.WeaponStats.ProjectileSpeed.ToString();
            lifeTimeId = EnumCollections.WeaponStats.LifeTime.ToString();
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity)
        {
            float spread = WeaponStatDictionary[spreadId].GetValue(completionRewardStats);
            float count = (int)WeaponStatDictionary[countId].GetValue(completionRewardStats);

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
                GameObject projectileObject = objectPooler.SpawnFromPool(ProjectileName, player.position + (player.up / 3),
                Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, newRotation + Random.Range(-4, 4)));

                //Initialize projectile
                PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();
                projectile.Initialize();
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                projectile.Damage = WeaponStatDictionary[damageId].GetValue(completionRewardStats);
                if (IsCrit())
                {
                    projectile.SetCrit();
                }

                projectile.PlayerVelocity = velocity;
                projectile.ProjectileSpeed = WeaponStatDictionary[projectileSpeedId].GetValue(completionRewardStats) * Random.Range(0.75f, 1.25f);

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
                projectile.LifeTime = WeaponStatDictionary[lifeTimeId].GetValue(completionRewardStats) * lifeTimeMultiplier;

                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }
    }
}