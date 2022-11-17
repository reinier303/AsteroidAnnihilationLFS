using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Unit : MonoBehaviour
    {
        //Variables
        public float Value;
        public float DisableTime;

        //Components
        private Transform player;

        //Script References
        private Player playerScript;
        private PlayerStats playerStats;
        private ObjectPooler objectPooler;
        private UIManager uIManager;

        private float magnetRadius;

        protected SpriteRenderer spriteRenderer;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start()
        {
            playerScript = GameManager.Instance.RPlayer;
            playerStats = playerScript.RPlayerStats;
            player = playerScript.transform;

            objectPooler = ObjectPooler.Instance;
            uIManager = UIManager.Instance;
            magnetRadius = playerScript.RPlayerStats.GetStatValue(EnumCollections.PlayerStats.BaseMagnetRadius);
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(DisableAfterTime());
        }

        protected virtual void Update()
        {
            float playerDistance = Vector2.Distance(transform.position, player.position);
            if (playerDistance < magnetRadius)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * Mathf.Clamp(magnetRadius / (playerDistance / 2f), 1, 50));
            }
        }

        public void MoveUnit()
        {
            float PowerX = Random.Range(-1.5f, 1.5f);
            float PowerY = Random.Range(-1.5f, 1.5f);
            float Time = Mathf.Abs(0.35f * ((Mathf.Abs(PowerX) + Mathf.Abs(PowerY)) / 2));

            StartCoroutine(lerpPosition(transform.position, (Vector2)transform.position + new Vector2(PowerX, PowerY), Time));
        }

        private IEnumerator lerpPosition(Vector2 StartPos, Vector2 EndPos, float LerpTime)
        {
            float StartTime = Time.time;
            float EndTime = StartTime + LerpTime;

            while (Time.time < EndTime)
            {
                float timeProgressed = (Time.time - StartTime) / LerpTime;  // this will be 0 at the beginning and 1 at the end.
                transform.position = Vector2.Lerp(StartPos, EndPos, timeProgressed);

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator DisableAfterTime()
        {
            float DisableTimeFinal = DisableTime + Random.Range(0, 1f);
            yield return new WaitForSeconds(DisableTimeFinal);
            //LeanTween.value(gameObject, setSpriteAlpha, 1, 0, DisableTimeFinal);
            gameObject.SetActive(false);
        }

        public void setSpriteAlpha(float val)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, val);
        }

        // Update is called once per frame
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                objectPooler.SpawnFromPool("CoinPickupEffect", transform.position, Quaternion.identity);
                playerStats.AddToUnits(Value);
                gameObject.SetActive(false);
            }
        }
    }
}