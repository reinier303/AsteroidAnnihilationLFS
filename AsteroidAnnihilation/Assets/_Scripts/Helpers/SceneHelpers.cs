using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AsteroidAnnihilation
{
    public class SceneHelpers : MonoBehaviour
    {
        public static AsyncOperation sceneLoadOperation;

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
