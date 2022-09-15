using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidAnnihilation
{
    public class Powerup : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                GameManager.Instance.StartCoroutine(ApplyPowerup());
                gameObject.SetActive(false);
            }        
        }

        public virtual IEnumerator ApplyPowerup()
        {
            //This method is meant to be overridden
            yield break;
        }
    }
}
    