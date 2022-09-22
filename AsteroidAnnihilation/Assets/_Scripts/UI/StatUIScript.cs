using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace AsteroidAnnihilation
{
    public class StatUIScript : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
    {
        /*
        private UpgradePanel upgradePanel;
        private PlayerStats playerStats;

        private string weaponName;
        private string statName;

        private float currentValue;

        private float cost;

        private int level;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI valueText;

        [SerializeField] private GameObject tooltip;
        [SerializeField] private TextMeshProUGUI costText;

        [SerializeField] private GameObject minButton;

        [SerializeField] private GameObject lockPanel;

        private JSONWeaponDatabank weaponDatabank;

        public void SetupStatUI(UpgradePanel uPanel, PlayerStats pStats, JSONWeaponDatabank databank, string _weaponName ,UpgradableStat stat)
        {
            //Set references
            upgradePanel = uPanel;
            playerStats = pStats;
            weaponDatabank = databank;

            //Set variable values
            statName = stat.StatName;
            level = stat.Level;
            weaponName = _weaponName;
            currentValue = GetStatValue();

            //Setup text elements
            nameText.text = statName;
            valueText.text = "" + currentValue;
            SetStatCost();

            //Lock weapon in case needed;
            lockPanel.SetActive(!stat.Unlocked);

            //Can you decrease stat?
            if(!stat.HasNegative)
            {
                minButton.SetActive(false);
            }
        }

        public void UpgradeStat()
        {
            if(playerStats.TryPlayerBuy(cost))
            {
                upgradePanel.UpgradeStat(statName);

                level++;
                currentValue = GetStatValue();

                valueText.text = "" + currentValue;

                SetStatCost();
            }
        }

        public void DecreaseStat()
        {
            upgradePanel.DecreaseStat(statName);

            level--;
            currentValue = GetStatValue();

            valueText.text = "" + currentValue;
        }

        private void SetStatCost()
        {
            cost = weaponDatabank.LookUpStat(weaponName, statName).UpgradeCost[level];

            costText.text = "Cost\n" + cost;
        }


        private float GetStatValue()
        {
            return weaponDatabank.LookUpStat(weaponName, statName).Values[level] + CompletionRewardStats.Instance.GetRewardedStat(statName, weaponName);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.SetActive(true);
            costText.text = "Cost\n" + cost;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.SetActive(false);
        }
        */
    }
}