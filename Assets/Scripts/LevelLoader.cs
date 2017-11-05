using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour 
{
    public Slider loadingBar;
    public Text percentageText;
    public GameObject loadingScreenUiElement;

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    IEnumerator LoadLevelAsync(string sceneName)
    {
        var loadingDetails = SceneManager.LoadSceneAsync(sceneName);
        loadingScreenUiElement.SetActive(true);

        while (!loadingDetails.isDone)
        {
            UpdateLoadingUi(loadingDetails.progress / 0.9f);

            yield return null;
        }
    }

    void UpdateLoadingUi(float percentage)
    {
        if (loadingBar != null)
        {
            loadingBar.value = percentage;
        }

        if (percentageText != null)
        {
            percentageText.text = Mathf.Round(percentage * 100)  + "%";
        }
    }
}
