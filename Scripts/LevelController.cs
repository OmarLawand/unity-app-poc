using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject loadingScreen;
    private string sceneToLoad;
    private CanvasGroup canvasGroup;

    public static LevelController instance;

    public void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        canvasGroup = loadingScreen.GetComponent<CanvasGroup>();
    }


    public void SetScene(string sceneName)
    {
        sceneToLoad = sceneName;

        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(StartLoad());
    }

    IEnumerator StartLoad()
    {
        yield return StartCoroutine(FadeLoadingScreen(1, 1));

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}
