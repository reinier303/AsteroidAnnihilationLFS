using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapons/MineWeapon", order = 997)]
    public class MineWeapon : Weapon
    {
        private string damageId;
        private string launchVelocityId;
        private string lifeTimeId;
        private string sizeId;

        public override void Initialize(PlayerStats pStats)
        {
            base.Initialize(pStats);

            sizeId = EnumCollections.WeaponStats.Size.ToString();
            damageId = EnumCollections.WeaponStats.Damage.ToString();
            launchVelocityId = EnumCollections.WeaponStats.LaunchVelocity.ToString();
            lifeTimeId = EnumCollections.WeaponStats.LifeTime.ToString();
        }

        public override void Fire(ObjectPooler objectPooler, Transform player, Vector2 velocity)
        {
            GameObject Mine = objectPooler.SpawnFromPool("PlayerMine", player.position - (player.up * 0.75f), Quaternion.identity);
            Mine.GetComponent<Rigidbody2D>().AddForce(-player.up * WeaponStatDictionary[launchVelocityId].GetValue(completionRewardStats));
            PlayerProjectile projectile = Mine.GetComponent<PlayerProjectile>();

            //Set projectile stat values
            projectile.Damage = WeaponStatDictionary[damageId].GetValue(completionRewardStats);
            projectile.LifeTime = WeaponStatDictionary[lifeTimeId].GetValue(completionRewardStats);
            if (IsCrit())
            {
                projectile.SetCrit();
            }
            //Cast to projectile type to set type specific variables
            MineProjectile mineProjectile = (MineProjectile)projectile;
            mineProjectile.Size = WeaponStatDictionary[sizeId].GetValue(completionRewardStats);

            projectile.Initialize();
            projectile.WeaponIndex = WeaponIndex;
            projectile.StartCoroutine(projectile.DisableAfterTime());
        }
    }
}
