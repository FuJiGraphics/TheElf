
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    static string nextScene;

    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Slider progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadScene");
    }

    private void Start()
    {
        StartCoroutine(this.LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            textUI.text = $"Loading {op.progress * 100f}%";
            if (op.progress < 0.9f)
            {
                progressBar.value = op.progress * 100f;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0f, 1f, timer) * 100f;
                textUI.text = $"Loading {progressBar.value}%";
                if (progressBar.value >= 100f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

} // class LoadSceneManager