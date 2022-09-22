using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class EnumCollections : MonoBehaviour
    {
        public enum EnemyFactions
        {
            TheSwarm,
            DravensGang,
            Meteorites,
            HyperesSingularity
        }

        public enum EnemyTypes
        {
            Asteroid,
            BigAsteroid,
            BlueAsteroid,
            HardAsteroid,
            OrbitingAsteroid,
            AsteroidGroup3Random,
            AsteroidGroup_3Alligned,
            SwarmPod,
            SwarmDrone,
            OutlawGrunt,
            OutlawMarauderMK1
        }

        public enum Backgrounds
        {
            BackgroundNebulaBlue,
            BackgroundNebulaGreen,
            BackgroundNebulaOrange,
            BackgroundNebulaPink,
            BackgroundNebulaPurple,
            BackgroundNebulaRed,
            BackgroundNebulaTurquoise,
            BackgroundNebulaYellow,
            BackgroundNebulaGrey,
        }

        public enum Weapons
        {
            None,
            PlasmaGun,
            PlasmaShotgun,
            EnergyMines,
        }

        public enum WeaponStats
        {
            Damage,
            LifeTime,
            FireRate,
            ProjectileSpeed,
            Size,
            ProjectileCount,
            LaunchVelocity,
            ProjectileSpread,
            EnergyPerShot
        }

        public enum PlayerStats
        {
            CurrentUnits,
            CurrentExperience,
            Health,
            UnitMultiplier,
            AsteroidMultiplier,
            MovementSpeed,
            BoostSpeed,
            BoostFuel,
            BoostRegen,
            MagnetRadius,
            CritRate,
            CritMultiplier,
            PowerUpChance,
            EnergyCapacity,
            EnergyRegen
        }

        public enum Unlocks
        {
            UnlockBoost,
            UnlockPlasmaShotgun,
            UnlockEnergyMines
        }

        public enum PlayerProjectiles
        {
            PlasmaBullet,
            EnergyMine,
            ShotgunBullet
        }

        public enum Rarities
        {
            Common,
            Augmented,
            Rare,
            Epic,
            Legendary,
            Mythic,
            Godly,
            Chaos
        }

        public enum ItemType
        {
            Material,
            Hull,
            EnergyCore,
            Engine,
            Weapon,
            Consumable
        }
    }
}
