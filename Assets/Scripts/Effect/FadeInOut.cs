using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public float playTime = 1f;
    public Image background;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (background == null)
        {
            background = GetComponent<Image>();
            if (background == null)
            {
                Debug.LogError("FadeInOut: Image 컴포넌트가 없습니다!");
                return;
            }
        }

        FadeIn();
    }

    public void FadeIn()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsed = 0f;
        Color newColor = background.color;
        newColor.a = 0f;
        background.color = newColor;

        while (elapsed < playTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / playTime);
            newColor.a = Mathf.Lerp(0f, 1f, t);
            background.color = newColor;
            yield return null;
        }

        newColor.a = 1f;
        background.color = newColor;
        fadeCoroutine = null;
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsed = 0f;
        Color newColor = background.color;
        newColor.a = 1f;
        background.color = newColor;

        while (elapsed < playTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / playTime);
            newColor.a = Mathf.Lerp(1f, 0f, t);
            background.color = newColor;
            yield return null;
        }

        newColor.a = 0f;
        background.color = newColor;
        fadeCoroutine = null;
        gameObject.SetActive(false);
    }
}
