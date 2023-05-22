using UnityEngine;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour
{

    [SerializeField] float fadeDuration = 4f;
    [SerializeField] Image image;

    float fadeSpeed;
    bool fading;

    void Start()
    {
        fadeSpeed = 1f / fadeDuration;
        fading = false;

        SetImageAlpha(0f);
    }

    void Update()
    {
        if (fading)
        {
            StartCoroutine(FadeInImage());
        }
    }

    void OnEnable()
    {
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        fading = true;
    }

    System.Collections.IEnumerator FadeInImage()
    {
        fading = false;

        float targetAlpha = 1f;
        float currentAlpha = GetImageAlpha();
        float timer = 0f;

        while (currentAlpha < targetAlpha)
        {
            timer += Time.deltaTime;
            currentAlpha = Mathf.Lerp(0f, targetAlpha, timer / fadeDuration);

            SetImageAlpha(currentAlpha);

            yield return null;
        }
        Time.timeScale = 0;
    }

    float GetImageAlpha()
    {
        return image.color.a;
    }

    void SetImageAlpha(float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}