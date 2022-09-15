using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace AsteroidAnnihilation
{
    public class ShopItemUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private PlayerStats playerStats;
        private ShopItemSO shopItem;

        [SerializeField] private Image icon;

        [SerializeField] private GameObject tooltip;

        [SerializeField] private TextMeshProUGUI TooltipCost;
        [SerializeField] private TextMeshProUGUI TooltipName;
        [SerializeField] private TextMeshProUGUI TooltipDescription;

        public void Initialize(ShopItemSO item)
        {
            shopItem = item;
            playerStats = GameManager.Instance.RPlayer.RPlayerStats;
            icon.sprite = shopItem.sprite;
            InitializeTooltip();
        }

        private void InitializeTooltip()
        {
            TooltipCost.text = "$" + shopItem.Cost;
            TooltipName.text = shopItem.ItemName;
            TooltipDescription.text = shopItem.Description;

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            BuyItem();
        }

        private void BuyItem()
        {
            if(playerStats.TryPlayerBuy(shopItem.Cost))
            {
                shopItem.ItemBought();
                //TODO::remove from available items in shop save file.
                gameObject.SetActive(false);
            }
            else
            {
                //TODO::Can't buy sound
                Debug.Log("cannot buy");
            }
        }
    }
}
