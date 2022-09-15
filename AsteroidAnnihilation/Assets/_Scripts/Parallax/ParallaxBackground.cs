using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AsteroidAnnihilation
{
    public class ParallaxBackground : SerializedMonoBehaviour
    {
        private Transform player;

        private Vector2 size;

        private Vector2 parallaxNumber;

        private float zValue;

        public float ParallaxMoveOffsetMultiplier = 3;

        [DictionaryDrawerSettings(KeyLabel ="Keys", ValueLabel ="Values")] 
        [SerializeField] private Dictionary<Transform, Vector2> BackgroundElements;

        public bool GetSpritesFromResources;
        public List<Sprite> BackgroundSprites;

        private float parallaxMoveOffset;

        //private List<Vector2> backgroundGrid;

        private void Awake()
        {
            BackgroundElements = new Dictionary<Transform, Vector2>();

            SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            zValue = transform.GetChild(0).localPosition.z;

            size = spriteRenderer.bounds.size;

            parallaxMoveOffset = size.x / ParallaxMoveOffsetMultiplier;
        }

        private void Start()
        {
            player = GameManager.Instance.RPlayer.transform;
            GetBackgroundSprites();
            SetBackgroundsStart();
            StartCoroutine(CheckParallax());
        }

        private void GetBackgroundSprites()
        {
            //TODO::Make background colors work with new mission system
            if (GetSpritesFromResources) { BackgroundSprites = AreaManager.Instance.GetCurrentBackgrounds(); }

            for (int i = 0; i < transform.childCount; i++)
            {
                RandomSprite backgroundRandom = transform.GetChild(i).GetComponent<RandomSprite>();
                if (backgroundRandom != null)
                {
                    BackgroundElements.Add(transform.GetChild(i), new Vector2(0, i * 10));
                    backgroundRandom.SetSprites(BackgroundSprites);
                    continue;
                }
            }
        }

        private void SetBackgroundsStart()
        {
            Vector3 position = new Vector3(size.x * (parallaxNumber.x + 1), size.x * parallaxNumber.y, zValue);
            Vector2 gridPosition = new Vector2(parallaxNumber.x + 1, parallaxNumber.y);
            SpawnBackground(position, gridPosition, true);
            position = new Vector3(size.x * (parallaxNumber.x - 1), size.x * parallaxNumber.y, zValue);
            gridPosition = new Vector2(parallaxNumber.x - 1, parallaxNumber.y);
            SpawnBackground(position, gridPosition, true);
            position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y + 1), zValue);
            gridPosition = new Vector2(parallaxNumber.x, parallaxNumber.y + 1);
            SpawnBackground(position, gridPosition, false);
            position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y - 1), zValue);
            gridPosition = new Vector2(parallaxNumber.x, parallaxNumber.y - 1);
            SpawnBackground(position, gridPosition, false);
        }

        private IEnumerator CheckParallax()
        {
            parallaxNumber = CalculateParallaxNumber();
            if (player.transform.position.x >= (size.x * (parallaxNumber.x + 1)) - (size.x / 2) - parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * (parallaxNumber.x + 1), size.x * parallaxNumber.y, zValue);
                Vector2 gridPosition = new Vector2(parallaxNumber.x + 1, parallaxNumber.y);
                SpawnBackground(position, gridPosition, true);
            }
            if (player.transform.position.x <= (size.x * (parallaxNumber.x - 1)) + (size.x / 2) + parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * (parallaxNumber.x - 1), size.x * parallaxNumber.y, zValue);
                Vector2 gridPosition = new Vector2(parallaxNumber.x - 1, parallaxNumber.y);
                SpawnBackground(position, gridPosition , true);
            }

            if (player.transform.position.y >= (size.y * (parallaxNumber.y + 1)) - (size.y / 2) - parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y + 1), zValue);
                Vector2 gridPosition = new Vector2(parallaxNumber.x, parallaxNumber.y + 1);
                SpawnBackground(position, gridPosition, false);
            }
            if (player.transform.position.y <= (size.y * (parallaxNumber.y - 1)) + (size.y / 2) + parallaxMoveOffset)
            {
                Vector3 position = new Vector3(size.x * parallaxNumber.x, size.y * (parallaxNumber.y - 1), zValue);
                Vector2 gridPosition = new Vector2(parallaxNumber.x, parallaxNumber.y - 1);
                SpawnBackground(position, gridPosition, false);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CheckParallax());
        }

        //Spawns 3 for diagonals
        private void SpawnBackground(Vector3 position, Vector2 gridPosition, bool x)
        {
            Vector2 startGridPos = gridPosition;
            for(int i = -1; i < 2; i++)
            {
                if (x)
                {
                    gridPosition = startGridPos + new Vector2(0, i);
                }
                else
                {
                    gridPosition = startGridPos + new Vector2(i, 0);
                }
                if (BackgroundElements.ContainsValue(gridPosition))
                {
                    continue;
                }
                Transform background = GetAvailableBackground();

                background.gameObject.SetActive(true);

                if(x)
                {
                    background.localPosition = position + new Vector3(0, size.y * i, 0);
                    BackgroundElements[background] = gridPosition;
                }
                else
                {
                    background.localPosition = position + new Vector3(size.x * i, 0, 0);
                    BackgroundElements[background] = gridPosition;
                }
            }
        }

        private Vector2 CalculateParallaxNumber()
        {
            int x = 0;
            int y = 0;
            if(player.transform.position.x > 0)
            {
                x = Mathf.FloorToInt((player.transform.position.x + (size.x /2)) / (size.x));
            }
            else
            {
                x = Mathf.CeilToInt((player.transform.position.x - (size.x / 2)) / (size.x));
            }
            if (player.transform.position.y > 0)
            {
                y = Mathf.FloorToInt((player.transform.position.y + (size.y / 2)) / (size.y));
            }
            else
            {
                y = Mathf.CeilToInt((player.transform.position.y - (size.y / 2)) / (size.y));
            }
            return new Vector2(x, y);
        }

        private Transform GetAvailableBackground()
        {
            foreach(Transform background in BackgroundElements.Keys)
            {
                if (Vector2.Distance(background.localPosition, player.position) >= size.x * 1.5f)
                {
                    return background;
                }
            }
            Debug.LogWarning("No available backgrounds found!, returning first child");
            return transform.GetChild(0);
        }
    }
}
