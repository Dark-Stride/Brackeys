using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CraterGlow : MonoBehaviour
{
    public Image glowImage;

    public float pulseDuration = 0.8f;
    public float maxAlpha = 0.8f;

    void Start()
    {
        StartCoroutine(PulseGlow());
    }

    IEnumerator PulseGlow()
    {
        while (true)
        {
            // Fade In
            float timer = 0f;

            while (timer < pulseDuration)
            {
                timer += Time.deltaTime;

                Color c = glowImage.color;
                c.a = Mathf.Lerp(0f, maxAlpha, timer / pulseDuration);
                glowImage.color = c;

                yield return null;
            }

            // Fade Out
            timer = 0f;

            while (timer < pulseDuration)
            {
                timer += Time.deltaTime;

                Color c = glowImage.color;
                c.a = Mathf.Lerp(maxAlpha, 0f, timer / pulseDuration);
                glowImage.color = c;

                yield return null;
            }
        }
    }
}