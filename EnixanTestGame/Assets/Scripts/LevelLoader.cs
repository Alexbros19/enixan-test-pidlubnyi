using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Slider slider;

    private void Start()
    {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        // start to load level into background
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        // activate loading screen object
        loadingScreen.SetActive(true);
        // not switch to new level 
        operation.allowSceneActivation = false;

        while (operation.isDone == false)
        {
            // using progress to move slider 
            slider.value = operation.progress;
            // when level is fully allowed progress is 0.9f
            if (operation.progress == 0.9f)
            {
                slider.value = 1f;
                // allow to switch to new level
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
