using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace AsteroidAnnihilation
{
    public class UpgradePanel : MonoBehaviour
    {
        //Old stuff might recycle/use later
        /*
        public static UpgradePanel Instance;

        //References
        private GameManager gameManager;
        private PlayerAttack playerAttack;
        private MenuOpener menuOpener;

        [SerializeField] private GameObject statUIObject;
        [SerializeField] private Transform statPanel;
        [SerializeField] private TextMeshProUGUI weaponText;

        //Private Variables
        private Dictionary<string, UpgradableStat> currentWeaponStats;

        private string lastWeaponName;

        private JSONWeaponDatabank weaponDatabank;

        private void Awake()
        {
            Instance = this;
            menuOpener = GetComponent<MenuOpener>();
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            playerAttack = gameManager.RPlayer.RPlayerAttack;
            weaponDatabank = JSONWeaponDatabank.Instance;
            GetCurrentWeapon();
            SetupUpgradeUI();
        }

        public void UpgradeStat(EnumCollections.WeaponStats stat)
        {
            currentWeaponStats[stat].Level++;
            int level = currentWeaponStats[stat].Level;
            currentWeaponStats[stat].Value = weaponDatabank.LookUpStat(lastWeaponName, stat).Values[level];
            playerAttack.SetCurrentWeaponStats(currentWeaponStats);
        }

        public void DecreaseStat(string name)
        {
            int level = currentWeaponStats[name].Level--;

            currentWeaponStats[name].Value = weaponDatabank.LookUpStat(lastWeaponName, name).Values[level];
            playerAttack.SetCurrentWeaponStats(currentWeaponStats);
        }

        public void GetCurrentWeapon()
        {
            Weapon currentWeapon = playerAttack.GetCurrentWeapon();
            currentWeaponStats = currentWeapon.WeaponStatDictionary;
            lastWeaponName = currentWeapon.WeaponName;
        }

        public void SetupUpgradeUI()
        {
            //Remove all previous stats from panel
            for(int i = 0; i < statPanel.childCount; i++)
            {
                if(statPanel.GetChild(i).GetComponent<LayoutElement>() == null)
                {
                    Destroy(statPanel.GetChild(i).gameObject);
                }
            }

            weaponText.text = lastWeaponName;

            //Add stats to panel
            foreach (UpgradableStat stat in currentWeaponStats.Values)
            {
                if (!stat.Upgradable) 
                {
                    continue;
                }
                GameObject statUI = Instantiate(statUIObject, statPanel);
                statUI.GetComponent<StatUIScript>().SetupStatUI(this, gameManager.RPlayer.RPlayerStats, weaponDatabank , lastWeaponName ,stat);
            }
        }*/
    }
}

