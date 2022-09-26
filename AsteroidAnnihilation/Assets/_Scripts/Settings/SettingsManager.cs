using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance;

        public PlayerShipSettings playerShipSettings;
        public GeneralItemSettings generalItemSettings;
        public PlayerLevelSettings playerLevelSettings;

        public AreaGenerationSettings generationSettingsT1;
        public AreaGenerationSettings generationSettingsT2;

        private void Awake()
        {
            Instance = this;

            playerShipSettings = (PlayerShipSettings)Resources.Load("Settings/PlayerShipSettings");
            generalItemSettings = (GeneralItemSettings)Resources.Load("Settings/GeneralItemSettings");
            playerLevelSettings = (PlayerLevelSettings)Resources.Load("Settings/PlayerLevelSettings");

            generationSettingsT1 = (AreaGenerationSettings)Resources.Load("Settings/AreaGenerationSettings_Tier1");
            generationSettingsT2 = (AreaGenerationSettings)Resources.Load("Settings/AreaGenerationSettings_Tier2");
        }
    }
}