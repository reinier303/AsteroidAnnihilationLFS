using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class NPC_Missions : NPCBase
    {
        private MissionManager missionManager;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            missionManager = MissionManager.Instance;
        }

        protected override void Update()
        {
            if (inRange && Input.GetKeyDown(KeyCode.F))
            {
                //Open Menu
                menuToOpen.gameObject.SetActive(true);
                inputManager.InputEnabled = false;
                missionManager.ShowMissionCards();
            }
        }
    }
}