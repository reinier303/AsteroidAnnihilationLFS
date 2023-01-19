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
            SwarmChaserDroneGroupSmall = 18,
            SwarmChaserDroneGroupMedium = 19,
            SwarmChaserDroneGroupLarge = 20,
            LoneSwarmChaserDrone = 21,
            Swarmling = 15,
            Seeker_ChargeDrone = 16,
            Seeker_SwarmPod = 25,
            SwarmDrone = 17,
            SwarmDroneGroupSmall = 22,
            SwarmDroneGroupMedium = 23,
            SwarmDroneGroupLarge = 24,
            OutlawGrunt = 10,
            OutlawMarauderMK1 = 11
        }

        public enum Bosses
        {
            SwarmPrincess = 0,
            SwarmRoyalWarrior = 1
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

        public enum Stats
        {
            Damage = 0,
            LifeTime = 1,
            FireRate = 2,
            ProjectileSpeed = 3,
            Size = 4,
            ProjectileCount = 5,
            LaunchVelocity = 6,
            ProjectileSpread = 7,
            EnergyPerShot= 8,
            EnergyRegen= 9,
            EnergyCapacity= 10,
            EnergyEfficiency= 11,
            Health= 12,
            HealthRegen = 23,
            MovementSpeed= 13,
            BoostSpeed= 14,
            BoostFuel= 15,
            BoostRegen= 16,
            CritRate= 17,
            CritMultiplier= 18,
            ExperienceMultiplier= 19,
            UnitsMultiplier= 20,
            MagnetRadius= 21,
            //Resistances
            PhysicalResistance= 22,
            HeatResistance = 23,
            ColdResistance = 24,
            EnergyResistance = 25,
            BioResistance = 26,
            EldritchResistance = 27,
            CelestialResistance = 28,
            ChaosResistance = 29,
            //Damage
            AllDamage = 30,
            PhysicalDamage = 31,
            HeatDamage = 32,
            ColdDamage = 33,
            EnergyAllDamage = 34,
            BioAllDamage = 35,
            EldritchAllDamage = 36,
            CelestialDamage = 37,
            ChaosDamage = 38,
        }

        public enum DamageType
        {
            Physical = 1,
            Heat = 2,
            Cold = 3,
            Energy = 4,
            Bio = 5,
            Eldritch = 6,
            Celestial = 7,
            Chaos = 8,
        }

        public enum PlayerStats
        {
            CurrentUnits,
            CurrentExperience,
            BaseHealth,
            UnitsMultiplier,
            ExperienceMultiplier,
            BaseMovementSpeed,
            BoostSpeed,
            BoostFuel,
            BoostRegen,
            BaseMagnetRadius,
            CritRate,
            CritMultiplier,
            PowerUpChance,
            BaseEnergyCapacity,
            BaseEnergyRegen,
            BaseHealthRegen,
            SkillPointsTotal,
            SkillPointsSpent,
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
            HullPlating,
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
