using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class NPCBase : MonoBehaviour
    {
        protected InputManager inputManager;
        protected UIManager uiManager;

        [SerializeField] protected GameObject menuToOpen;
        [SerializeField] protected GameObject outline;
        protected bool inRange;
        protected Vector2 startPosition;

        protected virtual void Start()
        {
            inputManager = InputManager.Instance;
            uiManager = UIManager.Instance;
            startPosition = transform.position;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                outline.SetActive(true);
                inRange = true;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                outline.SetActive(false);
                if (menuToOpen != null) 
                {
                    if (!uiManager.LoadingScreen.activeSelf) { inputManager.InputEnabled = true; }
                    menuToOpen.SetActive(false); 
                }
                inRange = false;
            }
        }

        protected virtual void Update()
        {
            if (inRange && Input.GetKeyDown(KeyCode.F))
            {
                //Open Menu
                menuToOpen.gameObject.SetActive(true);
                inputManager.InputEnabled = false;
            }
        }

        protected virtual void MoveBackToOriginalPosition()
        {
            if ((Vector2)transform.position != startPosition)
            {
                transform.LeanMove(startPosition, 2f);

            }
        }
    }
}
