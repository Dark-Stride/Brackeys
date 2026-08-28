using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupMod : MonoBehaviour
{
    public CanvasGroup canvasGroup; //reference to the Canvas Group that you'd want to transition from and to.
    public float fadeDuration = 2f; //can set to any float you would want. This is to tell how long the duration of the fade.
    private Coroutine fadeCoroutine; //This is to ensure that we can stop the coroutine if it is already running.
    
    [Header("Fade Settings")]  
    public bool fadeOnStart = true;
    public bool startInvisible = true;
    public bool disableInteractionWhileFading = true;

    void Awake()
    {
        canvasGroup.alpha = 0f; 
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        ToggleFade(true); //Change the structure a little so that it can accommodate the fade in/out portion based on bool
    }

    public void ToggleFade(bool fade)
    {
        //First we want to make sure that any other coroutines are not working first. To ensure it doesnt overlap!
        if (fadeCoroutine!= null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(fade ? FadeIn() : FadeOut());
    }

    public void FadeInMenu()
    {
        ToggleFade(true);
    }

    public void FadeOutMenu()
    {
        ToggleFade(false);
    }

    IEnumerator FadeIn()
    {
        float timer = 0f; //An internal timer to decide the time between the fading transition.
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; //Time.deltaTime is essentially the seconds in real time
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration); //Lerp is the translation from point A to point B in a specific second
            yield return null; //This is to make sure it breaks out of the Coroutine cycle
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    IEnumerator FadeOut()
    {
        float timer = 0f; 
        canvasGroup.interactable = false; //to stop any other clicks in that canvas group.

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; 
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration); 
            yield return null; 
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}