using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public Slider loadingBar;

    public Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync("World");
        op.allowSceneActivation = false;
        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, timer);
                loadingText.text = (loadingBar.value * 100f).ToString() + "%";
                if (loadingBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);
                loadingText.text = (loadingBar.value * 100f).ToString() + "%";
                if (loadingBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
