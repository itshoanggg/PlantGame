using UnityEngine;
using System.Collections;

public class DayNightFade : MonoBehaviour
{
    public SpriteRenderer daySprite;
    public SpriteRenderer nightSprite;

    public float fadeDuration = 1.5f;

    Coroutine fadeCoroutine;

    void Start()
    {
        // Bắt đầu là ban ngày
        SetInstantDay();
    }

    public void FadeToDay()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(1, 0));
    }

    public void FadeToNight()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(0, 1));
    }

    IEnumerator Fade(float dayAlphaTarget, float nightAlphaTarget)
    {
        float time = 0;

        float startDayAlpha = daySprite.color.a;
        float startNightAlpha = nightSprite.color.a;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            float newDayAlpha = Mathf.Lerp(startDayAlpha, dayAlphaTarget, t);
            float newNightAlpha = Mathf.Lerp(startNightAlpha, nightAlphaTarget, t);

            daySprite.color = new Color(1, 1, 1, newDayAlpha);
            nightSprite.color = new Color(1, 1, 1, newNightAlpha);

            yield return null;
        }
    }

    void SetInstantDay()
    {
        daySprite.color = new Color(1, 1, 1, 1);
        nightSprite.color = new Color(1, 1, 1, 0);
    }
}