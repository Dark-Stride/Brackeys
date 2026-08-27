using System.Collections;
using UnityEngine;

public class DSText : MonoBehaviour
{
    public RectTransform textRect;
    public CanvasGroup canvasGroup;
    public float riseDistance = 50f;
    public float duration = 1f;

    Vector2 endPos;
    Vector2 startPos;

    void Start()
    {
        endPos = textRect.anchoredPosition;
        startPos = endPos - Vector2.up * riseDistance;

        textRect.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;
    }

    public IEnumerator Reveal()  // Coroutine to reveal the text
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            t = 1 - Mathf.Pow(1 - t, 3);

            textRect.anchoredPosition =
                Vector2.Lerp(startPos, endPos, t);

            canvasGroup.alpha =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        textRect.anchoredPosition = endPos;
        canvasGroup.alpha = 1f;
    }
}
