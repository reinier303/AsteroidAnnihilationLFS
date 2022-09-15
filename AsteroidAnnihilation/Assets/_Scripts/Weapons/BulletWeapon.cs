using AsteroidAnnihilation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/BulletWeapon", order = 997)]

    public class BulletWeapon : Weapon
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
            //Use ceil to int for powerUp to apply effects of 1.5 upward
            int count = Mathf.FloorToInt(WeaponStatDictionary[countId].GetValue(completionRewardStats));

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
                GameObject projectileObject = objectPooler.SpawnFromPool(ProjectileName, player.position + (player.up / 3),
                Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, newRotation));

                //Initialize projectile
                PlayerProjectile projectile = projectileObject.GetComponent<PlayerProjectile>();
                projectile.WeaponIndex = WeaponIndex;

                //Set projectile stat values
                projectile.Damage = WeaponStatDictionary[damageId].GetValue(completionRewardStats);
                if(IsCrit())
                {
                    projectile.SetCrit();
                }
                projectile.PlayerVelocity = velocity;
                projectile.ProjectileSpeed = WeaponStatDictionary[projectileSpeedId].GetValue(completionRewardStats);
                projectile.LifeTime = WeaponStatDictionary[lifeTimeId].GetValue(completionRewardStats);


                projectile.StartCoroutine(projectile.DisableAfterTime());
            }

        }
    }
}
