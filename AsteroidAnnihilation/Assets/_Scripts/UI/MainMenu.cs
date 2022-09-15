using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidAnnihilation
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;

        private void Awake()
        {
            Time.timeScale = 1;
        }

        public void LoadScene(int scene)
        {
            ExtensionMethods.LoadSceneWithLoadingScreen(scene, loadingScreen, this);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}


