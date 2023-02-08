using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class Asteroid : BaseEnemy
    {
        public float MaxRotationSpeed;
        public float MinForce;
        public float MaxForce;
        public Vector2 MinMaxSize;

        protected Rigidbody2D rb;
        private Rotator rotator;

        protected Vector2 maxXandY;

        private bool freezing;

        [SerializeField] private GameObject TrailEffects;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
            rotator = GetComponentInChildren<Rotator>();
        }

        protected override void Start()
        {
            base.Start();
            //maxXandY = CameraOffset.Instance.ScreenEdgeValues + new Vector2(19f,11f);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            maxXandY = CameraOffset.Instance.ScreenEdgeValues + new Vector2(19f, 16f);

            //StartCoroutine(CheckScreenEdges(0.2f));

            AddForces();
            RandomSize();
            transform.localScale = RandomSize();
            freezing = false;
        }

        protected override IEnumerator CheckDistanceToPlayer(float time)
        {
            if (Vector2.Distance(transform.position, Player.position) > 250f)
            {
                EjectTrailEffects();
                gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(time);
            StartCoroutine(CheckDistanceToPlayer(time));
        }

        public override void TakeDamage(float damage, bool isCrit)
        {
            base.TakeDamage(damage, isCrit);
            if(!isDead && !freezing)
            {
                StartCoroutine(Freeze());
            }
        }

        //Use for confined screen
        protected virtual IEnumerator CheckScreenEdges(float time)
        {
            if (transform.position.x > maxXandY.x ||
           transform.position.y > maxXandY.y ||
           transform.position.x < -maxXandY.x ||
           transform.position.y < -maxXandY.y)
            {
                EjectTrailEffects();
                gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(time);
            StartCoroutine(CheckScreenEdges(time));

        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerEntity player = collision.GetComponent<PlayerEntity>();

            if (player != null)
            {
                player.OnTakeDamage?.Invoke(ContactDamage, false);
                //Die();
            }
        }

        protected virtual void AddForces()
        {
            Vector2 force = CalculateForce();

            rb.AddForce(force, ForceMode2D.Impulse);
            rotator.SetRotationSpeed(CalculateTorque());
        }

        protected virtual Vector2 CalculateForce()
        {
            Vector2 force = new Vector2(Random.Range(-MaxForce, MaxForce), Random.Range(-MaxForce, MaxForce));

            //Ugly if tree to make sure asteroids don't go too slow.
            if (force.x > 0 && force.x < MinForce)
            {
                force.x = MinForce;
            }
            else if (force.x < 0 && force.x > -MinForce)
            {
                force.x = -MinForce;
            }
            if (force.y > 0 && force.y < MinForce)
            {
                force.y = MinForce;
            }
            else if (force.y < 0 && force.y > -MinForce)
            {
                force.y = -MinForce;
            }
            //ew...

            return force;
        }

        protected virtual float CalculateTorque()
        {
            return Random.Range(-MaxRotationSpeed, MaxRotationSpeed);
        }

        protected virtual Vector2 RandomSize()
        {
            float randomSize = Random.Range(MinMaxSize.x, MinMaxSize.y);

            return new Vector2(randomSize, randomSize);
        }

        protected override void SpawnSegments()
        {
            for (int i = 0; i < Random.Range(PermanencePartMinMaxAmount.x, PermanencePartMinMaxAmount.y); i++)
            {
                GameObject permanencePart = objectPooler.SpawnFromPool("PermanencePart", transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                permanencePart.transform.localScale = RandomSize();
                permanencePart.GetComponent<PermanencePart>().InitializePart(permanenceSpriteList, PermanencePartOutwardsPower, PermanenceScaleFactor * Random.Range(0.65f, 1.15f));
            }
        }

        protected override void SpawnParticleEffect()
        {
            GameObject effect = objectPooler.SpawnFromPool(particleEffectName, transform.position, Quaternion.identity);
            float scaleFactor2 = (Mathf.Abs(transform.localScale.x - transform.localScale.y) / 2) + Mathf.Min(transform.localScale.x, transform.localScale.y);
            effect.transform.localScale *= ParticleEffectScale * scaleFactor2;
        }

        protected virtual IEnumerator Freeze()
        {
            freezing = true;

            //Save values
            Vector2 velocity = rb.velocity;
            float rotationSpeed = rotator.GetRotationSpeed();

            //Set values to 0 and wait
            rb.velocity = Vector2.zero;
            rotator.SetRotationSpeed(0);
            yield return new WaitForSeconds(0.03f);

            //Return values
            rb.velocity = velocity;
            rotator.SetRotationSpeed(rotationSpeed);

            freezing = false;
        }

        protected void EjectTrailEffects()
        {
            TrailEffects.transform.parent = gameManager.EjectedTrailEffects;

            TrailEffect effectScript = TrailEffects.GetComponent<TrailEffect>();
            effectScript.StartCoroutine(effectScript.ReturnToParentAfterTime(transform));
        }

        protected override void Die()
        {
            EjectTrailEffects();
            base.Die();
        }
    }
}