using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class TutorialWall : MonoBehaviour
    {
        private Transform player;
        private UIManager uiManager;

        private void Start()
        {
            uiManager = UIManager.Instance;
            player = Player.Instance.transform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            player.transform.position = Vector3.zero;
            uiManager.EnableStayInsideMessage();
        }
    }

}