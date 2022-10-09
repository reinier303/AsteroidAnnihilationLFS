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
            Asteroid = 0,
            BigAsteroid = 1,
            BlueAsteroid = 2,
            HardAsteroid = 3,
            OrbitingAsteroid = 4,
            AsteroidGroup3Random = 5,
            AsteroidGroup_3Alligned = 6,
            SwarmPod = 7,
            SwarmPodGroupSmall = 8,
            SwarmPodGroupMedium = 12,
            SwarmPodGroupLarge = 13,
            LoneSwarmPod = 14,
            SwarmChaserDrone = 9,
            OutlawGrunt = 10,
            OutlawMarauderMK1 = 11
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

        public enum EquipmentStats
        {
            Damage,
            LifeTime,
            FireRate,
            ProjectileSpeed,
            Size,
            ProjectileCount,
            LaunchVelocity,
            ProjectileSpread,
            EnergyPerShot,
            EnergyRegen,
            EnergyCapacity,
            EnergyEfficiency,
            Health,
            MovementSpeed,
            BoostSpeed,
            BoostFuel,
            BoostRegen,
            CritRate,
            CritMultiplier,
            ExperienceMultiplier,
            UnitsMultiplier,
            MagnetRadius,
            Resistance,
        }

        public enum PlayerStats
        {
            CurrentUnits,
            CurrentExperience,
            Health,
            UnitsMultiplier,
            ExperienceMultiplier,
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

        public enum EnemyProjectiles
        {
            SwarmBullet1 = 0
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
            Consumable,
            /* Accesory */ShipComponent,
            Inventory
        }

        public enum ShipType
        {
            Fighter,//Standard ship
            Warship,//Slower more weapon slots
            Buster,//Single very strong weapon slot
            Ghost,//Very Fast and agile ship... Don't know implementation yet
            Carrier//Drone ship
        }

        public enum ExplosionFX
        {
            SwarmExplosion1,
            SwarmExplosion2,
            SwarmExplosionBoss,
            AsteroidExplosion1,
            AsteroidExplosion2,
            AsteroidExplosionBoss,
            PlayerExplosion,
        }

        public enum PermanenceSprites
        {
            SwarmPermanence,
            AsteroidPermanence,
            BanditPermanence
        }
    }
}
