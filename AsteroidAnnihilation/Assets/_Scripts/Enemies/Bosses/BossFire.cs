using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    [CreateAssetMenu(menuName = "BossMoves/BossFire", order = 997)]
    public class BossFire : BaseBossMove
    {
        public float Size;
        public float ChargeTime;
        public float FireRate;
        public int Amount;
        public float Spread;
        public float BulletSpeed;
        public float ProjectileLifeTime = 1f;
        public EnumCollections.EnemyProjectiles Projectile;
        private ObjectPooler objectPooler;
        private Transform bossTransform;

        public override void ExecuteMove(Transform bossTransform, MonoBehaviour runOn, ObjectPooler objectPooler)
        {
            if(this.objectPooler == null) { this.objectPooler = objectPooler; }
            if (this.bossTransform == null) { this.bossTransform = bossTransform; }
            runOn.StartCoroutine(Fire());
        }

        private IEnumerator Fire()
        {
            //Create variables unattached to scriptable object
            int tempAmount = Amount;
            float tempSpread = Spread;
            float timer = 0;

            float angleIncrease;

            //Make spread increase for each bullet
            //Make sure there are no divisions by 0 in case of single projectile
            if (tempAmount == 1)
            {
                angleIncrease = 0;
                tempSpread = 0;
            }
            else
            {
                tempSpread += (tempAmount - 2) * 7.5f;

                angleIncrease = tempSpread / (tempAmount - 1);
            }

            ChargeEffect charge = objectPooler.SpawnFromPool("SwarmChargeEffect", bossTransform.position + bossTransform.up * 2f, bossTransform.rotation).GetComponent<ChargeEffect>();
            charge.transform.parent = bossTransform;
            charge.Initialize(ChargeTime, Size);
            yield return new WaitForSeconds(ChargeTime);

            while (timer < MoveTime)
            {
                for (int i = 0; i < tempAmount; i++)
                {
                    float newRotation = (bossTransform.eulerAngles.z - angleIncrease * i) + (tempSpread / 2);

                    BaseProjectile projectile = objectPooler.SpawnFromPool(Projectile.ToString(),
                        bossTransform.position + bossTransform.up * 2f,
                        Quaternion.Euler(bossTransform.eulerAngles.x, bossTransform.eulerAngles.y, newRotation)).GetComponent<BaseProjectile>();
                    projectile.Initialize(Size, 1, BulletSpeed, ProjectileLifeTime, false);
                }
                yield return new WaitForSeconds(1 / FireRate);
                timer += 1 / FireRate;
            }
        }
    }
}
