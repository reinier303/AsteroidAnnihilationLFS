using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AsteroidAnnihilation
{
    public static class ExtensionMethods
    {
        public static AsyncOperation sceneLoadOperation;

        public static void LoadSceneWithLoadingScreen(int scene, GameObject loadingScreen, MonoBehaviour runOn)
        {
            runOn.StartCoroutine(LoadSceneIENumerator(scene, loadingScreen));
        }

        public static IEnumerator LoadSceneIENumerator(int scene, GameObject loadingScreen)
        {
            loadingScreen.SetActive(true);

            yield return new WaitForSecondsRealtime(3f);

            sceneLoadOperation = SceneManager.LoadSceneAsync(scene);

            while (!sceneLoadOperation.isDone)
            {
                yield return null;
            }
        }
    }
}

