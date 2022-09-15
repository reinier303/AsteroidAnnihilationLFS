using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class AreaMenu : MonoBehaviour
    {
        private AreaManager areaManager;
        [SerializeField] private AreaTooltip tooltip;

        // Start is called before the first frame update
        void Start()
        {
            areaManager = AreaManager.Instance;
            PopulateAreaMenu();
        }

        private void PopulateAreaMenu()
        {          
            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.GetComponent<LayoutElement>())
                {
                    continue;
                }
                child.gameObject.SetActive(false);
            }

            List<AreaData> areaDatas = areaManager.GetTierDatas(areaManager.CurrentTier);

            //Set area button size !!!BASED ON SQUARE MENU!!!
            Vector2 menuSizeWithPadding = GetComponent<RectTransform>().sizeDelta - new Vector2(20, 20);
            GetComponent<GridLayoutGroup>().cellSize = menuSizeWithPadding / Mathf.Sqrt(areaDatas.Count);

            for (int i = 0; i < areaDatas.Count; i++)
            {
                Transform currentChild = transform.GetChild(i);
                currentChild.gameObject.SetActive(true);
                AreaUIButton areaButton = currentChild.GetComponent<AreaUIButton>();
                areaButton.area = new Vector2Int(areaManager.CurrentTier, i);
                areaButton.Tooltip = tooltip;
            }
        }
    }
}
