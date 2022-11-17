using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class SwarmSeekerDrone : BaseEnemy
    {
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float chargeMultiplier = 1.75f;
        [SerializeField] protected float chargeDistance = 10;
        private Vector2 baseScale;
        private bool animating;
        private bool canCharge;
        private bool charging;

        protected override void Start()
        {
            base.Start();
            float randomScale = Random.Range(sizeRange.x, sizeRange.y);
            Vector2 randomSize = new Vector2(randomScale, randomScale);
            transform.localScale = randomSize;
            baseScale = transform.localScale;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            canCharge = true;
            charging = false;
        }

        protected virtual void Update()
        {
            if(Vector2.Distance(transform.position, Player.position) < chargeDistance && canCharge)
            {
                StartCharge();
            }
            else if(!charging)
            {
                Move();
            }
        }

        protected virtual void Move()
        {
            if(animating)
            {
                transform.localScale = baseScale;
                animating = false;
            }
            transform.position += transform.up * Time.deltaTime * moveSpeed;
            Rotate();
        }


        protected virtual void StartCharge()
        {
            if (!animating)
            {
                StartCoroutine(Charge());
                transform.localScale = new Vector2(transform.localScale.x * 0.7f, transform.localScale.y * 1.3f);
                animating = true;
            }
        }

        private IEnumerator Charge()
        {
            charging = true;
            canCharge = false;
            float timer = 0;
            while(timer < 1f)
            {
                transform.position += transform.up * Time.deltaTime * moveSpeed * chargeMultiplier;
                Rotate(rotSpeedMultiplier: 0.35f);
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }
            charging = false;
            yield return new WaitForSeconds(4.5f);
            canCharge = true;
        }
    }
}
