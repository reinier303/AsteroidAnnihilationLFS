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
        }

        public enum ShipType
        {
            Fighter,//Standard ship
            Warship,//Slower more weapon slots
            Buster,//Single very strong weapon slot
            Ghost,//Very Fast and agile ship... Don't know implementation yet
            Carrier//Drone ship
        }
    }
}
