using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class EquipmentTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemType;
        [SerializeField] private TextMeshProUGUI energyPerShot;

        [SerializeField] private GameObject statDisplayPrefab;

        public void ShowTooltip(ItemData item)
        {
            SetItemProperties(item);
        }

        public void ShowTooltip(EquipmentData equipment)
        {
            SetItemProperties(equipment.ItemData);
            SetStats(equipment);
        }

        public void ShowTooltip(WeaponData weapon)
        {
            SetItemProperties(weapon.EquipmentData.ItemData);
            SetStats(weapon.EquipmentData);
            energyPerShot.text = EnumCollections.EquipmentStats.EnergyPerShot.ToString() + ": " + weapon.EquipmentData.EquipmentStats[EnumCollections.EquipmentStats.EnergyPerShot];
        }

        private void SetItemProperties(ItemData item)
        {
            itemName.text = item.ItemName;
            itemType.text = item.Rarity.ToString() + " " + item.ItemType.ToString();
        }

        private void SetStats(EquipmentData equipmentData)
        {
            //TODO::Make this use ObjectPooler
            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.GetComponent<LayoutElement>() == null) { Destroy(child.gameObject); }
            }
            Dictionary<EnumCollections.EquipmentStats, float> stats = equipmentData.EquipmentStats;
            foreach (EnumCollections.EquipmentStats stat in stats.Keys)
            {
                if (stat != EnumCollections.EquipmentStats.EnergyPerShot)
                {
                    //TODO::Link to ObjectPooler
                    TextMeshProUGUI statDisplay = Instantiate(statDisplayPrefab, transform).GetComponent<TextMeshProUGUI>();
                    if (stat == EnumCollections.EquipmentStats.Damage) 
                    {
                        float minValue = Mathf.RoundToInt(stats[stat] * 0.75f);
                        float maxValue = Mathf.RoundToInt(stats[stat] * 1.25f);

                        statDisplay.text = stat.ToString() + ": " + minValue + "-" + maxValue; 
                    } 
                    else { statDisplay.text = stat.ToString() + ": " + stats[stat]; }
                }
            }

            TextMeshProUGUI spacer = Instantiate(statDisplayPrefab, transform).GetComponent<TextMeshProUGUI>();
            spacer.text = "";

            Dictionary<EnumCollections.EquipmentStats, float> rarityStats = equipmentData.RarityStats;
            foreach (EnumCollections.EquipmentStats stat in rarityStats.Keys)
            {
                if (stat != EnumCollections.EquipmentStats.EnergyPerShot)
                {
                    //TODO::Link to ObjectPooler
                    TextMeshProUGUI statDisplay = Instantiate(statDisplayPrefab, transform).GetComponent<TextMeshProUGUI>();
                    if (stat == EnumCollections.EquipmentStats.Damage)
                    {
                        float minValue = Mathf.RoundToInt(rarityStats[stat] * 0.75f);
                        float maxValue = Mathf.RoundToInt(rarityStats[stat] * 1.25f);

                        statDisplay.text = stat.ToString() + ": " + minValue + "-" + maxValue;
                    }
                    else { statDisplay.text = stat.ToString() + ": " + rarityStats[stat]; }
                }
            }
        }
    }
}
