using System.Collections;
using UnityEngine;

public class MoonDrop : MonoBehaviour
{
    public RectTransform moon;  // Reference to the moon RectTransform, rectTransform is used for UI elements for positioning
    public CanvasGroup moonCanvas;
    public float dropDistance = 350f;
    public float duration = 1.2f;
    private Vector2 endPosition;
    private Vector2 startPosition;
    public DSText dsText;

    void Start()
    {
        Debug.Log("Moon Drop Started");

        endPosition = moon.anchoredPosition; // Store the original position of the moon

        startPosition = endPosition + Vector2.up * dropDistance; // Calculate the starting position above the end position

        moon.anchoredPosition = startPosition;

        moonCanvas.alpha = 0f;

        StartCoroutine(DropMoon());
    }

    IEnumerator DropMoon()
    {
        float timer = 0f;  // Initialize a timer to track the duration of the drop

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            // Ease Out
            t = 1 - Mathf.Pow(1 - t, 3);  //used for a smoother drop effect

            moon.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            moonCanvas.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        moon.anchoredPosition = endPosition;
        moonCanvas.alpha = 1f;

        // Small bounce
        yield return StartCoroutine(BounceMoon());
    }

    IEnumerator BounceMoon()
    {
        Vector2 bounceUp = endPosition + Vector2.up * 20f;

        float timer = 0f;

        // Bounces Up
        while (timer < 0.15f)
        {
            timer += Time.deltaTime;

            moon.anchoredPosition =
                Vector2.Lerp(endPosition, bounceUp, timer / 0.15f);

            yield return null;
        }

        timer = 0f;

        // Bounces Down
        while (timer < 0.20f)
        {
            timer += Time.deltaTime;

            moon.anchoredPosition =
                Vector2.Lerp(bounceUp, endPosition, timer / 0.20f);

            yield return null;
        }

        moon.anchoredPosition = endPosition;
        
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(dsText.Reveal());
    }
}